using DVLD_DTOs;
using DataAccessLayer.DataSources;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsLicenseClass : clsBaseLogic<LicenseClassDTO>
    {
        public clsLicenseClass() : base() { }
        private clsLicenseClass(LicenseClassDTO dto) : base(dto) { }
        public static clsLicenseClass Find(int licenseClassID)
        {
            LicenseClassDTO dto = clsLicenseClassData.GetLicenseClassByID(licenseClassID);

            if (dto != null)
            {
                return new clsLicenseClass(dto);
            }

            return null;
        }
        public static clsLicenseClass Find(string className)
        {
            LicenseClassDTO dto = clsLicenseClassData.GetLicenseClassByClassName(className);
            if (dto != null)
            {
                return new clsLicenseClass(dto);
            }
            return null;
        }
        public static DataTable GetAllLicenseClass()
        {
            return clsLicenseClassData.GetAllLicenseClass();
        }
        protected override bool _AddNew()
        {
            this.DTO.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.DTO);
            return (this.DTO.LicenseClassID != -1);
        }
        protected override bool _Update()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.DTO);
        }
    }
}
