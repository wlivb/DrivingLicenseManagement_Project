using System.Data;
using DataAccessLayer.DataSources;
using DVLD_DTOs;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsDriver : clsBaseLogic<DriverDTO>
    {
        public clsDriver() : base() {}
        private clsDriver(DriverDTO dto) : base(dto) {}

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
        public static clsDriver FindByDriverID(int ID)
        {
            DriverDTO dto = clsDriverData.GetDriverInfoByID(ID);
            if (dto != null)
                return new clsDriver(dto);
            else
                return null;
        }
        public static clsDriver FindByPersonID(int personID)
        {
            DriverDTO dto = clsDriverData.GetDriverInfoByPersonID(personID);
            if (dto != null)
                return new clsDriver(dto);
            else
                return null;
        }
        protected override bool _AddNew()
        {
            this.DTO.DriverID = clsDriverData.AddNewDriver(this.DTO);
            return (this.DTO.DriverID != -1);
        }
        protected override bool _Update()
        {
            return clsDriverData.UpdateDriver(this.DTO);
        }
        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers(); 
        }
        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }
        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }
    }
}
