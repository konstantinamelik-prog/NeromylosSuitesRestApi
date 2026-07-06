using AutoMapper;
using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;
using NeromylosSuites.Exceptions;
using NeromylosSuites.Models;
using NeromylosSuites.Repositories;
using System.Linq.Expressions;

namespace NeromylosSuites.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<BookingReadOnlyDTO> CreateBookingAsync(CreateBookingDTO createBooking)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingReadOnlyDTO> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                throw new EntityNotFoundException("Booking", $"Booking with bookingId: {bookingId} not found");
            }

            _logger.LogInformation("Booking found: {BookingId}", bookingId);
            return _mapper.Map<BookingReadOnlyDTO>(booking);
        }

        public async Task<List<BookingReadOnlyDTO>> GetBookingsByUserIdAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User", $"User with userId {userId} not found");
            }

            var bookings = await _unitOfWork.BookingRepository.GetBookingsByUserIdAsync(userId);

            _logger.LogInformation("Retrieved {Count} bookings with userId {UserId}", bookings.Count, userId);
            return _mapper.Map<List<BookingReadOnlyDTO>>(bookings);
        }

        public async Task<List<BookingReadOnlyDTO>> GetBookingsByVisitorIdAsync(int visitorId)
        {
            var visitor = await _unitOfWork.VisitorRepository.GetByIdAsync(visitorId);
            if (visitor == null)
            {
                throw new EntityNotFoundException("Visitor", $"Visitor with visitorId {visitorId} not found");
            }

            var bookings = await _unitOfWork.BookingRepository.GetBookingsByVisitorIdAsync(visitorId);

            _logger.LogInformation("Retrieved {Count} bookings with visitorId {VisitorId}", bookings.Count, visitorId);
            return _mapper.Map<List<BookingReadOnlyDTO>>(bookings);
        }

        public async Task<PaginatedResult<BookingReadOnlyDTO>> GetPaginatedBookingsAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.BookingRepository.GetPaginatedBookingsAsync(pageNumber, pageSize);

            var dtoResult = new PaginatedResult<BookingReadOnlyDTO>()
            {
                Data = _mapper.Map<List<BookingReadOnlyDTO>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            _logger.LogInformation("Retrieved {Count} bookings", dtoResult.Data.Count);
            return dtoResult;
        }

        public async Task<PaginatedResult<BookingReadOnlyDTO>> GetPaginatedBookingsFilteredAsync(
            int pageNumber, int pageSize, BookingFiltersDTO bookingFiltersDTO)
        {
            List<Expression<Func<Booking, bool>>> predicates = [];

            if (bookingFiltersDTO.CheckIn.HasValue)
            {
                predicates.Add(b => b.CheckIn == bookingFiltersDTO.CheckIn);
            }
            if (bookingFiltersDTO.CheckOut.HasValue)
            {
                predicates.Add(b => b.CheckOut == bookingFiltersDTO.CheckOut);
            }
            if (!string.IsNullOrEmpty(bookingFiltersDTO.Status))
            {
                predicates.Add(b => b.Status == bookingFiltersDTO.Status);
            }

            var result = await _unitOfWork.BookingRepository.GetPaginatedBookingsFilteredAsync(pageNumber, pageSize,
                predicates);

            var dtoResult = new PaginatedResult<BookingReadOnlyDTO>()
            {
                Data = _mapper.Map<List<BookingReadOnlyDTO>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            _logger.LogInformation("Retrieved {Count} bookings", dtoResult.Data.Count);
            return dtoResult;
        }
    }
}
