using AutoMapper;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Services.Pharmacy.DTOs.PurchaseOrder;

namespace PulseLogicHMR.Services.Pharmacy.Mapper.PurchaseOrder
{
    public class PurchaseOrderMappingProfile : Profile
    {
        public PurchaseOrderMappingProfile()
        {
            CreateMap<PurchaseOrder_DTO, PHRMPurchaseOrderModel>().ForMember(dest => dest.PHRMPurchaseOrderItems, act => act.Ignore());
            CreateMap<PurchaseOrderItems_DTO, PHRMPurchaseOrderItemsModel>();
        }
    }
}
