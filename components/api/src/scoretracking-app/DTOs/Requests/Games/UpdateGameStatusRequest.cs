﻿using DataAnnotationsExtensions;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Validations;
using ScoreTracking.App.Validations.UserValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.DTOs.Requests.Games
{
    public class UpdateGameStatusRequest
    {
       public GameStatus Status { get; set; }
    }
}
