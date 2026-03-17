using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Properties;
using System.IO;
using System.Windows.Forms;

namespace PresentationLayer.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;
        private int _PersonId = -1;
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }
        public int PersonId
        {
            get { return _PersonId; } 
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if(_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with NationalNo = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void ResetPersonInfo()
        {
            _PersonId = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
        }
        private void _LoadPersonImage()
        {
            string ImagePath = _Person.DTO.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonId = _Person.DTO.PersonID;
            lblPersonID.Text = _Person.DTO.PersonID.ToString();
            lblNationalNo.Text = _Person.DTO.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGendor.Text = _Person.DTO.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.DTO.Email;
            lblPhone.Text = _Person.DTO.Phone;
            lblDateOfBirth.Text = _Person.DTO.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.DTO.NationalityCountryID).DTO.CountryName;
            lblAddress.Text = _Person.DTO.Address;
            _LoadPersonImage();
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonId);
            frm.ShowDialog();

            //refresh
            LoadPersonInfo(_PersonId);
        }
    }
}
