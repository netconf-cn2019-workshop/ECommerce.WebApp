using System.Linq;
using ECommerce.WebApp.Controllers;
using ECommerce.WebApp.Models;
using ECommerce.WebApp.Services;
using Moq;
using Xunit;

namespace ECommerce.WebApp.Tests
{
    public class BasketControllerFacts
    {
        [Fact]
        public void should_add_product_to_basket_when_post()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(svc => svc.AddProduct(It.IsAny<Product>())).Verifiable();
            var controller = new BasketController(basketService.Object);
            
            controller.Post(new Product{ Name = "玩具"});

            basketService.Verify();
        }
    }
}