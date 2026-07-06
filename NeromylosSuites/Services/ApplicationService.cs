namespace NeromylosSuites.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IMemberService MemberService { get; }
        public IBookingService BookingService { get; }
        public IVisitorService VisitorService { get; }

        public ApplicationService(IUserService userService, IMemberService memberService, IBookingService bookingService,
            IVisitorService visitorService)
        {
            UserService = userService;
            MemberService = memberService;
            BookingService = bookingService;
            VisitorService = visitorService;
        }
    }
}
