using ChairsBackend.Models;

namespace ChairsBackend.Entities
{
    public static class GameManager
    {
        public static int ActiveGameId { get; private set; }
        static GameManager()
        {
            // Set the initial value of ActiveGameId to -1 to indicate no active game
            ActiveGameId = -1;
        }

        public static void SetActiveGame(Game game)
        {
            ActiveGameId = game.Id;
        }
    }
}
