using System.Text.Json;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class DeliveryService : MetadataService, IDeliveryService
    {
        public DeliveryService(DbContextEntity dbContextEntity) : base(dbContextEntity)
        {

        }

        public async Task Insert(Delivery delivery)
        {
            await Insert(
                new Metadata()
                {
                    Category = Enum.MetadataCategoryEnum.Delivery,
                    Type = (int)delivery.Type,
                    Key = delivery.Type.ToString(),
                    Value = JsonSerializer.Serialize(delivery)
                }
            );
        }

        public void Update(Delivery delivery)
        {
            var Metadata = GetByCategory(MetadataCategoryEnum.Delivery, (int)delivery.Type);
            if (Metadata != null)
            {
                Metadata.Value = JsonSerializer.Serialize(delivery);
                Update(Metadata);
            }
        }

        public Delivery GetDelivery(DeliveryTypeEnum type)
        {
            var delivery = GetByCategory(MetadataCategoryEnum.Delivery, (int)type);

            return delivery == null ? null
            : JsonSerializer.Deserialize<Delivery>(delivery.Value);
        }
    }
}