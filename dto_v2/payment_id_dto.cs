namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Payment Identification DTO for SWIFT CBPR+ messages
    /// Contains instruction ID, end-to-end ID, and UETR
    /// Maps to PmtId partial
    /// </summary>
    public class PaymentIdentificationDto
    {
        /// <summary>
        /// Instruction Identification - your internal unique reference
        /// Mandatory
        /// Max length: 35 characters
        /// Note: Operations provide this manually (trade number + date + customer code)
        /// </summary>
        public string InstrId { get; set; }
        
        /// <summary>
        /// End-to-End Identification - reference that travels with the payment
        /// Mandatory
        /// Max length: 35 characters
        /// Typically same as InstrId for originating payments
        /// </summary>
        public string EndToEndId { get; set; }
        
        /// <summary>
        /// Unique End-to-End Transaction Reference (UETR)
        /// Must be a valid UUID v4 format
        /// Mandatory for CBPR+ cross-border payments
        /// Example: "550e8400-e29b-41d4-a716-446655440000"
        /// Generated using CbprFormatter.GenerateUetr()
        /// </summary>
        public string UETR { get; set; }
        
        /// <summary>
        /// Validates the Payment Identification for CBPR+ compliance
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(InstrId))
                return ValidationResult.Fail("InstrId is mandatory");
            
            if (InstrId.Length > 35)
                return ValidationResult.Fail("InstrId exceeds 35 character limit");
            
            if (string.IsNullOrWhiteSpace(EndToEndId))
                return ValidationResult.Fail("EndToEndId is mandatory");
            
            if (EndToEndId.Length > 35)
                return ValidationResult.Fail("EndToEndId exceeds 35 character limit");
            
            if (string.IsNullOrWhiteSpace(UETR))
                return ValidationResult.Fail("UETR is mandatory");
            
            // Validate UETR is a valid GUID format
            if (!Guid.TryParse(UETR, out _))
                return ValidationResult.Fail("UETR must be a valid UUID format");
            
            return ValidationResult.Success();
        }
    }
}
