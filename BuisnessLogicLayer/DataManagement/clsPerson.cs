using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsPerson : clsBaseLogic<PersonDTO>
    {
        public string NationalityCountryName
        {
            get => clsCountryData.GetCountryNameByID(this.DTO.NationalityCountryID);
        }
        public string FullName
        {
            get
            {
                string full = this.DTO.FirstName + " " + this.DTO.SecondName;
                if (!string.IsNullOrWhiteSpace(this.DTO.ThirdName))
                    full += " " + this.DTO.ThirdName;
                full += " " + this.DTO.LastName;
                return full;
            }
        }
        public clsPerson() : base() { }
        private clsPerson(PersonDTO dto) : base(dto) { }
        public static clsPerson Find(int ID)
        {
            PersonDTO dto = clsPersonData.GetPersonInfoByID(ID);

            if (dto != null)
                return new clsPerson(dto); 
            else
                return null;
        }
        public static clsPerson Find(string NationalNo)
        {
            PersonDTO dto = clsPersonData.GetPersonInfoByNationalNo(NationalNo);

            if (dto != null)     
               return new clsPerson(dto);
            else
                return null;
        }
        protected override bool _AddNew()
        {
            this.DTO.PersonID = clsPersonData.AddNewPerson(this.DTO);
            return (this.DTO.PersonID != -1);
        }
        protected override bool _Update()
        {
            return clsPersonData.UpdatePerson(this.DTO);
        }
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
        public static bool Delete(int PersonID)
        {
            if (clsPersonData.IsPersonLinkedToOtherRecords(PersonID))
            {
                return false;
            }

            return clsPersonData.DeletePerson(PersonID);
        }
        public static bool IsExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }
        public static bool IsExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
    }
}
