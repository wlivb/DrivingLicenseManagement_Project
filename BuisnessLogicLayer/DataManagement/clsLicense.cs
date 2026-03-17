using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System;
using System.Data;
using static BuisnessLogicLayer.DataManagement.clsApplication;


namespace BuisnessLogicLayer.DataManagement
{
    public class clsLicense : clsBaseLogic<LicenseDTO>
    {
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public clsLicense() : base() { }
        private clsLicense(LicenseDTO dto) : base(dto) { }

        private clsDriver _DriverInfo;
        public clsDriver DriverInfo
        {
            get
            {
                if (_DriverInfo == null)
                    _DriverInfo = clsDriver.FindByDriverID(this.DTO.DriverID);
                return _DriverInfo;
            }
        }
        public bool IsDetained
        {
            get
            {
                return clsDetainedLicense.IsLicenseDetained(this.DTO.LicenseID);
            }
        }

        private clsLicenseClass _LicenseClassInfo;
        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                if (_LicenseClassInfo == null)
                    this._LicenseClassInfo = clsLicenseClass.Find(this.DTO.LicenseClassID);
                return _LicenseClassInfo;
            }
        }
        public clsDetainedLicense DetainedLicense
        {
            get
            {
                return clsDetainedLicense.FindByLicenseID(this.DTO.LicenseID);
            }
        }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public static string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }
        public static clsLicense Find(int licenseID)
        {
            LicenseDTO dto = clsLicenseData.GetLicenseInfoByID(licenseID);
            if (dto != null)
                return new clsLicense(dto);
            else
                return null;
        }
        protected override bool _AddNew()
        {
            this.DTO.LicenseID = clsLicenseData.AddNewLicense(this.DTO);

            return (this.DTO.LicenseID != -1);
        }
        protected override bool _Update()
        {
            return clsLicenseData.UpdateLicense(this.DTO);
        }
        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        public static DataTable GetDriverLicenses(int driverID)
        {
            return clsLicenseData.GetDriverLicenses(driverID);
        }
        public static bool IsLicenseExistByPersonID(int personID, int licenseClassID)
        {
            return (clsLicenseData.GetActiveLicenseIDByPersonID(personID, licenseClassID) > 0);
        }
        public static int GetActiveLicenseIDByPersonID(int personID, int licenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(personID, licenseClassID);
        }
        public Boolean IsLicenseExpired()
        {
            return (this.DTO.ExpirationDate < DateTime.Now);
        }
        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(this.DTO.LicenseID);
        }
        public static bool IsDetaind(int licence)
        {
            return clsDetainedLicense.IsLicenseDetained(licence);
        }
        public int Detain(decimal FineFees, int CreatedByUserID)
        {
            if (this.IsDetained)
                return -1;

            clsDetainedLicense detainedLicense = new clsDetainedLicense();

            detainedLicense.DTO.LicenseID = this.DTO.LicenseID;
            detainedLicense.DTO.DetainDate = DateTime.Now;
            detainedLicense.DTO.FineFees = FineFees;
            detainedLicense.DTO.CreatedByUserID = CreatedByUserID;
            detainedLicense.DTO.IsReleased = false; 

            if (detainedLicense.Save())
            {
                return detainedLicense.DTO.DetainID;
            }
            else
            {
                return -1;
            }
        }
        public bool ReleaseDetain(int ReleaseByUserID, ref int ApplicationID)
        {
            clsApplication newApp = new clsApplication();

            newApp.DTO.ApplicantPersonID = this.DriverInfo.DTO.PersonID;
            newApp.DTO.ApplicationDate = DateTime.Now;
            newApp.DTO.ApplicationTypeID = (int)enApplicationType.ReleaseDetainedDrivingLicense;
            newApp.DTO.ApplicationStatus = (byte)enApplicationStatus.Completed;
            newApp.DTO.LastStatusDate = DateTime.Now;
            newApp.DTO.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense).DTO.ApplicationTypeFees;
            newApp.DTO.CreatedByUserID = ReleaseByUserID;

            if (!newApp.Save())
                return false;

            ApplicationID = newApp.DTO.ApplicationID;

            return clsDetainedLicense.ReleaseLicense(this.DetainedLicense.DTO.DetainID, ReleaseByUserID, ApplicationID);
        }
        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            if (!this.DeactivateLicense())
                return null;

            clsApplication newApp = new clsApplication();

            newApp.DTO.ApplicationID = -1;
            newApp.DTO.ApplicantPersonID = this.DriverInfo.DTO.PersonID;
            newApp.DTO.ApplicationDate = DateTime.Now;
            newApp.DTO.ApplicationTypeID = (int)enApplicationType.RenewDrivingLicense;
            newApp.DTO.ApplicationStatus = (byte)enApplicationStatus.Completed;
            newApp.DTO.LastStatusDate = DateTime.Now;
            newApp.DTO.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).DTO.ApplicationTypeFees;
            newApp.DTO.CreatedByUserID = CreatedByUserID;

            if (!newApp.Save())
                return null;

            clsLicense newLicense = new clsLicense();
            newLicense.DTO.ApplicationID = newApp.DTO.ApplicationID;
            newLicense.DTO.DriverID = this.DTO.DriverID;
            newLicense.DTO.LicenseClassID = this.DTO.LicenseClassID;
            newLicense.DTO.IssueDate = DateTime.Now;
            newLicense.DTO.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DTO.DefaultValidityLength);
            newLicense.DTO.Notes = Notes;
            newLicense.DTO.PaidFees = this.LicenseClassInfo.DTO.ClassFees;
            newLicense.DTO.IsActive = true;
            newLicense.DTO.IssueReason = (byte)enIssueReason.Renew;
            newLicense.DTO.CreatedByUserID = CreatedByUserID;

            if (newLicense.Save())
                return newLicense;
            else
                return null;
        }
        public clsLicense Replace(enIssueReason issueReason, int CreatedByUserID)
        {
            if (!this.DeactivateLicense())
                return null;

            clsApplication newApp = new clsApplication();

            newApp.DTO.ApplicationID = -1;
            newApp.DTO.ApplicantPersonID = this.DriverInfo.DTO.PersonID;
            newApp.DTO.ApplicationDate = DateTime.Now;

            clsApplication.enApplicationType appType = (issueReason == enIssueReason.DamagedReplacement) ?
            clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
            clsApplication.enApplicationType.ReplaceLostDrivingLicense;

            newApp.DTO.ApplicationTypeID = (int)appType;
            newApp.DTO.ApplicationStatus = (byte)enApplicationStatus.Completed;
            newApp.DTO.LastStatusDate = DateTime.Now;
            newApp.DTO.PaidFees = clsApplicationType.Find((int)appType).DTO.ApplicationTypeFees;
            newApp.DTO.CreatedByUserID = CreatedByUserID;

            if (!newApp.Save())
                return null;

            clsLicense newLicense = new clsLicense();
            newLicense.DTO.ApplicationID = newApp.DTO.ApplicationID;
            newLicense.DTO.DriverID = this.DTO.DriverID;
            newLicense.DTO.LicenseClassID = this.DTO.LicenseClassID;
            newLicense.DTO.IssueDate = DateTime.Now;
            newLicense.DTO.ExpirationDate = this.DTO.ExpirationDate;
            newLicense.DTO.Notes = this.DTO.Notes;
            newLicense.DTO.PaidFees = 0;
            newLicense.DTO.IsActive = true;
            newLicense.DTO.IssueReason = (byte)issueReason;
            newLicense.DTO.CreatedByUserID = CreatedByUserID;

            if (newLicense.Save())
                return newLicense;
            else
                return null;
        }
    }
}
