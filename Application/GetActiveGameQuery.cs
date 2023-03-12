using ChairsBackend.DAL;
using ChairsBackend.Entities;
using ChairsBackend.Models;
using MediatR;
namespace ChairsBackend.Application
{

	public record GetActiveGameQuery() : IRequest<Game>;

	public class GetActiveGameQueryHandler : IRequestHandler<GetActiveGameQuery, Game>
	{
		private readonly ILogger<GetActiveGameQueryHandler> _logger;
        private readonly IRepository _gemesRepo;

        public GetActiveGameQueryHandler(ILogger<GetActiveGameQueryHandler> logger, IRepository gemesRepo)
        {
            _logger = logger;
            _gemesRepo = gemesRepo;
        }

        public async Task<Game> Handle(GetActiveGameQuery request, CancellationToken cancellationToken)
		{
            Game activeGame;
            if (GameManager.ActiveGameId == -1)
            {
                activeGame = await _gemesRepo.GetWithNewestIdAsync();
                GameManager.SetActiveGame(activeGame);
            }
            return await _gemesRepo.GetByIdAsync(GameManager.ActiveGameId);            
        }
	}
}
