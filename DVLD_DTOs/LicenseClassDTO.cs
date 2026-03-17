using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DTOs
{
    public class LicenseClassDTO
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        public LicenseClassDTO(int licenseClassID, string classNam, string descrp, byte MinAge, byte ValidLength, decimal Fees)
        {
            this.LicenseClassID = licenseClassID;
            this.ClassName = classNam;
            this.ClassDescription = descrp;
            this.MinimumAllowedAge = MinAge;
            this.DefaultValidityLength = ValidLength;
            this.ClassFees = Fees;
        }
        public LicenseClassDTO() { }
    }
}
