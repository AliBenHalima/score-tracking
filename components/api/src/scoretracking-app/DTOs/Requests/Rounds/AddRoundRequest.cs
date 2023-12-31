﻿using ScoreTracking.App.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Requests.Rounds
{
    public class AddRoundRequest
    {
        public int GameId { get; set; }
        public GameRoundStatus Status { get; set; }
        public IEnumerable<RoundInformationDTO> RoundInformation { get; set; }

    }
}
