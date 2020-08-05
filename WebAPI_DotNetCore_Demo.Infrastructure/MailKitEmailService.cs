using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Exceptions;
using WebAPI_DotNetCore_Demo.Application.Infrastructure;
using WebAPI_DotNetCore_Demo.Infrastructure.Options;

namespace WebAPI_DotNetCore_Demo.Infrastructure
{
    public class MailKitEmailService : IEmailService, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _smtpSettings;

        public MailKitEmailService(IOptionsSnapshot<SmtpSettings> smtpSettings)
        {
            // Install-Package MailKit
            _smtpClient = new SmtpClient();
            _smtpSettings = smtpSettings?.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
        }

        public void Dispose()
        {
            if (_smtpClient.IsConnected)
            {
                _smtpClient.Disconnect(true);
            }

            _smtpClient.Dispose();
        }

        public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _smtpClient.Timeout = _smtpSettings.TimeoutMs;
                await _smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port,
                    SecureSocketOptions.StartTls, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new SmtpConnectException(ex.Message, ex);
            }

            return _smtpClient.IsConnected;
        }

        public async Task<bool> SendEmailAsync(string subject, string body,
            string fromAddress, IEnumerable<string> toAddresses,
            IEnumerable<string> ccAddresses = default,
            IEnumerable<string> bccAddresses = default,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var message = GetMessage(subject, body,
                    fromAddress, toAddresses, ccAddresses, bccAddresses);

                // The reason to segregate out ConnectAsync is so that it can be unit tested separately.
                if (!_smtpClient.IsConnected)
                {
                    await ConnectAsync(cancellationToken);
                }
                if (!_smtpClient.IsAuthenticated)
                {
                    await _smtpClient.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Password, cancellationToken);
                }
                await _smtpClient.SendAsync(message, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static MimeMessage GetMessage(string subject, string body,
            string fromAddress, IEnumerable<string> toAddresses,
            IEnumerable<string> ccAddresses, IEnumerable<string> bccAddresses)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(fromAddress),
                Subject = subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                }
            };
            message.To.AddRange(toAddresses.Select(toAddress => MailboxAddress.Parse(toAddress)));

            if (ccAddresses != null)
            {
                message.Cc.AddRange(ccAddresses
                    .Select(ccAddress => MailboxAddress.Parse(ccAddress)));
            }
            if (bccAddresses != null)
            {
                message.Bcc.AddRange(bccAddresses
                    .Select(bccAddress => MailboxAddress.Parse(bccAddress)));
            }

            return message;
        }
    }
}
