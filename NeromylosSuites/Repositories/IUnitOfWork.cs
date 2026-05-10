namespace NeromylosSuites.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMemberRepository MemberRepository { get; }
        IVisitorRepository VisitorRepository { get; }
        IBookingRepository BookingRepository { get; }
        IRoomRepository RoomRepository { get; }
        ISeasonalPricesRepository SeasonalPricesRepository { get; }

        Task<bool> SaveAsync();
    }
}
