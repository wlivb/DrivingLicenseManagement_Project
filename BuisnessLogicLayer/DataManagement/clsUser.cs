using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsUser : clsBaseLogic<UserDTO>
    {
        private clsPerson _PersonInfo;
        public clsPerson PersonInfo
        {
            get
            {
                if (_PersonInfo == null)
                    _PersonInfo = clsPerson.Find(this.DTO.PersonID);
                return _PersonInfo;
            }
        }

        public clsUser() : base() { }
        private clsUser(UserDTO dto) : base(dto) { }

        public static clsUser Find(int ID)
        {
            UserDTO dto = clsUserData.GetUserInfoByID(ID);

            if (dto != null)
                return new clsUser(dto);
            else
                return null;
        }
        public static clsUser Find(string UserName)
        {
            UserDTO dto = clsUserData.GetUserInfoByUserName(UserName);

            if (dto != null)
                return new clsUser(dto);
            else
                return null;
        }
        public static clsUser Find(string UserName, string Password)
        {
            string HashedPassword = clsUtil.ComputeHash(Password);

            UserDTO dto = clsUserData.GetUserByUserNameAndPassword(UserName, HashedPassword);

            if (dto != null)
                return new clsUser(dto);
            else
                return null;
        }
        protected override bool _AddNew()
        {
            if(!IsPersonAlreadyUser(this.DTO.PersonID))
            {
                this.DTO.Password = clsUtil.ComputeHash(this.DTO.Password);

                this.DTO.UserID = clsUserData.AddNewUser(this.DTO);
                return (this.DTO.UserID != -1);
            } 
           return false;
        }
        protected override bool _Update()
        {
            return clsUserData.UpdateUser(this.DTO);
        }
        public bool ChangePassword(string NewPassword)
        {
            string HashedPassword = clsUtil.ComputeHash(NewPassword);

            return clsUserData.ChangeUserPassword(this.DTO.UserID, HashedPassword);
        }
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static bool Delete(int ID)
        {
            return clsUserData.DeleteUser(ID);
        }
        public static bool IsExist(int ID)
        {
            return clsUserData.IsUserExist(ID);
        }
        public static bool IsExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }
        public static bool IsPersonAlreadyUser(int PersonID)
        {
            return clsUserData.IsPersonAlreadyUser(PersonID);
        }
    }
}
