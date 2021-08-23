using System;
using Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace RestApi.Test.Helper
{
    [TestFixture]
    public class MailHelperTest
    {
        [Test]
        public void ShouldSendMail()
        {
            Assert.Throws<ArgumentNullException>(() =>
            new MailHelper(new Mock<SmtpMailConfig>().Object,
                 new Mock<ILogger<MailHelper>>().Object).SendMail(new Mailer())
            );
        }
    }
}