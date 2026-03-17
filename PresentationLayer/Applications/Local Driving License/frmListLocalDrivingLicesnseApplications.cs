using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Licenses;
using PresentationLayer.Licenses.Local_Licenses;
using PresentationLayer.Tests;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace PresentationLayer.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }
        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApp.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns["LocalDrivingLicenseApplicationID"].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns["LocalDrivingLicenseApplicationID"].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns["ClassName"].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns["ClassName"].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns["NationalNo"].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns["NationalNo"].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns["FullName"].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns["FullName"].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns["ApplicationDate"].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns["ApplicationDate"].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns["PassedTestCount"].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns["PassedTestCount"].Width = 150;

                if (dgvLocalDrivingLicenseApplications.Columns.Contains("ApplicationStatus"))
                    dgvLocalDrivingLicenseApplications.Columns["ApplicationStatus"].Visible = false;
            }

            cbFilterBy.SelectedIndex = 0;
        }
        private void _RefreshApplicationsList()
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApp.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns["LocalDrivingLicenseApplicationID"].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns["LocalDrivingLicenseApplicationID"].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns["ClassName"].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns["ClassName"].Width = 250;

                dgvLocalDrivingLicenseApplications.Columns["NationalNo"].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns["NationalNo"].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns["FullName"].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns["FullName"].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns["ApplicationDate"].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns["ApplicationDate"].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns["PassedTestCount"].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns["PassedTestCount"].Width = 100;

                dgvLocalDrivingLicenseApplications.Columns["StatusText"].HeaderText = "Status";
            }
        }
        private int _GetSelectedLocalDrivingLicenseApplicationID()
        {
            return (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(_GetSelectedLocalDrivingLicenseApplicationID());
            frm.ShowDialog();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {

                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;


                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                //in this case we deal with integer not string.
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication(_GetSelectedLocalDrivingLicenseApplicationID());
            frm.ShowDialog();

            _RefreshApplicationsList();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase L.D.L.AppID id is selected.
            if (cbFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void vistionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsTestAppointment Appointment =
                clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.VisionTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Vision Test Appointment Found!", "Set Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            frmTakeTest frm = new frmTakeTest(Appointment.DTO.TestAppointmentID, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
        }
        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;


            if (!clsLocalDrivingLicenseApp.DoesPassTestType(LocalDrivingLicenseApplicationID, clsTestType.enTestType.VisionTest))
            {
                MessageBox.Show("Person Should Pass the Vision Test First!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestAppointment Appointment =
               clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.WrittenTest);


            if (Appointment == null)
            {
                MessageBox.Show("No Written Test Appointment Found!", "Set Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            frmTakeTest frm = new frmTakeTest(Appointment.DTO.TestAppointmentID, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
        }
        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            frmListTestAppointments frm = new frmListTestAppointments(_GetSelectedLocalDrivingLicenseApplicationID(), TestType);
            frm.ShowDialog();
            _RefreshApplicationsList();
        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }
        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }
        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
            //refresh
            _RefreshApplicationsList();
        }
        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            //refresh
            _RefreshApplicationsList();
        }
        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            // 1. جلب المعرفات الأساسية باستخدام أسماء الأعمدة لضمان الدقة
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;

            // جلب الكائن من طبقة البزنس
            clsLocalDrivingLicenseApp LocalDrivingLicenseApplication = clsLocalDrivingLicenseApp.Find(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication == null)
            {
                e.Cancel = true; // إلغاء فتح القائمة إذا لم نجد البيانات
                return;
            }

            // 2. قراءة الحالة وعدد الاختبارات المجتازة من الجدول مباشرة (أسرع من استدعاء الداتا بيز مرة أخرى)
            int TotalPassedTests = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentRow.Cells["PassedTestCount"].Value);
            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            // تحديد حالة الطلب (New = 1)
            bool IsApplicationNew = (LocalDrivingLicenseApplication.DTO.ApplicationStatus == (byte)clsApplication.enApplicationStatus.New);

            // 3. التحكم في الأزرار الأساسية (Edit, Delete, Cancel)
            // هذه الأزرار تفعل فقط إذا كان الطلب جديداً
            editToolStripMenuItem.Enabled = IsApplicationNew && !LicenseExists;
            DeleteApplicationToolStripMenuItem.Enabled = IsApplicationNew;
            CancelApplicaitonToolStripMenuItem.Enabled = IsApplicationNew;

            // 4. التحكم في زر إصدار الرخصة لأول مرة
            // يفعل فقط إذا اجتاز 3 اختبارات ولم تصدر له رخصة بعد
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            // 5. التحكم في زر عرض الرخصة
            showLicenseToolStripMenuItem.Enabled = LicenseExists;

            // 6. منطق جدولة الاختبارات (Schedule Tests)
            // القائمة الرئيسية تفعل فقط إذا كان الطلب جديداً ولم يجتز كل الاختبارات
            ScheduleTestsMenue.Enabled = IsApplicationNew && (TotalPassedTests < 3);

            if (ScheduleTestsMenue.Enabled)
            {
                // نستخدم عدد الاختبارات المجتازة (TotalPassedTests) لتحديد أي اختبار نفتح بالترتيب:
                // 0 -> يفتح Vision فقط
                // 1 -> يفتح Written فقط
                // 2 -> يفتح Street فقط

                scheduleVisionTestToolStripMenuItem.Enabled = (TotalPassedTests == 0);
                scheduleWrittenTestToolStripMenuItem.Enabled = (TotalPassedTests == 1);
                scheduleStreetTestToolStripMenuItem.Enabled = (TotalPassedTests == 2);
            }
            else
            {
                // إذا كان الطلب مكتملاً أو ملغى، نغلق جميع الخيارات الفرعية
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }
        }
        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = _GetSelectedLocalDrivingLicenseApplicationID();

            int LicenseID = clsLocalDrivingLicenseApp.Find(LocalAppID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApp LocalDrivingLicenseApplication = clsLocalDrivingLicenseApp.Find(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    _RefreshApplicationsList();
                }
                else
                {
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApp LocalDrivingLicenseApplication = clsLocalDrivingLicenseApp.Find(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    _RefreshApplicationsList();
                }
                else
                {
                    MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApp localDrivingLicenseApplication = clsLocalDrivingLicenseApp.Find(LocalDrivingLicenseApplicationID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(localDrivingLicenseApplication.DTO.ApplicantPersonID);
            frm.ShowDialog();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
