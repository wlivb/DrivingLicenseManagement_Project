using DataAccessLayer.DataSources;
using DVLD_DTOs;
using System.Data;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsCountry : clsBaseLogic<CountryDTO>
    {
        public clsCountry() : base() { }

        private clsCountry(CountryDTO dto) : base(dto) { }

        public static clsCountry Find(int ID)
        {
            CountryDTO dto = clsCountryData.GetCountryInfoByID(ID);
            if (dto != null)
                return new clsCountry(dto);
            else
                return null;
        }
        public static clsCountry Find(string Name)
        {
            CountryDTO dto = clsCountryData.GetCountryInfoByName(Name);
            if (dto != null)
                return new clsCountry(dto);
            else
                return null;
        }

        protected override bool _AddNew()
        {
            this.DTO.CountryID = clsCountryData.AddNewCountry(this.DTO);
            return (this.DTO.CountryID != -1);
        }
        protected override bool _Update()
        {
            return clsCountryData.UpdateCountry(this.DTO);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
        public static bool Delete(int ID)
        {
            return clsCountryData.DeleteCountry(ID);
        }
    }
}
