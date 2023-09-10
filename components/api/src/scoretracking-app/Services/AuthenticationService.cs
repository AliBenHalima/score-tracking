using AutoMapper;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Providers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using System.Threading.Tasks;

namespace ScoreTracking.App.Services
{
    public class AuthenticationService : Interfaces.Services.IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUploadFileProvider _uploadFileProvider;


        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository, IJwtProvider jwtProvider, IUploadFileProvider uploadFileProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _uploadFileProvider = uploadFileProvider;
        }

        public async Task<User> Register(RegisterUserRequest registerUserRequest) {
            User? userByEmail = await _userRepository.FindByEmail(registerUserRequest.Email);
            if (userByEmail is not null) throw new RessourceNotFoundException("{0} Already Exist", nameof(RegisterUserRequest.Email));

            if(registerUserRequest.Image is not null)
            {
                string? path = await _uploadFileProvider.UploadFile(registerUserRequest.Image, string.Empty);
                registerUserRequest.ImagePath = path;
            }

            User user = _mapper.Map<User>(registerUserRequest);
            user.Password = PasswordManager.HashPassword(registerUserRequest.Password);

            await _userRepository.Create(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<string> Signin(SigninUserRequest signinUserRequest)
        {
           User? user = await _userRepository.FindByEmail(signinUserRequest.Email);
            if (user is null) throw new BadRequestException("Invalid Credentials");
           bool IsPasswordIdentical = PasswordManager.VerifyPassword(user.Password, signinUserRequest.Password);
            if (!IsPasswordIdentical) throw new BadRequestException("Invalid Credentials");
            string token = _jwtProvider.Generate(user);
            return token;
        }
    }
}
