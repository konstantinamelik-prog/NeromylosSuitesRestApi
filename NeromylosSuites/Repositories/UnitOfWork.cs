using NeromylosSuites.Data;

namespace NeromylosSuites.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NeromylosSuitesMvcContext _context;

        public IUserRepository UserRepository { get; }
        public IMemberRepository MemberRepository { get; }
        public IVisitorRepository VisitorRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public ISeasonalPricesRepository SeasonalPricesRepository { get; }

        public UnitOfWork(NeromylosSuitesMvcContext context)
        {
            _context = context;

            UserRepository = new UserRepository(context);
            MemberRepository = new MemberRepository(context);
            VisitorRepository = new VisitorRepository(context);
            BookingRepository = new BookingRepository(context);
            RoomRepository = new RoomRepository(context);
            SeasonalPricesRepository = new SeasonalPricesRepository(context);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}