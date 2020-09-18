using Core.Enums;

namespace Core.Models
{
    public class Currency
    {
        public CurrencyName Name { get; private set; }
        public decimal Value { get; private set; }

        public Currency(CurrencyName name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}
