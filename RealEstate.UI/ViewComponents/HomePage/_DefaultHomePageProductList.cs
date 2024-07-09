﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RealEstate.UI.DTOs.ProductDtos;
using RealEstate.UI.Models;

namespace RealEstate.UI.ViewComponents.HomePage
{
    public class _DefaultHomePageProductList:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        public _DefaultHomePageProductList(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // DealOfTheDay alani true olanlar listelenecek (gunun firsati olan ilanlar)
            var client = _httpClientFactory.CreateClient(); // istemci ornegi olusturuldu.
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            var responseMessage = await client.GetAsync("Products/GetProductByDealOfTheDayTrueWithCategoryAsync");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // Gelen icerigi string formatinda oku
                var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);   // DeserializeObject => json bir degeri okuyor ve bizim istedigimiz metin formatina donusturur
                                                                                                 // SerializeObject => metin formatini json formatina donusturur.
                return View(values);
            }
            return View();
        }
    }
}
