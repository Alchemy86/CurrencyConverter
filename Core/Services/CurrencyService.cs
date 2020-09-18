using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CurrencyService
    {
        private HttpClient Client { get; }

        public CurrencyService(HttpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Client.BaseAddress = new Uri("https://api.exchangeratesapi.io/");
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Currency>> GetConversionRates(CurrencyName currency)
        {
            var response = await Client.GetAsync($"latest?base={currency}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonSerializer.DeserializeAsync<Root>(responseStream);

            return responseData.rates.GetType().GetProperties().Select(prop =>
            {
                return new Currency((CurrencyName)Enum.Parse(typeof(CurrencyName), prop.Name), Decimal.Parse(prop.GetValue(responseData.rates, null).ToString()));
            });
        }
    }
}
