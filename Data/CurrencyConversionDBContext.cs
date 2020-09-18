using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Data
{
    public class CurrencyConversionDBContext : DbContext
    {
        public CurrencyConversionDBContext(DbContextOptions<CurrencyConversionDBContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyConversion> CurrencyConversion { get; set; }
    }
}
