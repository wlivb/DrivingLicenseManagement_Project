using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DTOs
{
    public class LocalDrivingLicenseAppDTO
    {
            public int LocalDrivingLicenseAppID { get; set; }
            public int ApplicationID { get; set; }
            public int LicenseClassID { get; set; }
            public LocalDrivingLicenseAppDTO(int localDrivingLicenseAppID, int applicationID, int licenseClassID)
            {
                this.LocalDrivingLicenseAppID = localDrivingLicenseAppID;
                this.ApplicationID = applicationID;
                this.LicenseClassID = licenseClassID;
            }
            public LocalDrivingLicenseAppDTO() { }
        
    }
}

