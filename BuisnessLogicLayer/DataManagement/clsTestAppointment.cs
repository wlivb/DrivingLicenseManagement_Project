using BuisnessLogicLayer;
using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsTestAppointment : clsBaseLogic<TestAppointmentDTO>
    {
        public clsTestAppointment() : base() { }
        private clsTestAppointment(TestAppointmentDTO dto) : base(dto) { }

        private clsApplication _RetakeTestAppInfo;
        public clsApplication RetakeTestAppInfo
        {
            get
            {
                if (_RetakeTestAppInfo == null)
                {
                    _RetakeTestAppInfo = clsApplication.Find(this.DTO.RetakeTestApplicationID);
                }

                return _RetakeTestAppInfo;
            }
        }
        public int TestID
        {
            get { return clsTestAppointmentData.GetTestID(this.DTO.TestAppointmentID); }
        }
        public static clsTestAppointment Find(int ID)
        {
            TestAppointmentDTO dto = clsTestAppointmentData.GetTestAppointmentInfoByID(ID);
            if (dto != null)
                return new clsTestAppointment(dto);
            else
                return null;
        }
        public static clsTestAppointment Find (clsTestType.enTestType TestTypeID)
        {
            TestAppointmentDTO dto = clsTestAppointmentData.GetTestAppointmentInfoByTestTypeID((int)TestTypeID);
            if (dto != null)
                return new clsTestAppointment(dto);
            else
                return null;
        }
        protected override bool _AddNew()
        {
            if (this.DTO.RetakeTestApplicationID == 0)
                this.DTO.RetakeTestApplicationID = -1;

            this.DTO.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment(this.DTO);
            return (this.DTO.TestAppointmentID != -1);
        }
        protected override bool _Update()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.DTO);
        }
        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            bool isFound = false;

            TestAppointmentDTO dto = clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID, ref isFound);
            if (dto != null & isFound == true)
                return new clsTestAppointment(dto);
            else
                return null;
        }
        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }
        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.DTO.LocalDrivingLicenseAppID, (int)TestTypeID);
        }
        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseAppID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseAppID, (int)TestTypeID);
        }    
    }
}
