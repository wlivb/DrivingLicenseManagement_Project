using System;

namespace DVLD_DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } 

        public UserDTO(int UserID, int PerID, string UName, string Pas, bool IsAct)
        {
            this.UserID = UserID; this.PersonID = PerID; this.UserName = UName;
            this.Password = Pas; this.IsActive = IsAct;
        }
        public UserDTO() { }
    }

}
