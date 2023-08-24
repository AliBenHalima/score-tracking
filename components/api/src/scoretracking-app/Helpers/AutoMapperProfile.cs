using AutoMapper;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Models;
using System;


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
                opt => opt.MapFrom(src => generateRandom(1000, 9999))
            );

            CreateMap<RegisterUserRequest, User>();

            //DTOs
            CreateMap<Game, CreateGameDTO>();
            CreateMap<User, FilterDTO>();
           
        }
        private string generateRandom(int min = 1000, int max = 9999)
        {
            Random random = new Random();
            return random.Next(min, max).ToString();
        }
    }
}
