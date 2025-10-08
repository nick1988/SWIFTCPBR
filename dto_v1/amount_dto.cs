namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Amount DTO for currency and value
    /// Used for settlement amounts, instructed amounts, etc.
    /// </summary>
    public class AmountDto
    {
        /// <summary>
        /// Currency code (ISO 4217 3-letter code)
        /// Example: "GBP", "USD", "EUR"
        /// Mandatory
        /// </summary>
        public string Ccy { get; set; }
        
        /// <summary>
        /// Amount value
        /// Example: 1000.50m
        /// Mandatory
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Amount value formatted for SWIFT using CbprFormatter
        /// Automatically handles currency-specific decimal places:
        /// - JPY: 0 decimals
        /// - KWD, BHD, OMR: 3 decimals
        /// - All others: 2 decimals
        /// Used by Scriban template
        /// </summary>
        public string ValueFormatted => CbprFormatter.FormatAmount(Value, Ccy);
        
        /// <summary>
        /// Validates the amount for CBPR+ compliance
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Ccy))
                return ValidationResult.Fail("Currency is mandatory");
            
            if (Ccy.Length != 3)
                return ValidationResult.Fail("Currency must be a 3-letter ISO 4217 code");
            
            if (Value <= 0)
                return ValidationResult.Fail("Amount value must be greater than zero");
            
            return ValidationResult.Success();
        }
    }
}
