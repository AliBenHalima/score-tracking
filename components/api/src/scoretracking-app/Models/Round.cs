﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScoreTracking.App.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScoreTracking.App.Models
{
    [Table("rounds")]
    public class Round : Base
    {
        public int Number { get; set; }
        public GameRoundStatus Status { get; set; } = GameRoundStatus.Played;
        [JsonIgnore]
        public List<UserGame> UserGames { get; } = new();
        [JsonIgnore]
        public List<UserGameRound> UserGameRounds { get; } = new();
    }
}
