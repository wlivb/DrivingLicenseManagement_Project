
namespace DVLD_DTOs
{
    public class TestDTO
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public byte TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public TestDTO(int ID, int AppointmentID, byte Result, string Notes, int CreatedByUserID)
        {
            this.TestID = ID;
            this.TestAppointmentID = AppointmentID;
            this.TestResult = Result;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
        }
        public TestDTO() { }

    }
}
