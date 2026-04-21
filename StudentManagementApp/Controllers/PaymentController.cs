using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using StudentManagementApp.BLL.Services;

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
    public async Task<IActionResult> HandleWebhook([FromBody] WebhookType webhookBody)
    {
        try
        {
            _logger.LogInformation("Received PayOS Webhook: {OrderCode}", webhookBody.data.orderCode);
            
            // Verify signature
            var webhookData = _paymentService.VerifyWebhook(webhookBody);

            if (webhookData.status == "PAID")
            {
                await _orderService.CompleteOrderAsync(webhookData.orderCode);
                return Ok(new { success = true, message = "Order updated successfully" });
            }

            return Ok(new { success = true, message = "Webhook received but status not PAID" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PayOS Webhook");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}
