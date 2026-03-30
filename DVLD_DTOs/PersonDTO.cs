
namespace DVLD_DTOs
{
    public class PersonDTO
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; } // قد يكون Null
        public string LastName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } // قد يكون Null
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; } // قد يكون Null

        public PersonDTO(int ID, string NatNo, string FName, string SName, string TName, string LName,
                 System.DateTime DOB, byte Gndr, string Addr, string Phn, string Eml, int CountryID, string Img)
        {
            this.PersonID = ID; this.NationalNo = NatNo; this.FirstName = FName;
            this.SecondName = SName; this.ThirdName = TName; this.LastName = LName;
            this.DateOfBirth = DOB; this.Gendor = Gndr; this.Address = Addr;
            this.Phone = Phn; this.Email = Eml; this.NationalityCountryID = CountryID;
            this.ImagePath = Img;
        }

        public PersonDTO() { }
    }

}
