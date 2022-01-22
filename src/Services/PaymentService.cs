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

        public void Update(Payment payment)
        {
            var Metadata = GetByCategoryDetail(MetadataCategoryEnum.Pay, (int)payment.Type);
            if (Metadata != null)
            {
                Metadata.Value = JsonSerializer.Serialize(payment);
                Update(Metadata);
            }
        }

        public Payment GetPayment(PaymentTypeEnum type)
        {
            var payment = GetByCategoryDetail(MetadataCategoryEnum.Pay, (int)type);

            return payment == null ? null
            : JsonSerializer.Deserialize<Payment>(payment.Value);
        }

        public List<Payment> GetMany()
        {
            var deliveries = GetByCategory(MetadataCategoryEnum.Pay).ToListAsync();
            var result = new List<Payment>();
            deliveries.Result.ForEach(r =>
            {
                result.Add(JsonSerializer.Deserialize<Payment>(r.Value));
            });
            return result;
        }
    }
}