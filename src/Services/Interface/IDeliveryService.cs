using System.Threading.Tasks;
using Enum;
using Services.Dto;

namespace Services.Interface
{
    public interface IDeliveryService
    {
        Task Insert(Delivery delivery);

        void Update(Delivery delivery);

        Delivery GetDelivery(DeliveryTypeEnum type);
    }
}