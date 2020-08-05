using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Infrastructure.Options;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Tests
{
    public class MailKitEmailServiceTests : IDisposable
    {
        private SmtpSettings _smtpSettings;
        private readonly Mock<IOptionsSnapshot<SmtpSettings>> _mockSmtpSettings;

        private readonly MailKitEmailService _mailKitEmailServiceSUT;

        public MailKitEmailServiceTests()
        {
            _smtpSettings = new SmtpSettings();
            _mockSmtpSettings = new Mock<IOptionsSnapshot<SmtpSettings>>();
            _mockSmtpSettings.Setup(x => x.Value).Returns(_smtpSettings);
            _mailKitEmailServiceSUT = new MailKitEmailService(_mockSmtpSettings.Object);
        }

        public void Dispose()
        {
            _mailKitEmailServiceSUT.Dispose();
        }

        [Theory]
        [InlineData("wrong.live.com", 123, 1000)]
        [InlineData("wrongagain.gmail.com", 456, 1000)]
        [InlineData("damnson.office365.com", 789, 1000)]
        public async Task ConnectAsync_InvalidHostAndPort_ThrowsSmtpConnectException(string host, int port, int timeout)
        {
            // Arrange
            _smtpSettings.Host = host;
            _smtpSettings.Port = port;
            _smtpSettings.TimeoutMs = timeout;

            await Assert.ThrowsAsync<SmtpConnectException>(async () =>
            {
                await _mailKitEmailServiceSUT.ConnectAsync();
            });
        }

        [Theory]
        [InlineData("smtp.live.com", 587, 1000)]
        [InlineData("smtp.gmail.com", 587, 1000)]
        [InlineData("smtp.office365.com", 587, 1000)]
        public async Task ConnectAsync_ValidHostAndPort_ReturnsSuccessfulConnection(string host, int port, int timeout)
        {
            // Arrange
            _smtpSettings.Host = host;
            _smtpSettings.Port = port;
            _smtpSettings.TimeoutMs = timeout;

            // Act
            var isConnected = await _mailKitEmailServiceSUT.ConnectAsync();

            // Assert
            Assert.True(isConnected);
        }

        //[Theory]
        [Theory(Skip = "Requires proper email credentials to perform this unit test.")]
        [InlineData("smtp.live.com", 587, "", "")]
        [InlineData("smtp.gmail.com", 587, "", "")]
        [InlineData("smtp.office365.com", 587, "", "")]
        public async Task SendEmailAsync(string host, int port, string user, string password)
        {
            // Arrange
            _smtpSettings.Host = host;
            _smtpSettings.Port = port;
            _smtpSettings.User = user;
            _smtpSettings.Password = password;

            // Act
            var isSentSuccess = await _mailKitEmailServiceSUT.SendEmailAsync(
                "MailKit SMTP EmailService Unit Test",
                "Warmest Regards, Unit Tester.", user,
                new List<string>
                { 
                    "firstRecipient@hotmail.com",
                    "secondRecipient@gmail.com"
                });

            // Assert
            Assert.True(isSentSuccess);
        }
    }
}
