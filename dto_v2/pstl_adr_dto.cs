    /// <summary>
    /// Address Type DTO
    /// Can be either Code or Proprietary
    /// </summary>
    public class AddressTypeDto
    {
        /// <summary>
        /// Address Type Code
        /// Standard codes like ADDR, PBOX, HOME, BIZZ, MLTO, DLVY
        /// </summary>
        public string? Code { get; set; }
        
        /// <summary>
        /// Proprietary Addressnamespace YourNamespace.Dtos.Swift
{
    /// <summary>
    /// Postal Address DTO for SWIFT CBPR+ messages
    /// Maps to PstlAdr partial
    /// Uses structured address only (no AdrLine) as per your requirements
    /// </summary>
    public class PstlAdrDto
    {
        /// <summary>
        /// Address Type
        /// Optional - Code or Proprietary
        /// </summary>
        public AddressTypeDto? AdrTp { get; set; }
        
        /// <summary>
        /// Department
        /// Optional - Max 70 characters
        /// </summary>
        public string? Dept { get; set; }
        
        /// <summary>
        /// Sub-Department
        /// Optional - Max 70 characters
        /// </summary>
        public string? SubDept { get; set; }
        
        /// <summary>
        /// Street Name
        /// Optional - Max 70 characters
        /// </summary>
        public string? StrtNm { get; set; }
        
        /// <summary>
        /// Building Number
        /// Optional - Max 16 characters
        /// </summary>
        public string? BldgNb { get; set; }
        
        /// <summary>
        /// Building Name
        /// Optional - Max 35 characters
        /// </summary>
        public string? BldgNm { get; set; }
        
        /// <summary>
        /// Floor
        /// Optional - Max 70 characters
        /// </summary>
        public string? Flr { get; set; }
        
        /// <summary>
        /// Post Box
        /// Optional - Max 16 characters
        /// </summary>
        public string? PstBx { get; set; }
        
        /// <summary>
        /// Room
        /// Optional - Max 70 characters
        /// </summary>
        public string? Room { get; set; }
        
        /// <summary>
        /// Post Code
        /// Optional - Max 16 characters
        /// </summary>
        public string? PstCd { get; set; }
        
        /// <summary>
        /// Town Name
        /// Optional - Max 35 characters
        /// </summary>
        public string? TwnNm { get; set; }
        
        /// <summary>
        /// Town Location Name
        /// Optional - Max 35 characters
        /// </summary>
        public string? TwnLctnNm { get; set; }
        
        /// <summary>
        /// District Name
        /// Optional - Max 35 characters
        /// </summary>
        public string? DstrctNm { get; set; }
        
        /// <summary>
        /// Country Sub Division (e.g., state/province)
        /// Optional - Max 35 characters
        /// </summary>
        public string? CtrySubDvsn { get; set; }
        
        /// <summary>
        /// Country (ISO 3166-1 alpha-2 code)
        /// Optional - 2 characters
        /// Example: "GB", "US", "FR"
        /// </summary>
        public string? Ctry { get; set; }
        
        /// <summary>
        /// Validates the Postal Address
        /// </summary>
        public ValidationResult Validate()
        {
            // Validate string lengths
            if (!string.IsNullOrEmpty(Dept) && Dept.Length > 70)
                return ValidationResult.Fail("Dept exceeds 70 character limit");
            
            if (!string.IsNullOrEmpty(SubDept) && SubDept.Length > 70)
                return ValidationResult.Fail("SubDept exceeds 70 character limit");
            
            if (!string.IsNullOrEmpty(StrtNm) && StrtNm.Length > 70)
                return ValidationResult.Fail("StrtNm exceeds 70 character limit");
            
            if (!string.IsNullOrEmpty(BldgNb) && BldgNb.Length > 16)
                return ValidationResult.Fail("BldgNb exceeds 16 character limit");
            
            if (!string.IsNullOrEmpty(BldgNm) && BldgNm.Length > 35)
                return ValidationResult.Fail("BldgNm exceeds 35 character limit");
            
            if (!string.IsNullOrEmpty(Flr) && Flr.Length > 70)
                return ValidationResult.Fail("Flr exceeds 70 character limit");
            
            if (!string.IsNullOrEmpty(PstBx) && PstBx.Length > 16)
                return ValidationResult.Fail("PstBx exceeds 16 character limit");
            
            if (!string.IsNullOrEmpty(Room) && Room.Length > 70)
                return ValidationResult.Fail("Room exceeds 70 character limit");
            
            if (!string.IsNullOrEmpty(PstCd) && PstCd.Length > 16)
                return ValidationResult.Fail("PstCd exceeds 16 character limit");
            
            if (!string.IsNullOrEmpty(TwnNm) && TwnNm.Length > 35)
                return ValidationResult.Fail("TwnNm exceeds 35 character limit");
            
            if (!string.IsNullOrEmpty(TwnLctnNm) && TwnLctnNm.Length > 35)
                return ValidationResult.Fail("TwnLctnNm exceeds 35 character limit");
            
            if (!string.IsNullOrEmpty(DstrctNm) && DstrctNm.Length > 35)
                return ValidationResult.Fail("DstrctNm exceeds 35 character limit");
            
            if (!string.IsNullOrEmpty(CtrySubDvsn) && CtrySubDvsn.Length > 35)
                return ValidationResult.Fail("CtrySubDvsn exceeds 35 character limit");
            
            if (!string.IsNullOrEmpty(Ctry) && Ctry.Length != 2)
                return ValidationResult.Fail("Ctry must be a 2-character ISO code");
            
            return ValidationResult.Success();
        }
    }
    
    /// <summary>
    /// Address Type DTO
    /// Can be either Code or Proprietary
    /// </summary>
    public class AddressTypeDto
    {
        /// <summary>
        /// Address Type Code
        /// Standard codes like ADDR, PBOX, HOME, BIZZ, MLTO, DLVY
        /// </summary>
        public string? Code { get; set; }
        
        /// <summary>
        /// Proprietary Address Type
        /// Custom address type identification
        /// </summary>
        public ProprietaryDto? Proprietary { get; set; }
    }
    
    /// <summary>
    /// Proprietary identification DTO
    /// </summary>
    public class ProprietaryDto
    {
        public string? Id { get; set; }
        public string? Issuer { get; set; }
        public string? SchemeName { get; set; }
    }
}