using AutoMapper;
using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;
using NeromylosSuites.Models;
using NeromylosSuites.Repositories;
using System.Linq.Expressions;

namespace NeromylosSuites.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;
        private readonly IPriceCalculationService _priceCalculationService;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<RoomService> logger, IPriceCalculationService priceCalculationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _priceCalculationService = priceCalculationService;
        }

        public async Task<List<RoomReadOnlyDTO>> GetAvailableRoomsByDateRangeAsync(DateTime checkIn, DateTime checkOut)
        {
            var availableRooms = await _unitOfWork.RoomRepository.GetAvailableRoomsByDateRangeAsync(checkIn, checkOut);

            var result = new List<RoomReadOnlyDTO>();
            foreach (var room in availableRooms)
            {
                var dto = _mapper.Map<RoomReadOnlyDTO>(room);
                dto.TotalPrice = await _priceCalculationService.CalculateRoomPriceAsync(room.Id, checkIn, checkOut);
                result.Add(dto);
            }

            return result;
        }

        public async Task<List<BookingReadOnlyDTO>> GetRoomBookingsAsync(string roomName)
        {
            var bookings = await _unitOfWork.RoomRepository.GetRoomBookingsAsync(roomName);

            return _mapper.Map<List<BookingReadOnlyDTO>>(bookings);
        }

        public async Task<PaginatedResult<RoomReadOnlyDTO>> GetPaginatedRoomsFilteredAsync(
            int pageNumber, int pageSize, RoomFiltersDTO roomFiltersDTO)
        {
            List<Expression<Func<Room, bool>>> predicates = [];

            if (roomFiltersDTO.RoomNumber.HasValue)
            {
                predicates.Add(r => r.RoomNumber == roomFiltersDTO.RoomNumber);
            }
            if (!string.IsNullOrEmpty(roomFiltersDTO.Name))
            {
                predicates.Add(r => r.Name == roomFiltersDTO.Name);
            }
            if (!string.IsNullOrEmpty(roomFiltersDTO.Status))
            {
                predicates.Add(r => r.Status == roomFiltersDTO.Status);
            }

            var result = await _unitOfWork.RoomRepository.GetPaginatedRoomsFilteredAsync(pageNumber, pageSize,
                predicates);

            var dtoResult = new PaginatedResult<RoomReadOnlyDTO>()
            {
                Data = _mapper.Map<List<RoomReadOnlyDTO>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            _logger.LogInformation("Retrieved {Count} rooms", dtoResult.Data.Count);
            return dtoResult;
        }
    }
}
