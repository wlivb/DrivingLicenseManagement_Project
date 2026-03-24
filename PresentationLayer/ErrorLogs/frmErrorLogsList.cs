using BuisnessLogicLayer.DataManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.ErrorLogs
{
    public partial class frmErrorLogsList : Form
    {
        public frmErrorLogsList()
        {
            InitializeComponent();
        }
        private void _LoadData()
        {
            DataTable dt = clsError.GetErrorLogs();

            dgvLogs.DataSource = dt;

            lblErrorCount.Text = "إجمالي الأخطاء المسجلة: " + dt.Rows.Count.ToString();

            if (dgvLogs.Columns.Count > 0)
            {
                if (dgvLogs.Columns.Contains("StackTrace"))
                    dgvLogs.Columns["StackTrace"].Visible = false;

                dgvLogs.Columns["LogID"].HeaderText = "Log ID";
                dgvLogs.Columns["Timestamp"].HeaderText = "Date";
                dgvLogs.Columns["QueryText"].HeaderText = "Query Text";

                dgvLogs.Columns["Timestamp"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            }
        }
        private void ErrorLogsList_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _LoadData();
        }
        private void dgvLogs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string errorMessage = dgvLogs.Rows[e.RowIndex].Cells["Message"].Value.ToString();
                string stackTrace = dgvLogs.Rows[e.RowIndex].Cells["StackTrace"].Value.ToString();
                string query = dgvLogs.Rows[e.RowIndex].Cells["QueryText"].Value.ToString();

                string fullDetails = $"Error: {errorMessage}\n\nQuery: {query}\n\nTrace: {stackTrace}";

                MessageBox.Show(fullDetails, "تفاصيل الخطأ التقنية", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
