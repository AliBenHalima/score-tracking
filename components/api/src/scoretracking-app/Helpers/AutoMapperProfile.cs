using AutoMapper;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScoreTracking.App.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Requests
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<CreateGameRequest, Game>()
            .ForMember(
                dest => dest.Code,
                opt => opt.MapFrom(src => GlobalHelper.generateRandom(1000,9999))
            );

            //DTOs
            CreateMap<Game, CreateGameDTO>();
        }
    }
}
