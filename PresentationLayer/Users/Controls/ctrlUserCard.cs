using BuisnessLogicLayer.DataManagement;
using System.Windows.Forms;

namespace PresentationLayer.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _User;
        private int _UserID = -1;
        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _UserID = UserID;
            _FillUserInfo();
        }
        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.DTO.PersonID);
            lblUserID.Text = _User.DTO.UserID.ToString();
            lblUserName.Text = _User.DTO.UserName;

            if (_User.DTO.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }
        private void _ResetPersonInfo()
        {
            _User = null;
            _UserID = -1;
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}
