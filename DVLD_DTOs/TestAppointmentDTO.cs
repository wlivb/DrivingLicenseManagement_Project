using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DTOs
{
    public class TestAppointmentDTO
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseAppID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; } // May be null
        public TestAppointmentDTO(int Id, int testTypeId, int LocalAppId, DateTime date, decimal fees, int creatByUsr, bool isLocked, int retakeTestAppId)
        {
            this.TestAppointmentID = Id; this.TestTypeID = testTypeId; this.LocalDrivingLicenseAppID = LocalAppId;
            this.AppointmentDate = date; this.PaidFees = fees; this.CreatedByUserID = creatByUsr;
            this.IsLocked = isLocked; this.RetakeTestApplicationID = retakeTestAppId;
        }
        public TestAppointmentDTO() { }
    }
}
