using ChairsBackend.DAL;
using ChairsBackend.Entities;
using ChairsBackend.Models;
using Gamebitcoin.WebAPI.SignalrHubs;
using MediatR;
using Microsoft.Extensions.Options;

namespace ChairsBackend.Application
{

    public record JoinGameCommand(string Player) : IRequest<Unit>;

    public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, Unit>
    {
        private readonly IRepository _gemesRepo;
        private readonly ILogger<JoinGameCommandHandler> _logger;
        private readonly GameSettings _settings;
        private readonly IGameNotificationService _notyfier;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public JoinGameCommandHandler(ILogger<JoinGameCommandHandler> logger, IRepository gemesRepo, IOptions<GameSettings> settings, IGameNotificationService notyfier)
        {
            _logger = logger;
            _gemesRepo = gemesRepo;
            _settings = settings.Value;
            _notyfier = notyfier;
        }

        public async Task<Unit> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync();
            try
            {
                Game activeGame;
                if (GameManager.ActiveGameId == -1)
                {
                    activeGame = await _gemesRepo.GetWithNewestIdAsync();
                    GameManager.SetActiveGame(activeGame);
                }
                activeGame = await _gemesRepo.GetByIdAsync(GameManager.ActiveGameId);

                if (activeGame.Players.Count == 12)
                {
                    throw new Exception("Game is full of players!");
                }

                if (activeGame.Players.Any(x => x.Name == request.Player))
                {
                    throw new Exception("You are allready in the game!");
                }


                activeGame.AddPlayer(request.Player, _settings.BetValue);
                await _gemesRepo.UpdateAsync(activeGame);
                await _notyfier.PlayerJoinedGame(new Dtos.PlayerJoinedGameDto() { GameNumber= activeGame.Id,Player=  request.Player });
                if (activeGame.Players.Count == 12)
                {
                    activeGame.SetStart(_settings.DurationInSeconds);
                    await _notyfier.GameStarted(new Dtos.GameStartedDto() { GameNumber = activeGame.Id, StartAt = activeGame.StartAt });
                    await _gemesRepo.UpdateAsync(activeGame);
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return Unit.Value;
        }
    }
}
