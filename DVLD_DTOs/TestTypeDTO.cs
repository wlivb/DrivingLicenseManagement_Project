
namespace DVLD_DTOs
{
    public class TestTypeDTO
    {
        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; } // May be null
        public decimal TestTypeFees { get; set; }

        public TestTypeDTO (int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestTypeFees = testTypeFees;
        }

        public TestTypeDTO() { }
    }
}
