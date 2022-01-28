using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class PaymentService : MetadataService, IPaymentService
    {
        public PaymentService(DbContextEntity dbContextEntity) : base(dbContextEntity)
        {
        }

        public async Task Insert(Payment payment)
        {
            await Insert(
                new Metadata()
                {
                    Category = Enum.MetadataCategoryEnum.Pay,
                    Key = (int)payment.Type,
                    Value = JsonSerializer.Serialize(payment)
                }
            );
        }

        public async Task Update(Payment payment)
        {
            var Metadata = await GetByCategoryDetail(MetadataCategoryEnum.Pay, (int)payment.Type);
            if (Metadata != null)
            {
                Metadata.Value = JsonSerializer.Serialize(payment);
                await Update(Metadata);
            }
        }

        public async Task<Payment> GetPayment(PaymentTypeEnum type)
        {
            var payment = await GetByCategoryDetail(MetadataCategoryEnum.Pay, (int)type);

            return payment == null ? null
            : JsonSerializer.Deserialize<Payment>(payment.Value);
        }

        public List<Payment> GetMany()
        {
            var deliveries = GetByCategory(MetadataCategoryEnum.Pay).ToList();
            var result = new List<Payment>();
            deliveries.ForEach(r =>
            {
                result.Add(JsonSerializer.Deserialize<Payment>(r.Value));
            });
            return result;
        }
    }
}