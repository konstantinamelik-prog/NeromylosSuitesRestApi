using AutoMapper;
using NeromylosSuites.Core;
using NeromylosSuites.DTO;
using NeromylosSuites.Models;
using NeromylosSuites.Repositories;
using Serilog;

namespace NeromylosSuites.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VisitorService> _logger;
        private readonly ILogger<VisitorService> logger = new LoggerFactory().AddSerilog().CreateLogger<VisitorService>();

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

        public async Task<PaginatedResult<VisitorReadOnlyDTO>> GetPaginatedVisitorsAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.VisitorRepository.GetPaginatedVisitorsAsync(pageNumber, pageSize);

            var dtoResult = new PaginatedResult<VisitorReadOnlyDTO>()
            {
                Data = result.Data.Select(v => new VisitorReadOnlyDTO
                {
                    Id = v.Id,
                    Firstname = v.Firstname,
                    Lastname = v.Lastname,
                    Email = v.Email
                }).ToList(),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            logger.LogInformation("Retrieved {Count} visitors", dtoResult.Data.Count);
            return dtoResult;
        }
    }
}
