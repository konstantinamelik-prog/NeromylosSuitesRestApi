namespace NeromylosSuites.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IMemberService MemberService { get; }

        public ApplicationService(IUserService userService, IMemberService memberService)
        {
            UserService = userService;
            MemberService = memberService;
        }
    }
}
