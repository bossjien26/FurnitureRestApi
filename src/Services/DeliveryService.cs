using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Microsoft.EntityFrameworkCore;
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
                    Key = (int)delivery.Type,
                    Value = JsonSerializer.Serialize(delivery)
                }
            );
        }

        public void Update(Delivery delivery)
        {
            var Metadata = GetByCategoryDetail(MetadataCategoryEnum.Delivery, (int)delivery.Type);
            if (Metadata != null)
            {
                Metadata.Value = JsonSerializer.Serialize(delivery);
                Update(Metadata);
            }
        }

        public Delivery GetDelivery(DeliveryTypeEnum type)
        {
            var delivery = GetByCategoryDetail(MetadataCategoryEnum.Delivery, (int)type);

            return delivery == null ? null
            : JsonSerializer.Deserialize<Delivery>(delivery.Value);
        }

        public List<Delivery> GetMany()
        {
            var deliveries = GetByCategory(MetadataCategoryEnum.Delivery).ToListAsync();
            var result = new List<Delivery>();
            deliveries.Result.ForEach(r =>
            {
                result.Add(JsonSerializer.Deserialize<Delivery>(r.Value));
            });
            return result;
        }
    }
}