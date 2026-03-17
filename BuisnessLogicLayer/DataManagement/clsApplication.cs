using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsApplication : clsBaseLogic<ApplicationDTO>
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1,
            RenewDrivingLicense = 2,
            ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4,
            ReleaseDetainedDrivingLicense = 5,
            NewInternationalLicense = 6,
            RetakeTest = 7
        }
        public enum enApplicationStatus
        {
            New = 1,
            Cancelled = 2,
            Completed = 3
        }
        public clsApplication() : base() { }
        protected clsApplication(ApplicationDTO dto) : base(dto) { }

        private clsPerson _PersonInfo;
        public clsPerson PersonInfo
        {
            get
            {
                if (_PersonInfo == null)
                {
                    _PersonInfo = clsPerson.Find(this.DTO.ApplicantPersonID);
                }

                return _PersonInfo;
            }
        }

        private clsApplicationType _ApplicationTypeInfo;
        public clsApplicationType ApplicationTypeInfo
        {
            get
            {
                if (_ApplicationTypeInfo == null)
                {
                    _ApplicationTypeInfo = clsApplicationType.Find(this.DTO.ApplicationTypeID);
                }
                return _ApplicationTypeInfo;
            }
        }

        private clsUser _User;
        public clsUser User
        {
            get
            {
                if (_User == null)
                {
                    _User = clsUser.Find(this.DTO.CreatedByUserID);
                }
                return _User;
            }

        }

        public string ApplicantFullName => PersonInfo?.FullName ?? "Unknown";
        public string ApplicationTypeName => ApplicationTypeInfo?.DTO.ApplicationTypeTitle ?? "Unknown";
        public string CreatedByUserName => User?.DTO.UserName ?? "Unknown";
        public static clsApplication Find(int applicationID)
        {
            ApplicationDTO dto = clsApplicationData.GetApplicationInfoByID(applicationID);
            if (dto != null)
            {
                return new clsApplication(dto);
            }
            return null;
        }
        protected override bool _AddNew()
        {
            this.DTO.ApplicationID = clsApplicationData.AddNewApplication(this.DTO);
            return (this.DTO.ApplicationID != -1);
        }
        protected override bool _Update()
        {
            return clsApplicationData.UpdateApplication(this.DTO);
        }
        public bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.DTO.ApplicationID);
        }
        public bool Cancel()
        { 
            if (clsApplicationData.UpdateApplicationStatus(this.DTO.ApplicationID, (byte)enApplicationStatus.Cancelled))
            {
                this.DTO.ApplicationStatus = (byte)enApplicationStatus.Cancelled;
                this.DTO.LastStatusDate = DateTime.Now;
                return true;
            }
            return false;
        }
        public bool SetComplete()
        {
            if (clsApplicationData.UpdateApplicationStatus(this.DTO.ApplicationID, (byte)enApplicationStatus.Completed))
            {
                this.DTO.ApplicationStatus = (byte)enApplicationStatus.Completed;
                this.DTO.LastStatusDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int AppTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, AppTypeID);
        }
        public string StatusText => ((enApplicationStatus)this.DTO.ApplicationStatus).ToString();
        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int LicenseClassID, enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, LicenseClassID, (int)ApplicationTypeID);
        }
        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications(); 
        }
        public static bool IsApplicationExist(int applicationID)
        {
            return clsApplicationData.IsApplicationExist(applicationID);
        }
    }
}
