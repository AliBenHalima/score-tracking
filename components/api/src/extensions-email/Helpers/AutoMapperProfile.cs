using AutoMapper;
using ScoreTracking.Extensions.Email.Contratcs;

namespace ScoreTracking.Extensions.Email.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmailQueueEntity, EmailMessage>();
        }
    }
}
