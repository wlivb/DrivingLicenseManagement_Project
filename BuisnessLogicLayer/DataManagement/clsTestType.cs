using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsTestType : clsBaseLogic<TestTypeDTO>
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public clsTestType() : base() { }
        private clsTestType(TestTypeDTO dto) : base(dto) { }

        public static clsTestType Find(int ID)
        {
            TestTypeDTO dto = clsTestTypeData.GetTestTypeInfoByID(ID);
            if (dto != null)
                return new clsTestType(dto);
            else
                return null;
        }
        public static clsTestType Find(string Title)
        {
            TestTypeDTO dto = clsTestTypeData.GetTestTypeInfoByTitle(Title);
            if (dto != null)
                return new clsTestType(dto);
            else
                return null;
        }

        protected override bool _AddNew()
        {
            this.DTO.TestTypeID = clsTestTypeData.AddNewTestType(this.DTO);
            return (this.DTO.TestTypeID != -1); 
        }

        protected override bool _Update()
        {
            return clsTestTypeData.UpdateTestType(this.DTO);
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestType();
        }

    }
}
