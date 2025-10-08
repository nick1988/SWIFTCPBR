namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Group Header DTO for SWIFT CBPR+ PACS messages (pacs.008, pacs.009)
    /// Contains message-level information and settlement details
    /// Maps to GrpHdr_Pacs partial
    /// </summary>
    public class GroupHeaderPacsDto
    {
        /// <summary>
        /// Message Identification - unique identifier for this message
        /// Mandatory - typically a ULID or UUID
        /// Max length: 35 characters
        /// </summary>
        public string MsgId { get; set; }
        
        /// <summary>
        /// Creation Date Time
        /// Mandatory
        /// </summary>
        public DateTime CreDtTm { get; set; }
        
        /// <summary>
        /// Creation Date Time formatted for SWIFT (ISO 8601 UTC)
        /// Used by Scriban template
        /// Example: "2025-10-08T14:30:00Z"
        /// </summary>
        public string CreDtTmFormatted => CbprFormatter.FormatDateTimeUtc(CreDtTm);
        
        /// <summary>
        /// Number of Transactions
        /// CBPR+ mandates this must always be "1" for pacs.008 and pacs.009
        /// Mandatory
        /// </summary>
        public string NbOfTxs { get; set; } = "1";
        
        /// <summary>
        /// Settlement Information (method and account)
        /// Mandatory
        /// </summary>
        public SettlementInfoDto SttlmInf { get; set; }
        
        /// <summary>
        /// Validates the Group Header for CBPR+ compliance
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(MsgId))
                return ValidationResult.Fail("MsgId is mandatory");
            
            if (MsgId.Length > 35)
                return ValidationResult.Fail("MsgId exceeds 35 character limit");
            
            if (CreDtTm == default(DateTime))
                return ValidationResult.Fail("CreDtTm is mandatory");
            
            if (string.IsNullOrWhiteSpace(NbOfTxs))
                return ValidationResult.Fail("NbOfTxs is mandatory");
            
            if (NbOfTxs != "1")
                return ValidationResult.Fail("NbOfTxs must be '1' for CBPR+ pacs messages");
            
            if (SttlmInf == null)
                return ValidationResult.Fail("SttlmInf is mandatory");
            
            var settlementValidation = SttlmInf?.Validate();
            if (settlementValidation != null && !settlementValidation.IsSuccess)
                return settlementValidation;
            
            return ValidationResult.Success();
        }
    }
    
    /// <summary>
    /// Settlement Information for CBPR+ messages
    /// Specifies how the transaction will be settled
    /// </summary>
    public class SettlementInfoDto
    {
        /// <summary>
        /// Settlement Method code
        /// Valid CBPR+ values:
        /// - INDA: Instructed Agent (settlement via debtor agent)
        /// - INGA: Instructing Agent (settlement via creditor agent)
        /// - COVE: Cover method (separate cover payment)
        /// - CLRG: Clearing system (settlement via clearing)
        /// Mandatory
        /// </summary>
        public string SttlmMtd { get; set; }
        
        /// <summary>
        /// Settlement Account - uses the generic Account DTO
        /// Optional - only required for certain settlement methods
        /// </summary>
        public AccountDto SttlmAcct { get; set; }
        
        /// <summary>
        /// Validates the Settlement Information
        /// </summary>
        public ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(SttlmMtd))
                return ValidationResult.Fail("SttlmMtd is mandatory");
            
            var validMethods = new[] { "INDA", "INGA", "COVE", "CLRG" };
            if (!validMethods.Contains(SttlmMtd))
                return ValidationResult.Fail($"SttlmMtd must be one of: {string.Join(", ", validMethods)}");
            
            var accountValidation = SttlmAcct?.Validate();
            if (accountValidation != null && !accountValidation.IsSuccess)
                return accountValidation;
            
            return ValidationResult.Success();
        }
    }
}
