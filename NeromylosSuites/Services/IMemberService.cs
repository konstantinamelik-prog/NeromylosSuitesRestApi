using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IMemberService
    {
        Task<UserReadOnlyDTO> SignUpMemberAsync(MemberSignupDTO request);
        Task<MemberReadOnlyDTO> GetMemberByPhoneNumberAsync(string phoneNumber);
        Task<MemberReadOnlyDTO> GetUserMemberByUsernameAsync(string username);
        Task<UserReadOnlyDTO> GetUserWithMemberByIdAsync(int userId);
        Task<List<BookingReadOnlyDTO>> GetMemberBookingsAsync(int userId);
        Task<List<MemberReadOnlyDTO>> GetMembersByCountryCodeAsync(string countryCode);
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedMembersFilteredAsync(
            int pageNumber, int pageSize, MemberFiltersDTO memberFiltersDTO);
    }
}
