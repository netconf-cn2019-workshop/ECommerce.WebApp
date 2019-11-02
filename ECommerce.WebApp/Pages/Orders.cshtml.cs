using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.WebApp.Models;
using ECommerce.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.WebApp.Pages
{
    public class OrdersModel : PageModelBase
    {
        private readonly IOrderService _orderService;

        public OrdersModel(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        public IList<Order> Orders { get; private set; }

        public async Task OnGetAsync()
        {
            CurrentPage = AppPage.Orders;

            try
            {
                Orders = await _orderService.GetOrdersAsync();
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
        }

        public static string DisplayOrderStatus(OrderStatus status){
            if(status == OrderStatus.None){
                return "未知";
            }

            var statusString = status.ToString();
            return statusString.Replace(",", "，")
                               .Replace("Submitted", "已提交")
                               .Replace("Packed", "打包完成")
                               .Replace("Payed", "支付完成")
                               .Replace("Shipped", "已发货");
        }
    }
}