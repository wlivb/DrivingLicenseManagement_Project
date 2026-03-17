using DVLD_DTOs;
using DataAccessLayer.DataSources;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsInternationalLicense : clsApplication
    {
        public InternationalLicenseDTO InterLicenseDTO { get; set; }
        public int InternationalLicenseID
        {
            get => InterLicenseDTO.InternationalLicenseID;
            set => InterLicenseDTO.InternationalLicenseID = value;
        }
        public clsInternationalLicense() : base()
        {
            this.InterLicenseDTO = new InternationalLicenseDTO();
            this.InternationalLicenseID = -1;

            this.DTO.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
            this.Mode = enMode.AddNew;
        }
        private clsInternationalLicense(ApplicationDTO appDTO, InternationalLicenseDTO interDTO) : base(appDTO)
        {
            this.InterLicenseDTO = interDTO;
            this.Mode = enMode.Update;
        }
        public static new clsInternationalLicense Find(int internationalLicenseID)
        {
            var fullInfo = clsInternationalLicenseData.GetInternationalLicenseFullInfoByID(internationalLicenseID);

            if (fullInfo != null)
            {
                return new clsInternationalLicense(fullInfo.Application, fullInfo.InternationalLicense);
            }

            return null;
        }
        public new bool Save()
        {
            enMode currentMode = this.Mode;

            if (!base.Save()) return false;

            this.InterLicenseDTO.ApplicationID = this.DTO.ApplicationID;

            switch (currentMode)
            {
                case enMode.AddNew:
                    this.InternationalLicenseID = clsInternationalLicenseData.AddNew(this.InterLicenseDTO);
                    return (this.InternationalLicenseID != -1);

                case enMode.Update:
                    return clsInternationalLicenseData.Update(this.InterLicenseDTO);
            }

            return false;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }
        public static DataTable GetDriverInternationalLicenses(int driverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(driverID);
        }
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
    }
}
