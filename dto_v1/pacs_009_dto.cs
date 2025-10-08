namespace YourNamespace.Dtos.Swift.Pacs
{
    /// <summary>
    /// DTO for PACS.009.001.08 - Financial Institution Credit Transfer
    /// Maps to the Pacs009_001_08.scriban template
    /// </summary>
    public class Pacs009Dto
    {
        /// <summary>
        /// Group Header - contains message identification and settlement information
        /// Mandatory
        /// </summary>
        public GroupHeaderDto GroupHeader { get; set; }
        
        /// <summary>
        /// Payment Identification - InstrId, EndToEndId, UETR
        /// Mandatory
        /// </summary>
        public PaymentIdentificationDto PaymentId { get; set; }
        
        /// <summary>
        /// Payment Type Information - priority, service level, category purpose
        /// Optional - only populate if needed
        /// </summary>
        public PaymentTypeInformationDto PaymentTypeInfo { get; set; }
        
        /// <summary>
        /// Interbank Settlement Amount (currency + value)
        /// Mandatory
        /// </summary>
        public AmountDto SettlementAmount { get; set; }
        
        /// <summary>
        /// Interbank Settlement Date
        /// Mandatory
        /// </summary>
        public DateTime SettlementDate { get; set; }
        
        /// <summary>
        /// Settlement Date formatted for SWIFT (YYYY-MM-DD)
        /// Used by Scriban template
        /// </summary>
        public string SettlementDateFormatted => SettlementDate.ToString("yyyy-MM-dd");
        
        /// <summary>
        /// Instructing Agent (the FI sending this message - your institution)
        /// Mandatory - will use BIC only (same as BAH "From" field)
        /// </summary>
        public AgentDto InstructingAgent { get; set; }
        
        /// <summary>
        /// Instructed Agent (the FI receiving this message - correspondent bank)
        /// Mandatory - will use BIC
        /// </summary>
        public AgentDto InstructedAgent { get; set; }
        
        /// <summary>
        /// Intermediary Agent 1 (optional intermediary in the payment chain)
        /// Optional - if present, IntermediaryAgent1Account becomes mandatory
        /// </summary>
        public AgentDto IntermediaryAgent1 { get; set; }
        
        /// <summary>
        /// Intermediary Agent 1 Account
        /// Mandatory if IntermediaryAgent1 is present, otherwise must be null
        /// </summary>
        public AccountDto IntermediaryAgent1Account { get; set; }
        
        /// <summary>
        /// Debtor (the FI whose account will be debited)
        /// Mandatory - always your own BIC (same as InstructingAgent)
        /// Will use BIC only, no name/address
        /// </summary>
        public AgentDto Debtor { get; set; }
        
        /// <summary>
        /// Debtor Agent (the agent of the debtor FI)
        /// Mandatory - will use BIC
        /// </summary>
        public AgentDto DebtorAgent { get; set; }
        
        /// <summary>
        /// Creditor Agent (optional agent of the creditor)
        /// Optional - can use BIC or Name+Address combination
        /// If present, CreditorAgentAccount should also be populated
        /// </summary>
        public AgentDto CreditorAgent { get; set; }
        
        /// <summary>
        /// Creditor Agent Account
        /// Optional - but should be populated if CreditorAgent is present
        /// </summary>
        public AccountDto CreditorAgentAccount { get; set; }
        
        /// <summary>
        /// Creditor (the FI being credited)
        /// Mandatory - will use BIC
        /// </summary>
        public AgentDto Creditor { get; set; }
        
        /// <summary>
        /// Creditor Account (the account to be credited)
        /// Mandatory - logically required to complete the credit
        /// </summary>
        public AccountDto CreditorAccount { get; set; }
        
        /// <summary>
        /// Instruction for Creditor Agent (free text instructions)
        /// Optional - max 140 characters
        /// Maps to InstrForCdtrAgt/InstrInf
        /// </summary>
        public string InstrForCdtrAgt { get; set; }
        
        /// <summary>
        /// Instruction for Next Agent (free text, similar to MT72)
        /// Optional - max 140 characters
        /// Maps to InstrForNxtAgt/InstrInf
        /// </summary>
        public string InstrForNxtAgt { get; set; }
        
        /// <summary>
        /// Purpose code (e.g., "CASH" or "LOAN")
        /// Optional - Bank of England requirement
        /// Uses ISO Purpose Code external code set
        /// </summary>
        public string Purpose { get; set; }
        
        /// <summary>
        /// Remittance Information (unstructured free text)
        /// Optional
        /// Maps to RmtInf/Ustrd
        /// </summary>
        public string RemittanceInfo { get; set; }
        
        /// <summary>
        /// Validates the DTO for CBPR+ compliance and business rules
        /// </summary>
        public ValidationResult Validate()
        {
            // Check mandatory fields
            if (GroupHeader == null)
                return ValidationResult.Fail("GroupHeader is mandatory");
            
            if (PaymentId == null)
                return ValidationResult.Fail("PaymentId is mandatory");
            
            if (SettlementAmount == null)
                return ValidationResult.Fail("SettlementAmount is mandatory");
            
            if (SettlementDate == default(DateTime))
                return ValidationResult.Fail("SettlementDate is mandatory");
            
            if (InstructingAgent == null)
                return ValidationResult.Fail("InstructingAgent is mandatory");
            
            if (InstructedAgent == null)
                return ValidationResult.Fail("InstructedAgent is mandatory");
            
            if (Debtor == null)
                return ValidationResult.Fail("Debtor is mandatory");
            
            if (DebtorAgent == null)
                return ValidationResult.Fail("DebtorAgent is mandatory");
            
            if (Creditor == null)
                return ValidationResult.Fail("Creditor is mandatory");
            
            if (CreditorAccount == null)
                return ValidationResult.Fail("CreditorAccount is mandatory");
            
            // Business rule: IntermediaryAgent1 requires IntermediaryAgent1Account
            if (IntermediaryAgent1 != null && IntermediaryAgent1Account == null)
                return ValidationResult.Fail("IntrmyAgt1Acct is mandatory when IntrmyAgt1 is present");
            
            // Business rule: If IntermediaryAgent1Account exists, IntermediaryAgent1 must exist
            if (IntermediaryAgent1Account != null && IntermediaryAgent1 == null)
                return ValidationResult.Fail("IntrmyAgt1 must be present if IntrmyAgt1Acct is provided");
            
            // String length validations
            if (!string.IsNullOrEmpty(InstrForCdtrAgt) && InstrForCdtrAgt.Length > 140)
                return ValidationResult.Fail("InstrForCdtrAgt exceeds 140 character limit");
            
            if (!string.IsNullOrEmpty(InstrForNxtAgt) && InstrForNxtAgt.Length > 140)
                return ValidationResult.Fail("InstrForNxtAgt exceeds 140 character limit");
            
            // Validate nested DTOs
            var groupHeaderValidation = GroupHeader?.Validate();
            if (groupHeaderValidation != null && !groupHeaderValidation.IsSuccess)
                return groupHeaderValidation;
            
            var paymentIdValidation = PaymentId?.Validate();
            if (paymentIdValidation != null && !paymentIdValidation.IsSuccess)
                return paymentIdValidation;
            
            var settlementAmountValidation = SettlementAmount?.Validate();
            if (settlementAmountValidation != null && !settlementAmountValidation.IsSuccess)
                return settlementAmountValidation;
            
            var creditorAccountValidation = CreditorAccount?.Validate();
            if (creditorAccountValidation != null && !creditorAccountValidation.IsSuccess)
                return creditorAccountValidation;
            
            return ValidationResult.Success();
        }
    }
    
    /// <summary>
    /// Amount DTO for currency and value
    /// Used for settlement amounts, instructed amounts, etc.
    /// </summary>
    public class AmountDto
    {
        /// <summary>
        /// Currency code (ISO 4217 3-letter code)
        /// Example: "GBP", "USD", "EUR"
        /// </summary>
        public string Ccy { get; set; }
        
        /// <summary>
        /// Amount value with up to 2 decimal places
        /// Example: 1000.50m
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Amount value formatted for SWIFT (decimal with 2 places, no thousands separator)
        /// Used by Scriban template
        /// Example: "1000.50"
        /// </summary>
        public string ValueFormatted => Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        
        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Ccy))
                return ValidationResult.Fail("Currency is mandatory");
            
            if (Ccy.Length != 3)
                return ValidationResult.Fail("Currency must be a 3-letter ISO code");
            
            if (Value <= 0)
                return ValidationResult.Fail("Amount value must be greater than zero");
            
            return ValidationResult.Success();
        }
    }
}
