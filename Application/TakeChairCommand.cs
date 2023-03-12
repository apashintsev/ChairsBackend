using ChairsBackend.DAL;
using ChairsBackend.Entities;
using ChairsBackend.Models;
using Gamebitcoin.WebAPI.SignalrHubs;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;

namespace ChairsBackend.Application
{
    public record TakeChairCommand(string Name) : IRequest<Unit>;

    public class TakeChairCommandHandler : IRequestHandler<TakeChairCommand, Unit>
    {
        private readonly ILogger<TakeChairCommandHandler> _logger;
        private readonly IRepository _gemesRepo;
        private readonly GameSettings _settings;
        private readonly IGameNotificationService _notyfier;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public TakeChairCommandHandler(ILogger<TakeChairCommandHandler> logger, IRepository gemesRepo, IOptions<GameSettings> settings, IGameNotificationService notyfier)
        {
            _logger = logger;
            _gemesRepo = gemesRepo;
            _settings = settings.Value;
            _notyfier = notyfier;
        }

        public async Task<Unit> Handle(TakeChairCommand request, CancellationToken cancellationToken)
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

                if (activeGame.StartAt > DateTime.UtcNow || activeGame.StartAt == DateTime.MinValue)
                {
                    throw new Exception("Current Game is not started!");
                }
                if (!activeGame.PlayerJoined(request.Name))
                {
                    throw new Exception("You aren't in the game");
                }
                if (activeGame.Winners.Count == _settings.WinnersCount)
                {
                    throw new Exception("Game over, you loose");
                }


                activeGame.AddWinner(request.Name);
                await _gemesRepo.UpdateAsync(activeGame);
                await _notyfier.PlayerWins(new Dtos.PlayerWinsDto() { GameNumber = activeGame.Id, Player = request.Name, Gain = activeGame.Bank / _settings.WinnersCount });
                if (activeGame.Winners.Count == _settings.WinnersCount)
                {
                    await _notyfier.GameFinished(new Dtos.GameFinishedDto() { GameNumber = activeGame.Id });
                    var newGame = new Game();

                    await _gemesRepo.AddAsync(newGame);
                    GameManager.SetActiveGame(newGame);

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
