using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DTOs
{
    public class ApplicationDTO
    {
      
        public int ApplicationID { get; set; } 
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; } // Date the status was last updated 
        public decimal PaidFees { get; set; } // Total fees paid for this application
        public int CreatedByUserID { get; set; }

        public ApplicationDTO(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, byte ApplicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
        }

        public ApplicationDTO() { }

    }
}
