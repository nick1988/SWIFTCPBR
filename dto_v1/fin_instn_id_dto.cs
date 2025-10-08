namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Financial Institution Identification DTO for SWIFT CBPR+ messages
    /// Maps to FinInstnId partial
    /// CBPR+ Rule 1A: BIC excludes Name+Address; without BIC, Name+Address mandatory
    /// </summary>
    public class FinInstnIdDto
    {
        /// <summary>
        /// BIC (Bank Identifier Code) - 8 or 11 characters
        /// If present, Name and PostalAddress must NOT be populated (CBPR+ Rule 1A)
        /// Can be complemented by LEI and/or ClrSysMmbId
        /// </summary>
        public string BIC { get; set; }
        
        /// <summary>
        /// LEI (Legal Entity Identifier) - 20 characters
        /// Can complement BIC or be used with Name+Address
        /// </summary>
        public string LEI { get; set; }
        
        /// <summary>
        /// Clearing System Member Identification
        /// Can complement BIC or be used with Name+Address
        /// Example: GBDSC for UK sort codes
        /// </summary>
        public ClrSysMmbIdDto ClrSysMmbId { get; set; }
        
        /// <summary>
        /// Financial Institution Name
        /// Mandatory if BIC is not present
        /// Must NOT be present if BIC is present (CBPR+ Rule 1A)
        /// Max length: 140 characters
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Postal Address
        /// Mandatory if BIC is not present
        /// Must NOT be present if BIC is present (CBPR+ Rule 1A)
        /// Uses structured address only (no AdrLine)
        /// </summary>
        public PstlAdrDto PostalAddress { get; set; }
        
        /// <summary>
        /// Validates CBPR+ Rule 1A and other compliance requirements
        /// </summary>
        public ValidationResult Validate()
        {
            bool hasBIC = !string.IsNullOrWhiteSpace(BIC);
            bool hasName = !string.IsNullOrWhiteSpace(Name);
            bool hasAddress = PostalAddress != null;
            bool hasNameAndAddress = hasName && hasAddress;
            
            // CBPR+ Rule 1A: BIC and Name+Address are mutually exclusive
            if (hasBIC && (hasName || hasAddress))
            {
                return ValidationResult.Fail(
                    "CBPR+ Rule 1A Violation: When BIC is present, Name and PostalAddress must NOT be provided");
            }
            
            // CBPR+ Rule 1A: Without BIC, must have Name AND Address
            if (!hasBIC && !hasNameAndAddress)
            {
                return ValidationResult.Fail(
                    "CBPR+ Rule 1A Violation: Without BIC, both Name and PostalAddress are mandatory");
            }
            
            // If BIC is present, validate it
            if (hasBIC)
            {
                if (BIC.Length != 8 && BIC.Length != 11)
                    return ValidationResult.Fail("BIC must be 8 or 11 characters");
            }
            
            // If LEI is present, validate it
            if (!string.IsNullOrWhiteSpace(LEI))
            {
                if (LEI.Length != 20)
                    return ValidationResult.Fail("LEI must be 20 characters");
            }
            
            // If Name is present, validate length
            if (hasName && Name.Length > 140)
                return ValidationResult.Fail("Name exceeds 140 character limit");
            
            // Validate nested DTOs
            if (ClrSysMmbId != null)
            {
                var clrSysValidation = ClrSysMmbId.Validate();
                if (!clrSysValidation.IsSuccess)
                    return clrSysValidation;
            }
            
            if (PostalAddress != null)
            {
                var addressValidation = PostalAddress.Validate();
                if (!addressValidation.IsSuccess)
                    return addressValidation;
            }
            
            return ValidationResult.Success();
        }
    }
}
