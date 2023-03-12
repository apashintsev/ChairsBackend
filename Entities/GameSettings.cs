namespace ChairsBackend.Entities
{
    public class GameSettings
    {
        public int DurationInSeconds { get; set; }
        public decimal BetValue { get; set; }
        public int PlayerJoinRateSecondsMin { get; set; }
        public int PlayerJoinRateSecondsMax { get; set; }
        public int WinnersCount { get; set; }
    }
}
