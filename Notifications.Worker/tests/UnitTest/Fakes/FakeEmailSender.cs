using System.Threading.Tasks;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Infrastructure.Email;

namespace Notifications.Worker.UnitTests.Fakes
{
    public class FakeEmailSender : IEmailSender
    {
        public bool ShouldFail { get; set; } = false;

        public Task SendAsync(string to, string subject, string body)
        {
            if (ShouldFail)
                throw new System.Exception("Simulated email failure");

            return Task.CompletedTask;
        }
    }
}
