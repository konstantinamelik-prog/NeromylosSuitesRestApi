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
    public class VisitorService : IVisitorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VisitorService> _logger;

        public VisitorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VisitorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VisitorReadOnlyDTO> CreateVisitorAsync(CreateVisitorDTO request)
        {
            var visitor = _mapper.Map<Visitor>(request);

            var existingVisitor = await _unitOfWork.VisitorRepository.GetVisitorByEmailAsync(visitor.Email);

            if (existingVisitor != null)
            {
                _mapper.Map(request, existingVisitor);
                await _unitOfWork.VisitorRepository.UpdateAsync(existingVisitor);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Visitor {Email} updated successfully", existingVisitor.Email);
                return _mapper.Map<VisitorReadOnlyDTO>(existingVisitor);
            }

            await _unitOfWork.VisitorRepository.AddAsync(visitor);

            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Visitor {Firstname} {Lastname} added successfully", visitor.Firstname, visitor.Lastname);
            return _mapper.Map<VisitorReadOnlyDTO>(visitor);
        }

        public async Task DeleteVisitorAsync(int visitorId)
        {
            var visitor = await _unitOfWork.VisitorRepository.GetByIdAsync(visitorId);
            if (visitor == null)
            {
                throw new EntityNotFoundException("Visitor", $"Visitor with id: {visitorId} not found");
            }

            var hasActiveBookings = await _unitOfWork.BookingRepository.HasActiveBookingsForVisitorAsync(visitorId);
            if (hasActiveBookings)
            {
                throw new EntityHasActiveDependenciesException("Visitor",
                    "Cannot delete visitor with active or completed bookings.");
            }

            await _unitOfWork.VisitorRepository.DeleteAsync(visitorId);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Visitor with id {VisitorId} deleted successfully", visitorId);
        }

        public async Task<VisitorReadOnlyDTO> GetVisitorByPhoneNumberAsync(string phoneNumber)
        {
            var visitor = await _unitOfWork.VisitorRepository.GetVisitorByPhoneNumberAsync(phoneNumber);
            if (visitor == null)
            {
                throw new EntityNotFoundException("Visitor", $"Visitor with phoneNumber: {phoneNumber} not found");
            }

            _logger.LogInformation("Visitor with phoneNumber {PhoneNumber} found", phoneNumber);
            return _mapper.Map<VisitorReadOnlyDTO>(visitor);
        }

        public async Task<VisitorReadOnlyDTO> GetVisitorByIdAsync(int id)
        {
            var visitor = await _unitOfWork.VisitorRepository.GetByIdAsync(id);

            if (visitor == null)
            {
                throw new EntityNotFoundException("Visitor", $"Visitor with id: {id} not found");
            }

            _logger.LogInformation("Visitor with id {Id} found", id);
            return _mapper.Map<VisitorReadOnlyDTO>(visitor);
        }

        public async Task<List<BookingReadOnlyDTO>> GetVisitorBookingsAsync(int visitorId)
        {
            var visitor = await _unitOfWork.VisitorRepository.GetByIdAsync(visitorId);
            if (visitor == null)
            {
                throw new EntityNotFoundException("Visitor", $"Visitor with visitorId: {visitorId} not found");
            }

            var bookings = await _unitOfWork.VisitorRepository.GetVisitorBookingsAsync(visitorId);

            _logger.LogInformation("Retrieved {Count} bookings for visitor {VisitorId}", bookings.Count, visitorId);
            return _mapper.Map<List<BookingReadOnlyDTO>>(bookings);
        }

        public async Task<List<VisitorReadOnlyDTO>> GetVisitorsByCountryCodeAsync(string countryCode)
        {
            var visitors = await _unitOfWork.VisitorRepository.GetVisitorsByCountryCodeAsync(countryCode);

            _logger.LogInformation("Retrieved {Count} visitors with country code {CountryCode}", visitors.Count, countryCode);
            return _mapper.Map<List<VisitorReadOnlyDTO>>(visitors);
        }

        public async Task<PaginatedResult<VisitorReadOnlyDTO>> GetPaginatedVisitorsFilteredAsync(int pageNumber, int pageSize, VisitorFiltersDTO visitorFiltersDTO)
        {
            {
                List<Expression<Func<Visitor, bool>>> predicates = [];

                if (!string.IsNullOrEmpty(visitorFiltersDTO.Email))
                {
                    predicates.Add(v => v.Email == visitorFiltersDTO.Email);
                }
                if (!string.IsNullOrEmpty(visitorFiltersDTO.Lastname))
                {
                    predicates.Add(v => v.Lastname == visitorFiltersDTO.Lastname);
                }
                if (!string.IsNullOrEmpty(visitorFiltersDTO.CountryCode))
                {
                    predicates.Add(v => v.CountryCode == visitorFiltersDTO.CountryCode);
                }

                var result = await _unitOfWork.VisitorRepository.GetPaginatedVisitorsFilteredAsync(pageNumber, pageSize,
                    predicates);

                var dtoResult = new PaginatedResult<VisitorReadOnlyDTO>()
                {
                    Data = _mapper.Map<List<VisitorReadOnlyDTO>>(result.Data),
                    TotalRecords = result.TotalRecords,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize
                };

                _logger.LogInformation("Retrieved {Count} users", dtoResult.Data.Count);
                return dtoResult;
            }
        }
    }
}
