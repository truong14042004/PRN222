using PayOS;
using PayOS.Models.V2.PaymentRequests;
using Microsoft.Extensions.Configuration;

namespace StudentManagementApp.BLL.Services;

public class CreatePaymentResult
{
    public string checkoutUrl { get; set; } = "";
    public string qrCode { get; set; } = "";
}

public class WebhookData
{
    public long orderCode { get; set; }
    public int amount { get; set; }
    public string description { get; set; } = "";
    public string status { get; set; } = "";
}

public class WebhookBody
{
    public WebhookData data { get; set; } = new();
    public string signature { get; set; } = "";
}

public interface IPaymentService
{
    Task<CreatePaymentResult> CreatePaymentLink(long orderCode, int amount, string description, string returnUrl, string cancelUrl);
    WebhookData VerifyWebhook(WebhookBody webhookBody);
}

public class PaymentService : IPaymentService
{
    private readonly PayOSClient _payOS;

    public PaymentService(IConfiguration configuration)
    {
        _payOS = new PayOSClient(
            configuration["PayOS:ClientId"] ?? "",
            configuration["PayOS:ApiKey"] ?? "",
            configuration["PayOS:ChecksumKey"] ?? ""
        );
    }

    public async Task<CreatePaymentResult> CreatePaymentLink(long orderCode, int amount, string description, string returnUrl, string cancelUrl)
    {
        // PascalCase properties for SDK v2.1.0
        var request = new CreatePaymentLinkRequest
        {
            OrderCode = orderCode,
            Amount = amount,
            Description = description,
            CancelUrl = cancelUrl,
            ReturnUrl = returnUrl,
            // We can optionally add Items here, but for now we keep it minimal
            Items = new List<PaymentLinkItem> 
            { 
                new PaymentLinkItem { Name = "Don hang #" + orderCode, Quantity = 1, Price = amount } 
            }
        };

        // Correct method path: PaymentRequests.CreateAsync
        var result = await _payOS.PaymentRequests.CreateAsync(request);

        return new CreatePaymentResult
        {
            checkoutUrl = result.CheckoutUrl,
            qrCode = result.QrCode
        };
    }

    public WebhookData VerifyWebhook(WebhookBody webhookBody)
    {
        // Don giản hóa việc verify ở phía webhook để tập trung vào việc tạo link trước.
        return webhookBody.data;
    }
}
