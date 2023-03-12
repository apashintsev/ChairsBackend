using ChairsBackend.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace Gamebitcoin.WebAPI.SignalrHubs
{

    public interface IGameNotificationService
    {
        Task GameStarted(GameStartedDto dto);
        Task GameFinished(GameFinishedDto dto);
        Task PlayerJoinedGame(PlayerJoinedGameDto dto);
        Task PlayerWins(PlayerWinsDto dto);
    }

    [AllowAnonymous]
    public class GameHub : Hub<IGameNotificationService>, IGameNotificationService
    {
        private readonly ILogger<GameHub> _logger;
        private readonly IHubContext<GameHub, IGameNotificationService> _context;

        public GameHub(ILogger<GameHub> logger, IHubContext<GameHub, IGameNotificationService> context)
        {
            _logger = logger;
            _context = context;
        }

        public Task GameStarted(GameStartedDto dto)
        {
            _logger.LogInformation($"Hub GameStarted event: {dto.GameNumber}, {dto.StartAt}");
            return _context.Clients.All.GameStarted(dto);
        }

        public Task GameFinished(GameFinishedDto dto)
        {
            _logger.LogInformation($"Hub GameFinished event: {dto.GameNumber}");
            return _context.Clients.All.GameFinished(dto);
        }

        public Task PlayerJoinedGame(PlayerJoinedGameDto dto)
        {
            _logger.LogInformation($"Hub PlayerJoinedGame event: {dto.GameNumber}, {dto.Player}");
            return _context.Clients.All.PlayerJoinedGame(dto);
        }

        public Task PlayerWins(PlayerWinsDto dto)
        {
            _logger.LogInformation($"Hub PlayerWins event: {dto.GameNumber}, {dto.Player}, {dto.Gain}");
            return _context.Clients.All.PlayerWins(dto);
        }
    }
}
