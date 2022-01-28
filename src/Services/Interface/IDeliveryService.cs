using System.Collections.Generic;
using System.Threading.Tasks;
using Enum;
using Services.Dto;

namespace Services.Interface
{
    public interface IDeliveryService
    {
        Task Insert(Delivery delivery);

        Task Update(Delivery delivery);

        Task<Delivery> GetDelivery(DeliveryTypeEnum type);

        List<Delivery> GetMany();
    }
}