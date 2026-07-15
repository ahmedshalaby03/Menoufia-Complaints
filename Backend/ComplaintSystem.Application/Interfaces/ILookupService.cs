using ComplaintSystem.Application.DTOs.Lookups;

namespace ComplaintSystem.Application.Interfaces;

public interface ILookupService
{
    Task<List<LookupDto>> GetGovernoratesAsync();
    Task<List<CenterLookupDto>> GetCentersAsync(int? governorateId = null);
    Task<List<LookupDto>> GetSectorsAsync();
    Task<List<ServiceLookupDto>> GetServicesAsync(int? sectorId = null);
    Task<List<LookupDto>> GetGovernmentEntitiesAsync();
}
