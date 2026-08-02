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
        private readonly IVisitorService _visitorService;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;
        private readonly IPriceCalculationService _priceCalculationService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<BookingService> logger, IVisitorService visitorService, IPriceCalculationService priceCalculationService)
        {
            _unitOfWork = unitOfWork;
            _visitorService = visitorService;
            _mapper = mapper;
            _logger = logger;
            _priceCalculationService = priceCalculationService;
        }

        private static readonly HashSet<string> ValidStatuses = new()
        {
            BookingStatuses.Pending,
            BookingStatuses.Confirmed,
            BookingStatuses.Cancelled,
            BookingStatuses.Completed
            // BookingStatuses.Deleted μόνο μέσω DELETE endpoint
        };

        public async Task<BookingReadOnlyDTO> CreateBookingAsync(CreateBookingDTO request)
        {
            if (request.CheckOut <= request.CheckIn)
            {
                throw new ArgumentException("Check-out date must be after check-in date.");
            }

            if (request.CheckIn < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Check-in date cannot be in the past.");
            }

            var booking = _mapper.Map<Booking>(request);

            var existingUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email!);

            if (existingUser != null)
            {
                booking.UserId = existingUser.Id;
            }
            else
            {
                var visitorDTO = new CreateVisitorDTO
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CountryCode = request.CountryCode
                };
                var visitor = await _visitorService.CreateVisitorAsync(visitorDTO);
                booking.VisitorId = visitor.Id;
            }

            var availableRooms = await _unitOfWork.RoomRepository
                .GetAvailableRoomsByDateRangeAsync(request.CheckIn!.Value, request.CheckOut!.Value);

            var availableRoomIds = availableRooms.Select(r => r.Id).ToList();

            var requestedRooms = await _unitOfWork.RoomRepository.GetRoomsByIdsAsync(request.RoomIds!); 

            var unavailableRooms = requestedRooms
                .Where(r => !availableRoomIds.Contains(r.Id))
                .ToList();

            if (unavailableRooms.Any())
            {
                var roomNames = string.Join(", ", unavailableRooms.Select(r => r.Name));
                throw new EntityAlreadyExistsException("Room",
                    $"Rooms '{roomNames}' are not available for the selected dates");
            }

            decimal totalPrice = 0;
            foreach (var room in requestedRooms)
            {
                totalPrice += await _priceCalculationService.CalculateRoomPriceAsync(
                    room.Id, request.CheckIn!.Value, request.CheckOut!.Value);
            }

            booking.TotalPrice = totalPrice;
            booking.Rooms = requestedRooms;
            booking.Status = BookingStatuses.Pending;

            await _unitOfWork.BookingRepository.AddAsync(booking);

            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Booking request with id: {BookingId} has been successfully registered.", booking.Id);
            return _mapper.Map<BookingReadOnlyDTO>(booking);
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                throw new EntityNotFoundException("Booking", $"Booking with id: {bookingId} not found");
            }

            booking.Status = BookingStatuses.Deleted;
            await _unitOfWork.BookingRepository.UpdateAsync(booking);
            await _unitOfWork.BookingRepository.DeleteAsync(bookingId);

            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Booking with id {BookingId} deleted successfully", bookingId);
        }

        public async Task<BookingReadOnlyDTO> UpdateBookingStatusAsync(int bookingId, UpdateBookingStatusDTO request)
        {
            if (string.IsNullOrEmpty(request.Status) || !ValidStatuses.Contains(request.Status))
            {
                throw new ArgumentException(
                    $"Invalid status '{request.Status}'. Valid values are: {string.Join(", ", ValidStatuses)}");
            }

            var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                throw new EntityNotFoundException("Booking", $"Booking with id: {bookingId} not found");
            }

            booking.Status = request.Status;
            await _unitOfWork.BookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Booking with id {BookingId} status updated to {Status}", bookingId, request.Status);
            return _mapper.Map<BookingReadOnlyDTO>(booking);
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
