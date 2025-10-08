namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Party DTO for SWIFT CBPR+ messages
    /// Used for Debtor, Creditor, Ultimate Debtor, Ultimate Creditor
    /// Maps to Party partial
    /// </summary>
    public class PartyDto
    {
        /// <summary>
        /// Party Name (organisation name)
        /// Mandatory
        /// Max length: 140 characters
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Postal Address
        /// Mandatory - uses structured address only (no AdrLine)
        /// </summary>
        public PstlAdrDto PostalAddress { get; set; }
        
        /// <summary>
        /// Party Identification (BIC/LEI)
        /// Optional - only populate when available
        /// </summary>
        public PartyIdentificationDto PartyIdentification { get; set; }
        
        /// <summary>
        /// Validates the Party for CBPR+ compliance
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return ValidationResult.Fail("Party Name is mandatory");
            
            if (Name.Length > 140)
                return ValidationResult.Fail("Party Name exceeds 140 character limit");
            
            if (PostalAddress == null)
                return ValidationResult.Fail("Party PostalAddress is mandatory");
            
            var addressValidation = PostalAddress.Validate();
            if (!addressValidation.IsSuccess)
                return addressValidation;
            
            // PartyIdentification is optional, but if present, validate it
            if (PartyIdentification != null)
            {
                var idValidation = PartyIdentification.Validate();
                if (!idValidation.IsSuccess)
                    return idValidation;
            }
            
            return ValidationResult.Success();
        }
    }
}
