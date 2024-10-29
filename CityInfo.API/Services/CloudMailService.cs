namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    private string mailTo = string.Empty;
    private string mailFrom = string.Empty;

    public CloudMailService(IConfiguration configuration)
    {
        mailTo = configuration["mailSettings:mailToAddress"];
        mailFrom = configuration["mailSettings:mailFromAddress"];
    }

    public void Send(string subject, string message)
    {
        // send email - output to console
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(CloudMailService)}");
        Console.WriteLine(subject);
        Console.WriteLine(message);
    }
}
