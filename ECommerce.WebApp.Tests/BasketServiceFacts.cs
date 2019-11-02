using System;
using System.Collections.Generic;
using ECommerce.WebApp.Models;
using ECommerce.WebApp.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ECommerce.WebApp.Tests
{
    public class BasketServiceFacts
    {
        [Fact]
        public void should_add_product()
        {
            var basketService = new BasketService(CreateMockHttpContextAccessor());

            basketService.AddProduct(new Product {Name = "玩具"});

            var itemsInBasket = basketService.GetProducts().ToList();
            Assert.Single(itemsInBasket);
            Assert.Equal("玩具", itemsInBasket.Single().Name);
        }
        
        [Fact]
        public void should_remove_product()
        {
            var basketService = new BasketService(CreateMockHttpContextAccessor());
            basketService.AddProduct(new Product {Name = "玩具"});
            
            basketService.RemoveFromBasket("0");

            var itemsInBasket = basketService.GetProducts().ToList();
            Assert.Empty(itemsInBasket);
        }
        
              
        [Fact]
        public void should_add_product_after_remove()
        {
            var basketService = new BasketService(CreateMockHttpContextAccessor());
            basketService.AddProduct(new Product {Name = "玩具"});
            basketService.RemoveFromBasket("0");
            
            basketService.AddProduct(new Product {Name = "玩具2"});

            var itemsInBasket = basketService.GetProducts().ToList();
            Assert.Single(itemsInBasket);
            Assert.Equal("玩具2", itemsInBasket.Single().Name);
        }

//        [Fact]
//        public void should_add_product_after_remove_and_clear()
//        {
//            var basketService = new BasketService(CreateMockHttpContextAccessor());
//            basketService.AddProduct(new Product {Name = "玩具"});
//            basketService.RemoveFromBasket("0");

//            basketService.Clear();
//            basketService.AddProduct(new Product {Name = "玩具2"});
//
//            var itemsInBasket = basketService.GetProducts().ToList();
//            Assert.Single(itemsInBasket);
//            Assert.Equal("玩具2", itemsInBasket.Single().Name);
//        }
 
        private IHttpContextAccessor CreateMockHttpContextAccessor()
        {
            var httpCtx = new DefaultHttpContext();
            httpCtx.Session = new StubHttpSession();
            
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.SetupGet(acc => acc.HttpContext).Returns(httpCtx);
            return accessor.Object;
        }
    }
    
    
    
    class StubHttpSession : ISession
    {
        readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();

        public object this[string name]
        {
            get => _sessionStorage[name];
            set => _sessionStorage[name] = value;
        }

        string ISession.Id => throw new NotImplementedException();

        bool ISession.IsAvailable => throw new NotImplementedException();

        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;

        void ISession.Clear()
        {
            _sessionStorage.Clear();
        }

        void ISession.Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        void ISession.Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISession.CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage.TryGetValue(key, out var val))
            {
                value = (byte[]) val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }        
    }
}