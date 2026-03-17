using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Global_Classes;
using System.IO;
using System.Windows.Forms;

namespace PresentationLayer.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID;
        private clsLicense _License;

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        { 
            get
            {
                return _License;
            }
        }

        private void _LoadPersonImage()
        {
            string ImagePath = _License.DriverInfo.PersonInfo.DTO.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.Find(_LicenseID);
            if (_License == null)
            {
                MessageBox.Show("Could not find License ID = " + _LicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }
           
                lblLicenseID.Text = _License.DTO.LicenseID.ToString();
                lblIsActive.Text = _License.DTO.IsActive ? "Yes" : "No";
                lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
                lblClass.Text = _License.LicenseClassInfo.DTO.ClassName.ToString();
                lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
                lblNationalNo.Text = _License.DriverInfo.PersonInfo.DTO.NationalNo;
                lblGendor.Text = _License.DriverInfo.PersonInfo.DTO.Gendor == 0 ? "Male" : "Female";
                lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DTO.DateOfBirth);

                lblDriverID.Text = _License.DTO.DriverID.ToString();
                lblIssueDate.Text = clsFormat.DateToShort(_License.DTO.IssueDate);
                lblExpirationDate.Text = clsFormat.DateToShort(_License.DTO.ExpirationDate);
                lblIssueReason.Text = _License.IssueReasonText;
                lblNotes.Text = _License.DTO.Notes == "" ? "No Notes" : _License.DTO.Notes;
                _LoadPersonImage();
            
        }
    }
}
