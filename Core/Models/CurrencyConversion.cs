using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class CurrencyConversion : IValidatableObject
    {
        public int ID { get; set; }
        public CurrencyName BaseCurrency { get; set; }

        public CurrencyName ConvertedCurrency { get; set; }

        public decimal ConversionRate { get; set; }

        public decimal Multiplier { get; set; }

        public decimal ConversionValue { get; set; }

        public DateTime DateTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime > DateTime.Now) {
                yield return new ValidationResult("Entries cannot be subnmitted in the future", new string[] { nameof(DateTime) });
            }

            if (BaseCurrency != CurrencyName.GBP) {
                yield return new ValidationResult("Conversion has been limited to GBP", new string[] { nameof(BaseCurrency) });
            }
        }
    }
}
