using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsTest : clsBaseLogic<TestDTO>
    {
        public clsTest() : base() { }
        private clsTest(TestDTO dto) : base(dto) { }
        public clsTestAppointment TestAppointmentInfo { get; set; }
        protected override bool _AddNew()
        {
            this.DTO.TestID = clsTestData.AddNewTest(this.DTO);

            if (this.DTO.TestID != -1)
            {
                return clsTestAppointmentData.LockTestAppointment(this.DTO.TestAppointmentID);
            }
            return false;
        }
        protected override bool _Update()
        {
            return clsTestData.UpdateTest(this.DTO);
        }
        public static clsTest Find(int ID)
        {
            TestDTO dto = clsTestData.GetTestInfoByID(ID);

            if (dto != null)
                return new clsTest(dto);
            else
                return null;
        }
        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            TestDTO dto = clsTestData.GetLastTest(PersonID, LicenseClassID, (int)TestTypeID);
            if (dto != null)
                return new clsTest(dto);
            else
                return null;

        }
        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }
        public static byte GetPassedTestCount(int LocalAppID)
        {
            return clsTestData.GetPassedTestCount(LocalAppID);
        }
        public static bool PassedAllTests(int LocalAppID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalAppID) == 3;
        }
    }
}
