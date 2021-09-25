using System.Threading.Tasks;
using Entities;
using Enum;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.Repositories;
using Services;
using Services.Dto;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class PaymentServiceTest : BaseRepositoryTest
    {
        private readonly Mock<MetaData> _entityMock;

        private readonly Mock<IMetaDataRepository> _repoMock;

        private readonly Mock<Payment> _payment;

        public PaymentServiceTest()
        {
            _entityMock = new Mock<MetaData>();
            _repoMock = new Mock<IMetaDataRepository>();
            _payment = new Mock<Payment>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new PaymentService(_context).Insert(_payment.Object));
        }

        [Test]
        public void ShouldUpdate()
        {
            _repoMock.Setup(c => c.Get(x => x.Value == "category"))
                            .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Value = "cart";

            Assert.DoesNotThrow(() =>
                new PaymentService(_context).Update(_payment.Object)
            );
        }

        [Test]
        public void ShouldGetPayment()
        {
            _repoMock.Setup(c => c.Get(x => x.Key == PaymentTypeEnum.Bank.ToString()
            && x.Category == MetaDataCategoryEnum.Pay))
            .Returns(Task.FromResult(_entityMock.Object));


            Assert.DoesNotThrow(() =>
                new PaymentService(_context)
                .GetPayment(PaymentTypeEnum.Bank)
            );
        }
    }
}