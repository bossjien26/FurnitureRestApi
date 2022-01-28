using System.Collections.Generic;
using System.Threading.Tasks;
using Enum;
using Services.Dto;

namespace Services.Interface
{
    public interface IPaymentService : IMetadataService
    {
        Task Insert(Payment payment);

        Task Update(Payment payment);

        Task<Payment> GetPayment(PaymentTypeEnum type);

        List<Payment> GetMany();
    }
}