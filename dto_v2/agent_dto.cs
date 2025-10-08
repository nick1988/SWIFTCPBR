namespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Agent DTO for financial institution agents
    /// Used for InstructingAgent, InstructedAgent, DebtorAgent, CreditorAgent, etc.
    /// This is a wrapper that contains FinInstnId
    /// </summary>
    public class AgentDto
    {
        /// <summary>
        /// Financial Institution Identification
        /// Contains BIC, LEI, ClrSysMmbId, Name, and Address
        /// Mandatory
        /// </summary>
        public FinInstnIdDto FinInstnId { get; set; }
        
        /// <summary>
        /// Validates the Agent
        /// </summary>
        public ValidationResult Validate()
        {
            if (FinInstnId == null)
                return ValidationResult.Fail("FinInstnId is mandatory for Agent");
            
            var finInstnValidation = FinInstnId.Validate();
            if (!finInstnValidation.IsSuccess)
                return finInstnValidation;
            
            return ValidationResult.Success();
        }
    }
}
