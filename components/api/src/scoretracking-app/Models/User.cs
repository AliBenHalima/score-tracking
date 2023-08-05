﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    [Table("users")]
    public class User : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public List<Game> Games { get; } = new();
        [JsonIgnore]
        public List<UserGame> UserGames { get; } = new();
    }
}
