namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Party Identification DTO for organisation identification
    /// Contains BIC and/or LEI - at least one should be provided if this object is populated
    /// Maps to PartyId partial
    /// Simpler than FinInstnId - only BIC/LEI, no clearing system or other identification
    /// </summary>
    public class PartyIdentificationDto
    {
        /// <summary>
        /// Any BIC (BIC8 or BIC11 format)
        /// BIC11 preferred where possible
        /// Note: Uses AnyBIC element (not BICFI) for party identification
        /// </summary>
        public string? BIC { get; set; }
        
        /// <summary>
        /// Legal Entity Identifier (LEI)
        /// 20 character alphanumeric code
        /// </summary>
        public string? LEI { get; set; }
        
        /// <summary>
        /// Validates that at least one identification is provided
        /// </summary>
        public ValidationResult Validate()
        {
            bool hasBIC = !string.IsNullOrWhiteSpace(BIC);
            bool hasLEI = !string.IsNullOrWhiteSpace(LEI);
            
            if (!hasBIC && !hasLEI)
                return ValidationResult.Fail("PartyIdentification must have at least one of BIC or LEI");
            
            // Validate BIC if present
            if (hasBIC)
            {
                if (BIC.Length != 8 && BIC.Length != 11)
                    return ValidationResult.Fail("BIC must be 8 or 11 characters");
            }
            
            // Validate LEI if present
            if (hasLEI)
            {
                if (LEI.Length != 20)
                    return ValidationResult.Fail("LEI must be 20 characters");
            }
            
            return ValidationResult.Success();
        }
    }
}
