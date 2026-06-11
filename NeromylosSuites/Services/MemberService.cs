using AutoMapper;
using NeromylosSuites.DTO;
using NeromylosSuites.Exceptions;
using NeromylosSuites.Models;
using NeromylosSuites.Repositories;
using NeromylosSuites.Security;

namespace NeromylosSuites.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEncryptionUtil _encryptionUtil;
        private readonly ILogger<MemberService> _logger;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<MemberService> logger, IEncryptionUtil encryptionUtil)
        {
            _encryptionUtil = encryptionUtil;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserReadOnlyDTO> SignUpUserAsync(MemberSignupDTO request)
        {
            var member = _mapper.Map<Member>(request);
            var user = _mapper.Map<User>(request);

            var existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Username);

            if(existingUser != null)
            {
                throw new EntityAlreadyExistsException("User", $"User with username {existingUser.Username} already exists");
            }

            user.Member = member;
            user.Password = _encryptionUtil.Encrypt(user.Password);
            await _unitOfWork.UserRepository.AddAsync(user);

            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Member {Username} signed up successfully.", user.Username);
            return _mapper.Map<UserReadOnlyDTO>(user);
        }
    }
}
