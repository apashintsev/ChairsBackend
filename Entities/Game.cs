using System.Net;
using System;
using ChairsBackend.Entities;

namespace ChairsBackend.Models
{
    public class Game:BaseEntity
    {
        public decimal Bank { get; private set; }
        public DateTime StartAt { get; private set; }

        public IList<Player> Players { get; private set; }
        public IList<Player> Winners { get; private set; }

        public Game()
        {
            Players = new List<Player>();
            Winners = new List<Player>();
        } 
        public bool PlayerJoined(string name)
        {
            return Players.Any(p => p.Name == name);
        }

        public void AddPlayer(string name, decimal bet)
        {
            Players.Add(new Player(name));
            Bank += bet;
        }

        public void SetStart(int duration)
        {
            StartAt=DateTime.UtcNow.AddSeconds(duration);
        }

        public void AddWinner(string name)
        {
            Winners.Add(new Player(name));
        }
    }
}
