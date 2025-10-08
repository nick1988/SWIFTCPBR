namespace YourNamespace.Dtos.Swift.Camt
{
    /// <summary>
    /// DTO for CAMT.057.001.06 - Notification to Receive
    /// Maps to the Camt057_001_06.scriban template
    /// </summary>
    public class Camt057Dto
    {
        /// <summary>
        /// Group Header - contains message identification
        /// Mandatory - uses GrpHdr_Camt partial (MsgId + CreDtTm only)
        /// </summary>
        public GroupHeaderCamtDto GroupHeader { get; set; }
        
        /// <summary>
        /// Notification Identification
        /// Mandatory - unique identifier for this notification
        /// Max length: 35 characters
        /// </summary>
        public string NotificationId { get; set; }
        
        /// <summary>
        /// Account that will receive the funds
        /// Optional - uses Acct partial (IBAN or Other)
        /// </summary>
        public AccountDto? Account { get; set; }
        
        /// <summary>
        /// Account Owner - owner of the receiving account
        /// Optional - always uses Agent (nostro account scenario)
        /// Typically same as BAH "From" (your institution)
        /// </summary>
        public AgentDto? AccountOwner { get; set; }
        
        /// <summary>
        /// Account Servicer - FI that services the receiving account
        /// Optional - uses FinInstnId partial
        /// Typically same as BAH "To" (correspondent bank)
        /// </summary>
        public AgentDto? AccountServicer { get; set; }
        
        /// <summary>
        /// Item - contains the notification details
        /// Mandatory
        /// </summary>
        public NotificationItemDto Item { get; set; }
        
        /// <summary>
        /// Validates the DTO for CBPR+ compliance and business rules
        /// </summary>
        public ValidationResult Validate()
        {
            // Check mandatory fields
            if (GroupHeader == null)
                return ValidationResult.Fail("GroupHeader is mandatory");
            
            if (string.IsNullOrWhiteSpace(NotificationId))
                return ValidationResult.Fail("NotificationId is mandatory");
            
            if (NotificationId.Length > 35)
                return ValidationResult.Fail("NotificationId exceeds 35 character limit");
            
            if (Item == null)
                return ValidationResult.Fail("Item is mandatory");
            
            // Validate nested DTOs
            var groupHeaderValidation = GroupHeader?.Validate();
            if (groupHeaderValidation != null && !groupHeaderValidation.IsSuccess)
                return groupHeaderValidation;
            
            var accountValidation = Account?.Validate();
            if (accountValidation != null && !accountValidation.IsSuccess)
                return accountValidation;
            
            var itemValidation = Item?.Validate();
            if (itemValidation != null && !itemValidation.IsSuccess)
                return itemValidation;
            
            return ValidationResult.Success();
        }
    }
    
    /// <summary>
    /// Notification Item DTO - contains details of the expected payment
    /// </summary>
    public class NotificationItemDto
    {
        /// <summary>
        /// Item Identification
        /// Mandatory - unique identifier for this item
        /// Max length: 35 characters
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// End to End Identification
        /// Optional - reference from original payment instruction
        /// Max length: 35 characters
        /// </summary>
        public string? EndToEndId { get; set; }
        
        /// <summary>
        /// Unique End-to-End Transaction Reference
        /// Optional - UUID format
        /// </summary>
        public string? UETR { get; set; }
        
        /// <summary>
        /// Amount to be received (currency + value)
        /// Mandatory
        /// </summary>
        public AmountDto Amount { get; set; }
        
        /// <summary>
        /// Expected Value Date - when funds are expected to be received
        /// Mandatory
        /// </summary>
        public DateTime ExpectedValueDate { get; set; }
        
        /// <summary>
        /// Expected Value Date formatted for SWIFT (YYYY-MM-DD)
        /// Used by Scriban template
        /// </summary>
        public string ExpectedValueDateFormatted => CbprFormatter.FormatDate(ExpectedValueDate);
        
        /// <summary>
        /// Debtor - the party/agent sending the funds
        /// Mandatory - must populate EITHER Party OR Agent, never both
        /// </summary>
        public DebtorDto Debtor { get; set; }
        
        /// <summary>
        /// Debtor Agent - the agent of the debtor
        /// Optional - uses FinInstnId partial
        /// </summary>
        public AgentDto? DebtorAgent { get; set; }
        
        /// <summary>
        /// Intermediary Agent - optional intermediary in payment chain
        /// Optional - uses FinInstnId partial
        /// </summary>
        public AgentDto? IntermediaryAgent { get; set; }
        
        /// <summary>
        /// Validates the Item for CBPR+ compliance and business rules
        /// </summary>
        public ValidationResult Validate()
        {
            // Check mandatory fields
            if (string.IsNullOrWhiteSpace(Id))
                return ValidationResult.Fail("Item.Id is mandatory");
            
            if (Id.Length > 35)
                return ValidationResult.Fail("Item.Id exceeds 35 character limit");
            
            if (!string.IsNullOrWhiteSpace(EndToEndId) && EndToEndId.Length > 35)
                return ValidationResult.Fail("Item.EndToEndId exceeds 35 character limit");
            
            if (Amount == null)
                return ValidationResult.Fail("Item.Amount is mandatory");
            
            if (ExpectedValueDate == default(DateTime))
                return ValidationResult.Fail("Item.ExpectedValueDate is mandatory");
            
            if (Debtor == null)
                return ValidationResult.Fail("Item.Debtor is mandatory");
            
            // Validate nested DTOs
            var amountValidation = Amount?.Validate();
            if (amountValidation != null && !amountValidation.IsSuccess)
                return amountValidation;
            
            var debtorValidation = Debtor?.Validate();
            if (debtorValidation != null && !debtorValidation.IsSuccess)
                return debtorValidation;
            
            return ValidationResult.Success();
        }
    }
    
    /// <summary>
    /// Debtor DTO - represents either a Party or an Agent (choice in CBPR+)
    /// Only ONE should be populated, never both
    /// </summary>
    public class DebtorDto
    {
        /// <summary>
        /// Party information (if debtor is a party/customer)
        /// Use this when no BIC is available
        /// </summary>
        public PartyDto? Party { get; set; }
        
        /// <summary>
        /// Agent information (if debtor is a financial institution)
        /// Use this when BIC is available
        /// </summary>
        public AgentDto? Agent { get; set; }
        
        /// <summary>
        /// Validates that only ONE of Party or Agent is populated (CBPR+ choice rule)
        /// </summary>
        public ValidationResult Validate()
        {
            bool hasParty = Party != null;
            bool hasAgent = Agent != null;
            
            // Must have exactly one (XOR logic)
            if (!hasParty && !hasAgent)
                return ValidationResult.Fail("Debtor must have either Party or Agent populated");
            
            if (hasParty && hasAgent)
                return ValidationResult.Fail("Debtor cannot have both Party and Agent - only one is allowed (CBPR+ choice rule)");
            
            // Validate whichever is populated
            if (hasParty)
            {
                var partyValidation = Party.Validate();
                if (!partyValidation.IsSuccess)
                    return partyValidation;
            }
            
            return ValidationResult.Success();
        }
    }
}
