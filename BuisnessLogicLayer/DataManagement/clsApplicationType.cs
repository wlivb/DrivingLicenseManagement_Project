using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsApplicationType : clsBaseLogic<ApplicationTypeDTO>
    {
        public clsApplicationType() : base() { }

        private clsApplicationType(ApplicationTypeDTO dto) : base(dto) { }

        public static clsApplicationType Find(int ID)
        {
            ApplicationTypeDTO dto = clsApplictionTypeData.GetApplicationTypeInfoByID(ID);
            if (dto != null)
                return new clsApplicationType(dto);
            else
                return null;
        }
        public static clsApplicationType Find(string Title)
        {
            ApplicationTypeDTO dto = clsApplictionTypeData.GetApplicationTypeInfoByTitle(Title);
            if (dto != null)
                return new clsApplicationType(dto);
            else
                return null;
        }

        protected override bool _AddNew()
        {
            this.DTO.ApplicationTypeID = clsApplictionTypeData.AddNewApplicationType(this.DTO);
            return (this.DTO.ApplicationTypeID != -1);
        }

        protected override bool _Update()
        {
            return clsApplictionTypeData.UpdateApplicationType(this.DTO);
        }

        public static DataTable GetAllApplictionTypes()
        {
            return clsApplictionTypeData.GetAllApplicationType();
        }
    }
}

