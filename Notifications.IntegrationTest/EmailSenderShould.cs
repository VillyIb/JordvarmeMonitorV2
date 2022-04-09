using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Transmit;
using Xunit;

namespace JordvarmeMonitorV2.IntegrationTest;

public class EmailSenderShould
{
    private readonly IEmailSender _sut;

    public EmailSenderShould()
    {
        _sut = new EmailSender();
    }

    [Fact(Skip = "Do NOT flood with e-mails")]
    //[Fact]
    public void SendPhysicalEmail()
    {
        _sut.Send("Testing sending E-mail", "Body- testing sending e-mail");
    }
}