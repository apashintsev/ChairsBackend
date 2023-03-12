using ChairsBackend.Application;
using ChairsBackend.Entities;
using ChairsBackend.Models;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ChairsBackend.HostedServices
{
    public class BotsService : BackgroundService
    {
        private readonly ILogger<BotsService> _logger;
        private readonly GameSettings _settings;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BotsService(ILogger<BotsService> logger,
            IOptions<GameSettings> settings,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _settings = settings.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var bots = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                bots.Add($"Player #{i}");
            }
            try
            {
                List<Task> tasks = new();
                foreach (var bot in bots)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        while (true)
                        {
                            _logger.LogInformation($"Start bot ${bot}");
                            var timeBeforeInteraction = RandomNumberGenerator.GetInt32(_settings.PlayerJoinRateSecondsMin, _settings.PlayerJoinRateSecondsMax);
                            Thread.Sleep(timeBeforeInteraction * 1000);
                            _logger.LogInformation($"Bot ${bot} try join game");
                            try
                            {
                                await mediator.Send(new JoinGameCommand(bot), stoppingToken);
                                bool wait = true;
                                do
                                {
                                    var activeGame = await mediator.Send(new GetActiveGameQuery());
                                    Thread.Sleep(1000);
                                    wait = activeGame.Players.Count < 12;
                                } while (wait);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Bot ${bot} cant join game: ${ex.Message}");
                            }

                            try
                            {
                                Thread.Sleep(timeBeforeInteraction * 1000);
                                await mediator.Send(new TakeChairCommand(bot));
                            }
                            catch (Exception e)
                            {
                                _logger.LogWarning($"Bot ${bot} cant take seat: ${e.Message}");
                            }
                            _logger.LogInformation($"Stop bot ${bot}");
                        }

                    }, stoppingToken));
                }
                // Wait for all tasks to complete
                //await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

        }
    }
}
