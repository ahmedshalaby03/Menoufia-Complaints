using AutoMapper;
using ComplaintSystem.Application.DTOs.Complaints;
using ComplaintSystem.Application.DTOs.Lookups;
using ComplaintSystem.Domain.Entities;

namespace ComplaintSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Complaint, ComplaintListItemDto>();

        CreateMap<Complaint, ComplaintDetailsDto>()
            .ForMember(d => d.GovernorateName, o => o.MapFrom(s => s.Governorate.NameAr))
            .ForMember(d => d.CenterName, o => o.MapFrom(s => s.Center.NameAr))
            .ForMember(d => d.SectorName, o => o.MapFrom(s => s.Sector.NameAr))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.NameAr))
            .ForMember(d => d.GovernmentEntityName, o => o.MapFrom(s => s.GovernmentEntity != null ? s.GovernmentEntity.NameAr : null))
            .ForMember(d => d.CreatedByName, o => o.MapFrom(s => s.CreatedByUser.FullName))
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s => s.AssignedToUser != null ? s.AssignedToUser.FullName : null));

        CreateMap<ComplaintAttachment, AttachmentDto>();

        CreateMap<ComplaintStatusHistory, StatusHistoryDto>()
            .ForMember(d => d.ChangedByName, o => o.MapFrom(s => s.ChangedByUser.FullName));

        CreateMap<Governorate, LookupDto>().ForMember(d => d.NameAr, o => o.MapFrom(s => s.NameAr));
        CreateMap<Sector, LookupDto>();
        CreateMap<GovernmentEntity, LookupDto>();
        CreateMap<Center, CenterLookupDto>();
        CreateMap<Service, ServiceLookupDto>();
    }
}
