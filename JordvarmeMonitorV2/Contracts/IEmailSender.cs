namespace JordvarmeMonitorV2.Contracts;

public interface IEmailSender
{
    void Send(string subject, string body);
}