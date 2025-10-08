namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Clearing System Member Identification DTO for SWIFT CBPR+ messages
    /// Maps to ClrSysMmbId partial
    /// Example: GBDSC for UK sort codes
    /// </summary>
    public class ClrSysMmbIdDto
    {
        /// <summary>
        /// Clearing System Identification Code
        /// Example: "GBDSC" for UK Domestic Sort Code
        /// CBPR+ only allows Code, not Proprietary
        /// Mandatory
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Member Identification - the actual clearing code value
        /// Example: "123456" for UK sort code (no hyphens)
        /// Mandatory
        /// </summary>
        public string MmbId { get; set; }
        
        /// <summary>
        /// Validates the Clearing System Member Identification
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Code))
                return ValidationResult.Fail("ClrSysId Code is mandatory");
            
            if (string.IsNullOrWhiteSpace(MmbId))
                return ValidationResult.Fail("MmbId is mandatory");
            
            // For UK sort codes, validate format (6 digits, no hyphens)
            if (Code == "GBDSC")
            {
                if (MmbId.Length != 6)
                    return ValidationResult.Fail("UK sort code (GBDSC) must be 6 digits");
                
                if (!MmbId.All(char.IsDigit))
                    return ValidationResult.Fail("UK sort code (GBDSC) must contain only digits (no hyphens)");
            }
            
            return ValidationResult.Success();
        }
    }
}
