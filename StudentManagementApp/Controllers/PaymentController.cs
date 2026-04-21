using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.Services;
using System.Text.Json;

namespace StudentManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, IOrderService orderService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook([FromBody] object webhookBodyObject)
    {
        try
        {
            var webhookBodyJson = webhookBodyObject.ToString() ?? "";
            _logger.LogInformation("Received PayOS Webhook Raw: {Body}", webhookBodyJson);

            var webhookBody = JsonSerializer.Deserialize<WebhookBody>(webhookBodyJson);
            if (webhookBody == null || webhookBody.data == null)
            {
                return Ok(new { success = true, message = "Handshake or empty body received" });
            }

            // Check if it's a test/ping from PayOS
            if (webhookBody.data.orderCode == 0 || string.IsNullOrEmpty(webhookBody.signature))
            {
                return Ok(new { success = true, message = "Test webhook received" });
            }
            
            // Verify signature and get data
            var webhookData = _paymentService.VerifyWebhook(webhookBody);

            if (webhookData.status == "PAID" || webhookData.status == "SUCCESS") 
            {
                await _orderService.CompleteOrderAsync(webhookData.orderCode);
                return Ok(new { success = true, message = "Order updated successfully" });
            }

            return Ok(new { success = true, message = "Webhook received" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PayOS Webhook");
            // Always return 200 for PayOS during testing if you want to bypass 400 errors, 
            // but for real production, you might want to return 400 on true forgery.
            // For now, let's return 200 OK so PayOS allows saving the URL.
            return Ok(new { success = false, message = "Error but acknowledged" });
        }
    }
}
