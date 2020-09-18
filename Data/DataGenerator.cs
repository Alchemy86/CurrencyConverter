using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CurrencyConversionDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<CurrencyConversionDBContext>>()))
            {
                // Look for any board games.
                if (context.CurrencyConversion.Any())
                {
                    return;   // Data was already seeded
                }

                context.CurrencyConversion.AddRange(
                    new CurrencyConversion
                    {
                        ID = 1,
                        BaseCurrency = CurrencyName.GBP,
                        ConvertedCurrency = CurrencyName.USD,
                        Multiplier = 2,
                        ConversionRate = 1.7m,
                        ConversionValue = 2 * 1.7m,
                        DateTime = DateTime.Now
                    },
                    new CurrencyConversion
                    {
                        ID = 2,
                        BaseCurrency = CurrencyName.GBP,
                        ConvertedCurrency = CurrencyName.USD,
                        Multiplier = 2,
                        ConversionRate = 1.7m,
                        ConversionValue = 2 * 1.7m,
                        DateTime = DateTime.Now
                    });

                context.SaveChanges();
            }
        }
    }
}
