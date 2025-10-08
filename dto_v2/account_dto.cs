namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Account Identification DTO for SWIFT CBPR+ messages
    /// Used for DbtrAcct, CdtrAcct, SttlmAcct, etc.
    /// Maps to Acct partial
    /// IBAN and OtherAccountId are mutually exclusive - only populate one
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// International Bank Account Number (IBAN)
        /// Use this when available - preferred by CBPR+
        /// Will be validated and formatted by CbprFormatter
        /// </summary>
        public string? IBAN { get; set; }
        
        /// <summary>
        /// Other Account Identification - used for non-IBAN accounts
        /// Only populate this if IBAN is not available
        /// Examples: UK account number, US account number, etc.
        /// </summary>
        public string? OtherAccountId { get; set; }
        
        /// <summary>
        /// Validates that only one identification type is populated
        /// </summary>
        public ValidationResult Validate()
        {
            bool hasIBAN = !string.IsNullOrWhiteSpace(IBAN);
            bool hasOther = !string.IsNullOrWhiteSpace(OtherAccountId);
            
            // Must have exactly one (XOR logic)
            if (!hasIBAN && !hasOther)
                return ValidationResult.Fail("Account must have either IBAN or OtherAccountId");
            
            if (hasIBAN && hasOther)
                return ValidationResult.Fail("Account cannot have both IBAN and OtherAccountId - they are mutually exclusive");
            
            // Validate IBAN format if present
            if (hasIBAN)
            {
                try
                {
                    // Use CbprFormatter to validate IBAN
                    CbprFormatter.FormatIban(IBAN);
                }
                catch (ArgumentException ex)
                {
                    return ValidationResult.Fail($"Invalid IBAN: {ex.Message}");
                }
            }
            
            return ValidationResult.Success();
        }
    }
}
