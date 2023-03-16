using ChairsBackend.DAL;
using ChairsBackend.Entities;
using ChairsBackend.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace ChairsBackend.Application
{

	public record GetActiveGameQuery() : IRequest<Game>;

	public class GetActiveGameQueryHandler : IRequestHandler<GetActiveGameQuery, Game>
	{
		private readonly ILogger<GetActiveGameQueryHandler> _logger;
        private readonly IRepository _gemesRepo;
        private readonly GameSettings _settings;

        public GetActiveGameQueryHandler(ILogger<GetActiveGameQueryHandler> logger, IOptions<GameSettings> settings, IRepository gemesRepo)
        {
            _logger = logger;
            _gemesRepo = gemesRepo;
            _settings = settings.Value;
        }

        public async Task<Game> Handle(GetActiveGameQuery request, CancellationToken cancellationToken)
		{
            Game activeGame;
            if (GameManager.ActiveGameId == -1)
            {
                activeGame = await _gemesRepo.GetWithNewestIdAsync();
                GameManager.SetActiveGame(activeGame);
            }
            var game = await _gemesRepo.GetByIdAsync(GameManager.ActiveGameId);
            
            if(game.StartAt==DateTime.MinValue && game.Players.Count == 12)
            {
                game.SetStart(_settings.DurationInSeconds);
                await _gemesRepo.UpdateAsync(game);
            }
            if(game.Winners.Count == 6 && game.StartAt.AddSeconds(_settings.DurationInSeconds)<DateTime.UtcNow) {
                game = new Game();

                await _gemesRepo.AddAsync(game);
                GameManager.SetActiveGame(game);
            }
            return game;

        }
	}
}
