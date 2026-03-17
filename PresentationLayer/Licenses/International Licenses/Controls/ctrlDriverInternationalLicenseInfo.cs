using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Global_Classes;
using System.IO;
using System.Windows.Forms;

namespace PresentationLayer.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense;
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }
        private void _LoadPersonImage()
        {
            string ImagePath = _InternationalLicense.PersonInfo.DTO.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);
            if (_InternationalLicense == null)
            {
                MessageBox.Show("Could not find Internationa License ID = " + _InternationalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicense.DTO.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.InterLicenseDTO.IsActive ? "Yes" : "No";
            lblLocalLicenseID.Text = _InternationalLicense.InterLicenseDTO.IssuedUsingLocalLicenseID.ToString();
            lblFullName.Text = _InternationalLicense.PersonInfo.FullName;
            lblNationalNo.Text = _InternationalLicense.PersonInfo.DTO.NationalNo;
            lblGendor.Text = _InternationalLicense.PersonInfo.DTO.Gendor == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = clsFormat.DateToShort(_InternationalLicense.PersonInfo.DTO.DateOfBirth);

            lblDriverID.Text = _InternationalLicense.InterLicenseDTO.DriverID.ToString();
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicense.InterLicenseDTO.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicense.InterLicenseDTO.ExpirationDate);

            _LoadPersonImage();
        }
    }
}
