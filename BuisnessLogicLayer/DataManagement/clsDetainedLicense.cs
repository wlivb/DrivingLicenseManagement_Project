using System;
using System.Data;
using DataAccessLayer.DataSources;
using DVLD_DTOs;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsDetainedLicense : clsBaseLogic<DetainedLicenseDTO>
    {
        public clsDetainedLicense() : base() { }
        private clsDetainedLicense(DetainedLicenseDTO dto) : base(dto) { }
        public clsUser CreatedByUser
        {
            get
            {
                return clsUser.Find(this.DTO.CreatedByUserID);
            }
        }
        public clsUser ReleasedByUser
        {
            get
            {
                if (this.DTO.ReleasedByUserID != -1)
                {
                    return clsUser.Find(this.DTO.ReleasedByUserID);
                }
                return null;
            }
        }
        public static clsDetainedLicense FindByID(int detainedLicenseID)
        {
            DetainedLicenseDTO dto = clsDetainedLicenseData.GetDetainedLicenseInfoByID(detainedLicenseID);

            if (dto != null)
            {
                return new clsDetainedLicense(dto);
            }
            return null;
        }
        public static clsDetainedLicense FindByLicenseID(int licenseID)
        {
            DetainedLicenseDTO dto = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(licenseID);

            if (dto != null)
            {
                return new clsDetainedLicense(dto);
            }
            return null;
        }
        protected override bool _AddNew()
        {
            this.DTO.DetainID = clsDetainedLicenseData.AddNew(this.DTO);
            return (this.DTO.DetainID != -1);
        }
        protected override bool _Update()
        {
            return clsDetainedLicenseData.Update(this.DTO);
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }
        public static bool IsLicenseDetained(int licenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(licenseID);
        }
        public static bool ReleaseLicense(int detainedLicenseID, int releasedByUserID, int releaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(detainedLicenseID, releasedByUserID, releaseApplicationID);
        }
        public bool ReleaseLicense(int releasedByUserID, int releaseApplicationID)
        {
            bool isReleased = clsDetainedLicenseData.ReleaseDetainedLicense(this.DTO.DetainID, releasedByUserID, releaseApplicationID);

            if (isReleased)
            {
                // تحديث الكائن في الذاكرة ليتطابق مع الداتابيز
                this.DTO.IsReleased = true;
                this.DTO.ReleaseDate = DateTime.Now;
                this.DTO.ReleasedByUserID = releasedByUserID;
                this.DTO.ReleaseApplicationID = releaseApplicationID;
            }
            return isReleased;
        }
    }
}
