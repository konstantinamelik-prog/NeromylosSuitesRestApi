namespace NeromylosSuites.Services
{
    public interface IApplicationService
    {
        IUserService UserService { get; }
        IMemberService MemberService { get; }
        IBookingService BookingService { get; }
        IVisitorService VisitorService { get; }

    }
}
