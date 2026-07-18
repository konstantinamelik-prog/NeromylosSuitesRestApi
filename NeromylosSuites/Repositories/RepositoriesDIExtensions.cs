namespace NeromylosSuites.Repositories
{
    public static class RepositoriesDIExtensions
    {

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IVisitorRepository, VisitorRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ISeasonalPricesRepository, SeasonalPricesRepository>();

            return services;
        }
    }
}
