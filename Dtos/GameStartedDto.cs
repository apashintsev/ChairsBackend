namespace ChairsBackend.Dtos
{
    public class GameStartedDto
    {
        public int GameNumber { get; set; }
        public DateTime StartAt { get; set; }
    }

    public class GameFinishedDto
    {
        public int GameNumber { get; set; }
    }
    public class PlayerJoinedGameDto
    {
        public int GameNumber { get; set; }
        public string Player { get; set; }
    }
    public class PlayerWinsDto
    {
        public int GameNumber { get; set; }
        public string Player { get; set; }
        public decimal Gain { get; set; }
    }

}
