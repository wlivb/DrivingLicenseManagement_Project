

namespace DVLD_DTOs
{
    public class ApplicationTypeDTO
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationTypeFees { get; set; }

        public ApplicationTypeDTO(int ID, string Title, decimal Fees)
        {
            this.ApplicationTypeID = ID;
            this.ApplicationTypeTitle = Title;
            this.ApplicationTypeFees = Fees;
        }

        public ApplicationTypeDTO() { }

    }
}
