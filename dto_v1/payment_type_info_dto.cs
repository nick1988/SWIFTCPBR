namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Payment Type Information DTO for SWIFT CBPR+ messages
    /// Contains instruction priority, service level, and category purpose
    /// Maps to PmtTpInf partial
    /// </summary>
    public class PaymentTypeInformationDto
    {
        /// <summary>
        /// Instruction Priority
        /// Typically "NORM" (Normal) for standard payments
        /// Valid CBPR+ values: HIGH, NORM
        /// Mandatory
        /// </summary>
        public string InstrPrty { get; set; } = "NORM";
        
        /// <summary>
        /// Service Level Code
        /// Operations have requested "G001"
        /// Mandatory
        /// </summary>
        public string SvcLvl { get; set; } = "G001";
        
        /// <summary>
        /// Category Purpose Code
        /// Valid values for your use case: "LOAN" or "CASH"
        /// LOAN = Loan payment
        /// CASH = Cash management transfer
        /// Uses ISO Category Purpose external code set (Bank of England requirement)
        /// Mandatory
        /// </summary>
        public string CtgyPurp { get; set; }
        
        /// <summary>
        /// Validates the Payment Type Information for CBPR+ compliance
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(InstrPrty))
                return ValidationResult.Fail("InstrPrty is mandatory");
            
            var validPriorities = new[] { "HIGH", "NORM" };
            if (!validPriorities.Contains(InstrPrty))
                return ValidationResult.Fail($"InstrPrty must be one of: {string.Join(", ", validPriorities)}");
            
            if (string.IsNullOrWhiteSpace(SvcLvl))
                return ValidationResult.Fail("SvcLvl is mandatory");
            
            if (string.IsNullOrWhiteSpace(CtgyPurp))
                return ValidationResult.Fail("CtgyPurp is mandatory");
            
            var validCategoryPurposes = new[] { "LOAN", "CASH" };
            if (!validCategoryPurposes.Contains(CtgyPurp))
                return ValidationResult.Fail($"CtgyPurp must be one of: {string.Join(", ", validCategoryPurposes)}");
            
            return ValidationResult.Success();
        }
    }
}
