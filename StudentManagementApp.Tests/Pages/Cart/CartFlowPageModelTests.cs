using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.Pages.Cart;
using StudentManagementApp.Tests.TestSupport;
using Xunit;

namespace StudentManagementApp.Tests.Pages.Cart;

public class CartFlowPageModelTests
{
    [Fact]
    public async Task OnPostRemoveAsync_removes_item_by_type_and_item_id_for_logged_in_user()
    {
        var cartService = new CapturingCartService();
        var model = new IndexModel(cartService, new StubOrderService(), new StubPaymentService(), null!);
        var (_, session, _) = model.AttachHttpContext();
        session.SetString("UserId", "42");

        var result = await model.OnPostRemoveAsync("PurchasableItem", 9);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal(42, cartService.RemovedUserId);
        Assert.Equal(OrderItemType.PurchasableItem, cartService.RemovedItemType);
        Assert.Equal(9, cartService.RemovedItemId);
    }

    [Fact]
    public async Task SuccessPage_completes_pending_order_for_current_user()
    {
        var orderService = new CapturingOrderService();
        var model = new SuccessModel(orderService);
        var (_, session, _) = model.AttachHttpContext();
        session.SetString("UserId", "42");
        orderService.Order = CreateOrder(status: "Pending");

        var result = await model.OnGetAsync(orderService.Order.OrderCode);

        Assert.IsType<PageResult>(result);
        Assert.Equal(orderService.Order.OrderCode, model.Order?.OrderCode);
        Assert.Equal(orderService.Order.OrderCode, orderService.CompletedOrderCode);
    }

    [Fact]
    public async Task CancelPage_cancels_pending_order_for_current_user()
    {
        var orderService = new CapturingOrderService();
        var model = new CancelModel(orderService);
        var (_, session, _) = model.AttachHttpContext();
        session.SetString("UserId", "42");
        orderService.Order = CreateOrder(status: "Pending");

        var result = await model.OnGetAsync(orderService.Order.OrderCode);

        Assert.IsType<PageResult>(result);
        Assert.Equal(orderService.Order.OrderCode, orderService.CancelledOrderCode);
    }

    private static OrderDto CreateOrder(string status) => new()
    {
        Id = 7,
        UserId = 42,
        OrderCode = 20260422153000,
        Status = status,
        TotalAmount = 100000,
        PayOSAmount = 100000
    };

    private sealed class CapturingCartService : ICartService
    {
        public int RemovedUserId { get; private set; }
        public OrderItemType RemovedItemType { get; private set; }
        public int RemovedItemId { get; private set; }

        public Task<CartDto> GetCartAsync(int userId) => Task.FromResult(new CartDto());
        public Task AddToCartAsync(int userId, OrderItemType itemType, int itemId) => Task.CompletedTask;

        public Task RemoveFromCartAsync(int userId, OrderItemType itemType, int itemId)
        {
            RemovedUserId = userId;
            RemovedItemType = itemType;
            RemovedItemId = itemId;
            return Task.CompletedTask;
        }

        public Task ClearCartAsync(int userId) => Task.CompletedTask;
    }

    private sealed class CapturingOrderService : IOrderService
    {
        public OrderDto Order { get; set; } = CreateOrder("Pending");
        public long? CompletedOrderCode { get; private set; }
        public long? CancelledOrderCode { get; private set; }

        public Task<OrderDto> CreateOrderAsync(int userId, bool useBalance) => Task.FromResult(Order);
        public Task<OrderDto?> GetByOrderCodeAsync(long orderCode) => Task.FromResult<OrderDto?>(Order.OrderCode == orderCode ? Order : null);

        public Task CompleteOrderAsync(long orderCode)
        {
            CompletedOrderCode = orderCode;
            return Task.CompletedTask;
        }

        public Task CancelOrderAsync(long orderCode)
        {
            CancelledOrderCode = orderCode;
            return Task.CompletedTask;
        }

        public Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId) => Task.FromResult(new List<OrderDto>());
    }

    private sealed class StubOrderService : IOrderService
    {
        public Task<OrderDto> CreateOrderAsync(int userId, bool useBalance) => throw new NotSupportedException();
        public Task<OrderDto?> GetByOrderCodeAsync(long orderCode) => throw new NotSupportedException();
        public Task CompleteOrderAsync(long orderCode) => throw new NotSupportedException();
        public Task CancelOrderAsync(long orderCode) => throw new NotSupportedException();
        public Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId) => throw new NotSupportedException();
    }

    private sealed class StubPaymentService : IPaymentService
    {
        public Task<CreatePaymentResult> CreatePaymentLink(long orderCode, int amount, string description, string returnUrl, string cancelUrl) =>
            throw new NotSupportedException();

        public WebhookData VerifyWebhook(WebhookBody webhookBody) => throw new NotSupportedException();
    }
}
