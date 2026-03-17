using DataAccessLayer.DataSources;
using DVLD_DTOs;    
using System;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsLocalDrivingLicenseApp : clsApplication
    {
        public LocalDrivingLicenseAppDTO LocalAppDTO { get; set; }

        private clsLicenseClass _LicenseClassInfo;
        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                if (_LicenseClassInfo == null)
                    _LicenseClassInfo = clsLicenseClass.Find(this.LicenseClassID);
                return _LicenseClassInfo;
            }
        }
        public int LocalDrivingLicenseAppID
        {
            get => LocalAppDTO.LocalDrivingLicenseAppID;
            set => LocalAppDTO.LocalDrivingLicenseAppID = value;
        }
        public int LicenseClassID
        {
            get => LocalAppDTO.LicenseClassID;
            set => LocalAppDTO.LicenseClassID = value;
        }
        public string PersonFullName => this.PersonInfo.FullName;
        public clsLocalDrivingLicenseApp() : base()
        {
            this.LocalAppDTO = new LocalDrivingLicenseAppDTO();
            this.LocalDrivingLicenseAppID = -1;
            this.Mode = enMode.AddNew;
        }
        private clsLocalDrivingLicenseApp(ApplicationDTO appDTO, LocalDrivingLicenseAppDTO localDTO) : base(appDTO)
        {
            this.LocalAppDTO = localDTO;
            this.Mode = enMode.Update;
        }
        public static new clsLocalDrivingLicenseApp Find(int LocalDrivingLicenseApplicationID)
        {
            var fullInfo = clsLocalDrivingLicenseAppData.GetLocalDrivingLicenseAppFullInfoByID(LocalDrivingLicenseApplicationID);

            if (fullInfo != null)
            {
                return new clsLocalDrivingLicenseApp(fullInfo.Application, fullInfo.LocalApp);
            }

            return null;
        }
        private bool _AddNewLocalDrivingLicenseApp()
        {
            this.LocalDrivingLicenseAppID = clsLocalDrivingLicenseAppData.AddNewLocalDrivingLicenseApplication(this.LocalAppDTO);

            return (this.LocalDrivingLicenseAppID != -1);
        }
        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseAppData.UpdateLocalDrivingLicenseApplication 
                   (this.LocalAppDTO);
        }
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseAppData.GetAllLocalDrivingLicenseApplications();
        }
        public new bool Delete()
        {
            return clsLocalDrivingLicenseAppData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseAppID, base.DTO.ApplicationID);
        }
        public static bool Delete(int LocalAppID, int AppID)
        {
            return clsLocalDrivingLicenseAppData.DeleteLocalDrivingLicenseApplication(LocalAppID, AppID);
        }
        public new bool Save()
        {
            enMode currentMode = this.Mode; // لتفادي تغير المود بالكلاس الأب وحدوث مشكلة 

            if (!base.Save()) return false;

            this.LocalAppDTO.ApplicationID = this.DTO.ApplicationID;

            switch (currentMode)
            {
                case enMode.AddNew:
                    this.LocalDrivingLicenseAppID =
                    clsLocalDrivingLicenseAppData.AddNewLocalDrivingLicenseApplication(this.LocalAppDTO);
                    return (this.LocalDrivingLicenseAppID != -1);

                case enMode.Update:
                    return clsLocalDrivingLicenseAppData.UpdateLocalDrivingLicenseApplication(this.LocalAppDTO);
            }

            return false;
        }
        public bool DoesPassTestType(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.DoesPassTestType(this.LocalDrivingLicenseAppID, (int)testTypeID);
        }
        public static bool DoesPassTestType(int localAppId,clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.DoesPassTestType(localAppId, (int)testTypeID);
        }
        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {
            if (CurrentTestType == clsTestType.enTestType.VisionTest)
                return true;
            if (CurrentTestType == clsTestType.enTestType.WrittenTest)
                return this.DoesPassTestType(clsTestType.enTestType.VisionTest);
            if (CurrentTestType == clsTestType.enTestType.StreetTest)
                return this.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            return false;
        }
        public bool DoesAttendTestType(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.DoesAttendTestType(this.LocalDrivingLicenseAppID, (int)testTypeID);
        }
        public int TotalTrialsPerTest(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.TotalTrialsPerTest(this.LocalDrivingLicenseAppID, (int)testTypeID);
        }
        public static int TotalTrialsPerTest(int localAppId, clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.TotalTrialsPerTest(localAppId, (int)testTypeID);
        }
        public static bool AttendedTest(int localAppId, clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.TotalTrialsPerTest(localAppId, (int)testTypeID) > 0;
        }
        public bool AttendedTest(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.TotalTrialsPerTest(this.LocalDrivingLicenseAppID, (int)testTypeID) > 0;
        }
        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseAppID, (int)testTypeID);
        }
        public static bool IsThereAnActiveScheduledTest(int localAppId, clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseAppData.IsThereAnActiveScheduledTest(localAppId, (int)testTypeID);
        }
        public bool IsPassedAllTests()
        {
           return clsTest.PassedAllTests(this.LocalDrivingLicenseAppID);
        }
        public static bool IsPassedAllTests(int localAppId)
        {
            return clsTest.PassedAllTests(localAppId);
        }
        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(base.DTO.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.PersonInfo.DTO.PersonID, this.LicenseClassID);
        }
        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }
        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalAppDTO.LocalDrivingLicenseAppID);
        }
        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver Driver = clsDriver.FindByPersonID(this.DTO.ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriver();

                Driver.DTO.PersonID = this.DTO.ApplicantPersonID;
                Driver.DTO.CreatedByUserID = CreatedByUserID;
                Driver.DTO.CreatedDate = DateTime.Now;
                if (Driver.Save())
                {
                    DriverID = Driver.DTO.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DTO.DriverID;
            }
            //now we diver is there, so we add new licesnse

            clsLicense License = new clsLicense();
            License.DTO.ApplicationID = this.DTO.ApplicationID;
            License.DTO.DriverID = DriverID;
            License.DTO.LicenseClassID = this.LicenseClassID;
            License.DTO.IssueDate = DateTime.Now;
            License.DTO.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DTO.DefaultValidityLength);
            License.DTO.Notes = Notes;
            License.DTO.PaidFees = this.LicenseClassInfo.DTO.ClassFees;
            License.DTO.IsActive = true;
            License.DTO.IssueReason = 1; // = clsLicense.enIssueReason.FirstTime
            License.DTO.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();

                return License.DTO.LicenseID;
            }

            else
                return -1;
        }
    }

}
