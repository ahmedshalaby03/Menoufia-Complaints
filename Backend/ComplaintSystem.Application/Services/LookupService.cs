using AutoMapper;
using ComplaintSystem.Application.DTOs.Lookups;
using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Interfaces;

namespace ComplaintSystem.Application.Services;

public class LookupService : ILookupService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public LookupService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<List<LookupDto>> GetGovernoratesAsync() =>
        _mapper.Map<List<LookupDto>>(await _uow.Governorates.GetAllAsync());

    public async Task<List<CenterLookupDto>> GetCentersAsync(int? governorateId = null)
    {
        var query = _uow.Centers.Query();
        if (governorateId.HasValue) query = query.Where(c => c.GovernorateId == governorateId);
        return _mapper.Map<List<CenterLookupDto>>(query.ToList());
    }

    public async Task<List<LookupDto>> GetSectorsAsync() =>
        _mapper.Map<List<LookupDto>>(await _uow.Sectors.GetAllAsync());

    public async Task<List<ServiceLookupDto>> GetServicesAsync(int? sectorId = null)
    {
        var query = _uow.Services.Query();
        if (sectorId.HasValue) query = query.Where(s => s.SectorId == sectorId);
        return _mapper.Map<List<ServiceLookupDto>>(query.ToList());
    }

    public async Task<List<LookupDto>> GetGovernmentEntitiesAsync() =>
        _mapper.Map<List<LookupDto>>((await _uow.GovernmentEntities.GetAllAsync()).Where(e => e.IsActive).ToList());
}
