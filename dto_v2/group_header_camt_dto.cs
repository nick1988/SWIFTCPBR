namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Group Header DTO for SWIFT CBPR+ CAMT messages (camt.057)
    /// Simpler than PACS - only contains message identification
    /// Maps to GrpHdr_Camt partial
    /// </summary>
    public class GroupHeaderCamtDto
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
            
            return ValidationResult.Success();
        }
    }
}
