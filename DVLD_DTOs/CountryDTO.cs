using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DTOs
{
    public class CountryDTO
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public CountryDTO(int ID, string Name)
        {
            this.CountryID = ID; this.CountryName = Name;
        }

        public CountryDTO() { }

    }
}
