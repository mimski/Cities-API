namespace CityInfo.API.Services;

public class LocalMailService : IMailService
{
    private string mailTo = string.Empty;
    private string mailFrom = string.Empty;

    public LocalMailService(IConfiguration configuration)
    {
        mailTo = configuration["mailSettings:mailToAddress"];
        mailFrom = configuration["mailSettings:mailFromAddress"];
    }

    public void Send(string subject, string message)
    {
        // send email - output to console
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine(subject);
        Console.WriteLine(message);
    }
}
