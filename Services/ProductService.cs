﻿using ECommerce.WebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync(string filter);
    }

    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync(string filter)
        {
            var client = _httpClientFactory.CreateClient();
            var catalogServiceHost = _configuration["Services:Catalog"];
            var response = await client.GetStringAsync($"http://{catalogServiceHost}/api/products?includeServiceInfo=true&filter={filter}");
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<Product>>>(response);
            return obj;
        }
    }
}
