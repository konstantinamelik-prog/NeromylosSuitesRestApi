using NeromylosSuites.Models;

namespace NeromylosSuites.Services
{
    public interface IUserService
    {
        Task<User> VerifyAndGetUserAsync();
    }
}
