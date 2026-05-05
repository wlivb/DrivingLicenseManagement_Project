using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Global_Classes;
using PresentationLayer.People;
using System.Windows.Forms;

namespace PresentationLayer.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplication _Application;

        private int _ApplicationID = -1;
        public int ApplicationID
        {
            get { return _ApplicationID; }
        }
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.Find(ApplicationID);
            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }
        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.DTO.ApplicationID;
            lblApplicationID.Text = _Application.DTO.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblType.Text = _Application.ApplicationTypeInfo.DTO.ApplicationTypeTitle;
            lblFees.Text = _Application.DTO.PaidFees.ToString();
            lblApplicant.Text = _Application.ApplicantFullName;
            lblDate.Text = clsFormat.DateToShort(_Application.DTO.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_Application.DTO.LastStatusDate);
            lblCreatedByUser.Text = _Application.CreatedByUserName;
        }
        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";
        }
        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Application == null)
            {
                return;
            }

            frmShowPersonInfo frm = new frmShowPersonInfo(_Application.DTO.ApplicantPersonID);
            frm.ShowDialog();

            //Refresh
            LoadApplicationInfo(_ApplicationID);
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
