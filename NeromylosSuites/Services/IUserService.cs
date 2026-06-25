using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;

namespace NeromylosSuites.Services
{
    public interface IUserService
    {
        Task<LoginResultDTO> LoginAsync(UserLoginDTO credentials);
        Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username);
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReadOnlyDTO> GetUserByEmailAsync(string email);
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersFilteredAsync
            (int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO);
    }
}
