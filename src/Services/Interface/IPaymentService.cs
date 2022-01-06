using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Enum;
using Services.Dto;

namespace Services.Interface
{
    public interface IPaymentService : IMetadataService
    {
        Task Insert(Payment payment);

        void Update(Payment payment);

        Payment GetPayment(PaymentTypeEnum type);

        List<Payment> GetMany();
    }
}