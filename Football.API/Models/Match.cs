using System.Collections.Generic;
using System;

namespace Football.API.Models
{
    public class Match
    {
        public int Id { get; set; }

        public Manager HouseManager { get; set; }
        public Manager AwayManager { get; set; }

        public ICollection<Player> HousePlayers { get; set; }
        public ICollection<Player> AwayPlayers { get; set; }

        public Referee Referee { get; set; }

        public DateTime Date { get; set; }
    }
}
