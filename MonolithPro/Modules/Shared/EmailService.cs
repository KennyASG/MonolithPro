namespace MonolithPro.Shared;

public class EmailService
{
    private readonly LoggerService _logger;

    public EmailService(LoggerService logger)
    {
        _logger = logger;
    }

    public void SendEmail(string to, string subject, string body)
    {
        _logger.Log($"Sending email to: {to}");
        _logger.Log($"Subject: {subject}");
        _logger.Log($"Body: {body}");
        // Simulación de envío de email
    }

    public void SendWelcomeEmail(string email, string name)
    {
        SendEmail(email, "Welcome!", $"Hello {name}, welcome to MonolithPro!");
    }

    public void SendOrderConfirmation(string email, int orderId)
    {
        SendEmail(email, "Order Confirmation", $"Your order #{orderId} has been confirmed!");
    }
}