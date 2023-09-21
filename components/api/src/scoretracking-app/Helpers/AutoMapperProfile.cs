using AutoMapper;
using Nest;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.DTOs.Users;
using ScoreTracking.App.Models;
using System;
using System.Linq;

namespace ScoreTracking.App.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
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
            CreateMap<User, UserElasticsearchDto>()
                .ForMember(
                dest => dest.Location,
                opt => opt.MapFrom(src => new GeoLocation(src.Latitude ?? default(double), src.Longitude ?? default(double))

            ));
            CreateMap<IHit<UserElasticsearchDto>, UserByDistanceDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Source.Id))
                .ForMember(
                dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Source.FirstName))
                .ForMember(
                dest => dest.LastName,
                opt => opt.MapFrom(src => src.Source.LastName))
                .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Source.Email))
                .ForMember(
                dest => dest.Phone,
                opt => opt.MapFrom(src => src.Source.Phone))
                .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.Source.CreatedAt))
                 .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.Source.UpdatedAt))
                  .ForMember(
                dest => dest.Distance,
                opt => opt.MapFrom(src => src.Sorts.FirstOrDefault()));
        }
        private string generateRandom(int min = 1000, int max = 9999)
        {
            Random random = new Random();
            return random.Next(min, max).ToString();
        }
    }
}
