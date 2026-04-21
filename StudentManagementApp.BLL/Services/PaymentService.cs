using Net.payOS;
using Net.payOS.Types;
using Microsoft.Extensions.Configuration;

namespace StudentManagementApp.BLL.Services;

public interface IPaymentService
{
    Task<CreatePaymentResult> CreatePaymentLink(long orderCode, int amount, string description, string returnUrl, string cancelUrl);
    WebhookData VerifyWebhook(WebhookType webhookType);
}

public class PaymentService : IPaymentService
{
    private readonly Net.payOS.PayOS _payOS;

    public PaymentService(IConfiguration configuration)
    {
        var clientId = configuration["PayOS:ClientId"] ?? throw new Exception("PayOS ClientId is missing");
        var apiKey = configuration["PayOS:ApiKey"] ?? throw new Exception("PayOS ApiKey is missing");
        var checksumKey = configuration["PayOS:ChecksumKey"] ?? throw new Exception("PayOS ChecksumKey is missing");

        _payOS = new Net.payOS.PayOS(clientId, apiKey, checksumKey);
    }

    public async Task<CreatePaymentResult> CreatePaymentLink(long orderCode, int amount, string description, string returnUrl, string cancelUrl)
    {
        var paymentData = new PaymentData(orderCode, amount, description, null, cancelUrl, returnUrl);
        return await _payOS.createPaymentLink(paymentData);
    }

    public WebhookData VerifyWebhook(WebhookType webhookType)
    {
        return _payOS.verifyPaymentWebhookData(webhookType);
    }
}
