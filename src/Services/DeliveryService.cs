using System.Text.Json;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class DeliveryService : MetaDataService, IDeliveryService
    {
        public DeliveryService(DbContextEntity dbContextEntity) : base(dbContextEntity)
        {

        }

        public async Task Insert(Delivery delivery)
        {
            await Insert(
                new MetaData()
                {
                    Category = Enum.MetaDataCategoryEnum.Delivery,
                    Type = (int)delivery.Type,
                    Key = delivery.Type.ToString(),
                    Value = JsonSerializer.Serialize(delivery)
                }
            );
        }

        public void Update(Delivery delivery)
        {
            var metaData = GetByCategory(MetaDataCategoryEnum.Delivery, (int)delivery.Type);
            if (metaData != null)
            {
                metaData.Value = JsonSerializer.Serialize(delivery);
                Update(metaData);
            }
        }

        public Delivery GetDelivery(DeliveryTypeEnum type)
        {
            var delivery = GetByCategory(MetaDataCategoryEnum.Delivery, (int)type);

            return delivery == null ? null
            : JsonSerializer.Deserialize<Delivery>(delivery.Value);
        }
    }
}