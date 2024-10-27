namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    private string mailTo = "admin@mycompany.com";
    private string mailFrom = "noreply@mycompany.com";

    public void Send(string subject, string message)
    {
        // send email - output to console
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(CloudMailService)}");
        Console.WriteLine(subject);
        Console.WriteLine(message);
    }
}
