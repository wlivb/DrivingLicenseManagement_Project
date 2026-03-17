using BuisnessLogicLayer.DataManagement;
using PresentationLayer.Global_Classes;
using System;
using System.Windows.Forms;

namespace PresentationLayer.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _AppointmentID;
        private clsTestType.enTestType _TestType;

        private clsTest _Test;

        public frmTakeTest(int AppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
            _TestType = TestType;
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.TestTypeID = _TestType;
            ctrlSecheduledTest1.LoadInfo(_AppointmentID);

            // التحقق من صلاحية الموعد
            if (ctrlSecheduledTest1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            int _TestID = ctrlSecheduledTest1.TestID;

            if (_TestID != -1) // الاختبار موجود مسبقاً (وضع العرض فقط)
            {
                _Test = clsTest.Find(_TestID);

                if (_Test != null && _Test.DTO != null)
                {
                    rbPass.Checked = (_Test.DTO.TestResult == 1);
                    rbFail.Checked = (_Test.DTO.TestResult == 0);
                    txtNotes.Text = _Test.DTO.Notes;

                    lblUserMessage.Visible = true;
                    rbFail.Enabled = false;
                    rbPass.Enabled = false;
                    btnSave.Enabled = false; // تعطيل الحفظ لأن الاختبار مكتمل
                    txtNotes.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Could not find test details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else // اختبار جديد (وضع الإضافة)
            {
                _Test = new clsTest();
                _Test.DTO.TestAppointmentID = _AppointmentID; // أهم سطر للربط
                _Test.DTO.CreatedByUserID = clsGlobal.CurrentUser.DTO.UserID;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                      "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No
             )
            {
                return;
            }

            _Test.DTO.TestAppointmentID = _AppointmentID;
            _Test.DTO.TestResult = (byte)(rbPass.Checked ? 1 : 0);
            _Test.DTO.Notes = txtNotes.Text.Trim();
            _Test.DTO.CreatedByUserID = clsGlobal.CurrentUser.DTO.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
