namespace NeromylosSuites.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IMemberService MemberService { get; }
        public IBookingService BookingService { get; }
        public IVisitorService VisitorService { get; }
        public IRoomService RoomService { get; }

        public ApplicationService(IUserService userService, IMemberService memberService, IBookingService bookingService,
            IVisitorService visitorService, IRoomService roomService)
        {
            UserService = userService;
            MemberService = memberService;
            BookingService = bookingService;
            VisitorService = visitorService;
            RoomService = roomService;
        }
    }
}
