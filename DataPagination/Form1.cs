using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataPagination
{
    public partial class NguyenVoDangCao : Form
    {
        int pageSize = 3;
        int pageCount = 1;
        int currentPage = 1;

        String conString = Properties.Settings.Default.db_a7df44_springbootdemoConnectionString;
        SqlConnection connection;
        SqlDataAdapter dataAdapter;
        public NguyenVoDangCao()
        {
            InitializeComponent();
            connection = new SqlConnection(conString);
            dataAdapter = new SqlDataAdapter("select * from Employee", connection);
            this.calculatePageCount();
        }

        private void calculatePageCount()
        {
            //SqlCommand countcmd = new SqlCommand("select count(*) from Employee", connection);
            // Select count(*) from ... craw all table and count, so this is bad practice for get count record
            SqlCommand countcmd = new SqlCommand("select rows from sysindexes where id=OBJECT_ID('Employee') and indid = 1", connection);
            connection.Open();
            int numrows = (int)countcmd.ExecuteScalar();
            connection.Close();
            pageCount = (int)Math.Ceiling((double)numrows / pageSize); // 17/5 = 3.4 -> 4
        }

        private void employeeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.employeeBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Employee' table. You can move, or remove it, as needed.
            //this.employeeTableAdapter.Fill(this.dataSet1.Employee);
            this.loadPage(currentPage);
        }

        private void loadPage(int page)
        {
            toolStripTextBox1.Text = currentPage.ToString();
            dataSet1.Employee.Clear(); // xóa nội dung
            dataAdapter.Fill(dataSet1, (page - 1) * pageSize, pageSize, "Employee");

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            this.loadPage(currentPage);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            currentPage = pageCount;
            this.loadPage(currentPage);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (currentPage < this.pageCount)
            {
                this.loadPage(++currentPage);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                this.loadPage(--currentPage);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int n = Int16.Parse(this.toolStripTextBox1.Text);
                if (n >= 1 && n <= 5)
                {
                    this.currentPage = n;
                    this.loadPage(n);
                }
                else
                {
                    MessageBox.Show("Invalid Page number", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Page number", "Error", MessageBoxButtons.OK);
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
