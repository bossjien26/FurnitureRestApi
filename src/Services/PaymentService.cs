using System.Text.Json;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class PaymentService : MetaDataService, IPaymentService
    {
        public PaymentService(DbContextEntity dbContextEntity) : base(dbContextEntity)
        {
        }

        public async Task Insert(Payment payment)
        {
            await Insert(
                new MetaData()
                {
                    Category = Enum.MetaDataCategoryEnum.Pay,
                    Type = (int)payment.Type,
                    Key = payment.Type.ToString(),
                    Value = JsonSerializer.Serialize(payment)
                }
            );
        }

        public void Update(Payment payment)
        {
            var metaData = GetByCategory(MetaDataCategoryEnum.Pay, (int)payment.Type);
            if (metaData != null)
            {
                metaData.Value = JsonSerializer.Serialize(payment);
                Update(metaData);
            }
        }

        public Payment GetPayment(PaymentTypeEnum type)
        {
            var payment = GetByCategory(MetaDataCategoryEnum.Pay, (int)type);

            return payment == null ? null
            : JsonSerializer.Deserialize<Payment>(payment.Value);
        }
    }
}