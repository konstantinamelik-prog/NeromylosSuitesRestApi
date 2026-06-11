using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IMemberService
    {
        Task<UserReadOnlyDTO> SignUpUserAsync(MemberSignupDTO request);
    }
}
