namespace YourNamespace.Dtos.Swift.Pacs
{
    /// <summary>
    /// DTO for PACS.008.001.08 - FI to FI Customer Credit Transfer
    /// Maps to the Pacs008_001_08.scriban template
    /// </summary>
    public class Pacs008Dto
    {
        /// <summary>
        /// Group Header - contains message identification and settlement information
        /// Mandatory
        /// </summary>
        public GroupHeaderPacsDto GroupHeader { get; set; }
        
        /// <summary>
        /// Payment Identification - InstrId, EndToEndId, UETR
        /// Mandatory
        /// </summary>
        public PaymentIdentificationDto PaymentId { get; set; }
        
        /// <summary>
        /// Payment Type Information - priority, service level, category purpose
        /// Mandatory - uses PmtTpInf partial
        /// </summary>
        public PaymentTypeInformationDto PaymentTypeInfo { get; set; }
        
        /// <summary>
        /// Interbank Settlement Amount (currency + value)
        /// Mandatory - must match InstructedAmount
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
        public string SettlementDateFormatted => CbprFormatter.FormatDate(SettlementDate);
        
        /// <summary>
        /// Instructed Amount (amount debtor instructed to transfer)
        /// Mandatory - must match SettlementAmount
        /// </summary>
        public AmountDto InstructedAmount { get; set; }
        
        /// <summary>
        /// Charge Bearer code (who pays charges)
        /// Mandatory - typically "DEBT" (debtor pays all charges)
        /// Valid values: DEBT, CRED, SHAR, SLEV
        /// </summary>
        public string ChargeBearer { get; set; } = "DEBT";
        
        /// <summary>
        /// Instructing Agent (the FI sending this message - your institution)
        /// Mandatory - uses FinInstnId partial
        /// </summary>
        public AgentDto InstructingAgent { get; set; }
        
        /// <summary>
        /// Instructed Agent (the FI receiving this message - correspondent bank)
        /// Mandatory - uses FinInstnId partial
        /// </summary>
        public AgentDto InstructedAgent { get; set; }
        
        /// <summary>
        /// Intermediary Agent 1 (optional intermediary in the payment chain)
        /// Optional - if present, IntermediaryAgent1Account becomes mandatory
        /// </summary>
        public AgentDto? IntermediaryAgent1 { get; set; }
        
        /// <summary>
        /// Intermediary Agent 1 Account
        /// Mandatory if IntermediaryAgent1 is present, otherwise must be null
        /// </summary>
        public AccountDto? IntermediaryAgent1Account { get; set; }
        
        /// <summary>
        /// Debtor (the customer/party originating the payment)
        /// Mandatory - uses Party partial (Name, Address, optional ID)
        /// </summary>
        public PartyDto Debtor { get; set; }
        
        /// <summary>
        /// Debtor Account (account to be debited)
        /// Optional - uses Account partial
        /// </summary>
        public AccountDto? DebtorAccount { get; set; }
        
        /// <summary>
        /// Debtor Agent (the agent of the debtor - typically the instructing FI)
        /// Mandatory - uses FinInstnId partial
        /// </summary>
        public AgentDto DebtorAgent { get; set; }
        
        /// <summary>
        /// Creditor Agent (optional agent of the creditor)
        /// Optional - if present, CreditorAgentAccount becomes mandatory
        /// </summary>
        public AgentDto? CreditorAgent { get; set; }
        
        /// <summary>
        /// Creditor Agent Account
        /// Mandatory if CreditorAgent is present, otherwise must be null
        /// </summary>
        public AccountDto? CreditorAgentAccount { get; set; }
        
        /// <summary>
        /// Creditor (the beneficiary/party receiving the payment)
        /// Mandatory - uses Party partial (Name, Address, optional ID)
        /// </summary>
        public PartyDto Creditor { get; set; }
        
        /// <summary>
        /// Creditor Account (account to be credited)
        /// Mandatory - logically required to complete the credit transfer
        /// </summary>
        public AccountDto CreditorAccount { get; set; }
        
        /// <summary>
        /// Instruction for Creditor Agent (free text instructions)
        /// Optional - max 140 characters
        /// Maps to InstrForCdtrAgt/InstrInf
        /// </summary>
        public string? InstrForCdtrAgt { get; set; }
        
        /// <summary>
        /// Instruction for Next Agent (free text, similar to MT72)
        /// Optional - max 35 characters (NOTE: shorter than InstrForCdtrAgt!)
        /// Maps to InstrForNxtAgt/InstrInf
        /// </summary>
        public string? InstrForNxtAgt { get; set; }
        
        /// <summary>
        /// Purpose code (e.g., "CASH" or "LOAN")
        /// Optional - Bank of England requirement
        /// Uses ISO Purpose Code external code set
        /// </summary>
        public string? Purpose { get; set; }
        
        /// <summary>
        /// Remittance Information (unstructured free text)
        /// Optional
        /// Maps to RmtInf/Ustrd
        /// </summary>
        public string? RemittanceInfo { get; set; }
        
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
            
            if (PaymentTypeInfo == null)
                return ValidationResult.Fail("PaymentTypeInfo is mandatory");
            
            if (SettlementAmount == null)
                return ValidationResult.Fail("SettlementAmount is mandatory");
            
            if (SettlementDate == default(DateTime))
                return ValidationResult.Fail("SettlementDate is mandatory");
            
            if (InstructedAmount == null)
                return ValidationResult.Fail("InstructedAmount is mandatory");
            
            if (string.IsNullOrWhiteSpace(ChargeBearer))
                return ValidationResult.Fail("ChargeBearer is mandatory");
            
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
            
            // Business rule: SettlementAmount and InstructedAmount must match
            if (SettlementAmount.Ccy != InstructedAmount.Ccy)
                return ValidationResult.Fail("SettlementAmount and InstructedAmount must have the same currency");
            
            if (SettlementAmount.Value != InstructedAmount.Value)
                return ValidationResult.Fail("SettlementAmount and InstructedAmount must have the same value");
            
            // Business rule: IntermediaryAgent1 requires IntermediaryAgent1Account
            if (IntermediaryAgent1 != null && IntermediaryAgent1Account == null)
                return ValidationResult.Fail("IntrmyAgt1Acct is mandatory when IntrmyAgt1 is present");
            
            if (IntermediaryAgent1Account != null && IntermediaryAgent1 == null)
                return ValidationResult.Fail("IntrmyAgt1 must be present if IntrmyAgt1Acct is provided");
            
            // Business rule: CreditorAgent requires CreditorAgentAccount
            if (CreditorAgent != null && CreditorAgentAccount == null)
                return ValidationResult.Fail("CdtrAgtAcct is mandatory when CdtrAgt is present");
            
            if (CreditorAgentAccount != null && CreditorAgent == null)
                return ValidationResult.Fail("CdtrAgt must be present if CdtrAgtAcct is provided");
            
            // String length validations
            if (!string.IsNullOrEmpty(InstrForCdtrAgt) && InstrForCdtrAgt.Length > 140)
                return ValidationResult.Fail("InstrForCdtrAgt exceeds 140 character limit");
            
            if (!string.IsNullOrEmpty(InstrForNxtAgt) && InstrForNxtAgt.Length > 35)
                return ValidationResult.Fail("InstrForNxtAgt exceeds 35 character limit");
            
            // Validate nested DTOs
            var groupHeaderValidation = GroupHeader?.Validate();
            if (groupHeaderValidation != null && !groupHeaderValidation.IsSuccess)
                return groupHeaderValidation;
            
            var paymentIdValidation = PaymentId?.Validate();
            if (paymentIdValidation != null && !paymentIdValidation.IsSuccess)
                return paymentIdValidation;
            
            var paymentTypeValidation = PaymentTypeInfo?.Validate();
            if (paymentTypeValidation != null && !paymentTypeValidation.IsSuccess)
                return paymentTypeValidation;
            
            var settlementAmountValidation = SettlementAmount?.Validate();
            if (settlementAmountValidation != null && !settlementAmountValidation.IsSuccess)
                return settlementAmountValidation;
            
            var instructedAmountValidation = InstructedAmount?.Validate();
            if (instructedAmountValidation != null && !instructedAmountValidation.IsSuccess)
                return instructedAmountValidation;
            
            var debtorValidation = Debtor?.Validate();
            if (debtorValidation != null && !debtorValidation.IsSuccess)
                return debtorValidation;
            
            var creditorValidation = Creditor?.Validate();
            if (creditorValidation != null && !creditorValidation.IsSuccess)
                return creditorValidation;
            
            var creditorAccountValidation = CreditorAccount?.Validate();
            if (creditorAccountValidation != null && !creditorAccountValidation.IsSuccess)
                return creditorAccountValidation;
            
            return ValidationResult.Success();
        }
    }
}
