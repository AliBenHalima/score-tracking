using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Repositories
{
    public class RoundRepository : BaseRepository<Game>, IRoundRepository
    {
        public RoundRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}