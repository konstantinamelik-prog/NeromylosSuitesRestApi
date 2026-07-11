using AutoMapper;
using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;
using NeromylosSuites.Exceptions;
using NeromylosSuites.Models;
using NeromylosSuites.Repositories;
using NeromylosSuites.Security;
using System.Linq.Expressions;

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

        public async Task<UserReadOnlyDTO> SignUpMemberAsync(MemberSignupDTO request)
        {
            var member = _mapper.Map<Member>(request);
            var user = _mapper.Map<User>(request);

            var existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Username);

            if (existingUser != null)
            {
                throw new EntityAlreadyExistsException("User", $"User with username {existingUser.Username} already exists");
            }

            var existingEmail = await _unitOfWork.UserRepository.GetUserByEmailAsync(user.Email);
            if (existingEmail != null)
            {
                throw new EntityAlreadyExistsException("User", $"User with email {existingEmail.Email} already exists");
            }

            user.Member = member;
            user.Password = _encryptionUtil.Encrypt(user.Password);
            await _unitOfWork.UserRepository.AddAsync(user);

            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Member {Username} signed up successfully.", user.Username);
            return _mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<MemberReadOnlyDTO> GetMemberByPhoneNumberAsync(string phoneNumber)
        {
            var member = await _unitOfWork.MemberRepository.GetMemberByPhoneNumberAsync(phoneNumber);
            if (member == null)
            {
                throw new EntityNotFoundException("Member", $"Member with phoneNumber: {phoneNumber} not found");
            }

            _logger.LogInformation("Member with phonenumber {PhoneNumber} found", phoneNumber);
            return _mapper.Map<MemberReadOnlyDTO>(member);
        }

        public async Task<MemberReadOnlyDTO> GetUserMemberByUsernameAsync(string username)
        {
            var member = await _unitOfWork.MemberRepository.GetUserMemberByUsernameAsync(username);
            if (member == null)
            {
                throw new EntityNotFoundException("Member", $"Member with username: {username} not found");
            }

            _logger.LogInformation("Member with username {Username} found", username);
            return _mapper.Map<MemberReadOnlyDTO>(member);
        }

        public async Task<UserReadOnlyDTO> GetUserWithMemberByIdAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithMemberByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User", $"User with userId: {userId} not found");
            }

            _logger.LogInformation("User with userId {userId} found", userId);
            return _mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<List<BookingReadOnlyDTO>> GetMemberBookingsAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithMemberByIdAsync(userId);
            if (user == null || user.Member == null)
            {
                throw new EntityNotFoundException("Member", $"Member with userId: {userId} not found");
            }

            var bookings = await _unitOfWork.MemberRepository.GetMemberBookingsAsync(userId);
            
            _logger.LogInformation("Retrieved {Count} bookings for user {UserId}", bookings.Count, userId);
            return _mapper.Map<List<BookingReadOnlyDTO>>(bookings);
        }

        public async Task<List<MemberReadOnlyDTO>> GetMembersByCountryCodeAsync(string countryCode)
        {
            var members = await _unitOfWork.MemberRepository.GetMembersByCountryCodeAsync(countryCode);
            
            _logger.LogInformation("Retrieved {Count} members with country code {CountryCode}", members.Count, countryCode);
            return _mapper.Map<List<MemberReadOnlyDTO>>(members);
        }

        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedMembersAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.MemberRepository.GetPaginatedUsersMembersAsync(pageNumber, pageSize);

            var dtoResult = new PaginatedResult<UserReadOnlyDTO>()
            {
                Data = result.Data.Select(u => new UserReadOnlyDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Email = u.Email,
                    UserRole = u.Role.Name
                }).ToList(),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            _logger.LogInformation("Retrieved {Count} users-members", dtoResult.Data.Count);
            return dtoResult;
        }

        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedMembersFilteredAsync(
            int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            List<Expression<Func<User, bool>>> predicates = [];

            if (!string.IsNullOrEmpty(userFiltersDTO.Username))
            {
                predicates.Add(u => u.Username == userFiltersDTO.Username);
            }
            if (!string.IsNullOrEmpty(userFiltersDTO.Email))
            {
                predicates.Add(u => u.Email == userFiltersDTO.Email);
            }
            if (!string.IsNullOrEmpty(userFiltersDTO.Lastname))
            {
                predicates.Add(u => u.Lastname == userFiltersDTO.Lastname);
            }
            if (!string.IsNullOrEmpty(userFiltersDTO.UserRole))
            {
                predicates.Add(u => u.Role.Name == userFiltersDTO.UserRole);
            }

            var result = await _unitOfWork.UserRepository.GetPaginatedUsersAsync(pageNumber, pageSize,
                predicates);

            var dtoResult = new PaginatedResult<UserReadOnlyDTO>()
            {
                Data = _mapper.Map<List<UserReadOnlyDTO>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            _logger.LogInformation("Retrieved {Count} users", dtoResult.Data.Count);
            return dtoResult;
        }
    }
}
