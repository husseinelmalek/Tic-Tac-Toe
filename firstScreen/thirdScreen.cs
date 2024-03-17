using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace firstScreen
{
    public partial class thirdScreen : Form
    {
        public thirdScreen()
        {
            InitializeComponent();
            GetData();
            label1.BackColor = Color.Transparent;
        }

        private void GetData()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Server = DESKTOP-91QVAOD\\SQLEXPRESS ; Database = XOGame ; Integrated Security = SSPI ; TrustServerCertificate = True";

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM [Score]";

            command.Connection = con;
            con.Open();
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;


            con.Close();

        }
        public int id = 0;
        private void Button_Delete(object sender, DataGridViewCellMouseEventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Server=DESKTOP-91QVAOD\\SQLEXPRESS;Database=XOGame;trusted_connection=true;trustservercertificate=true";

            id = (int)dataGridView1.SelectedRows[0].Cells["id"].Value;
            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.CommandText = $"select playerX,playerXScore,PlayerO,playerOScore from Score where id= '{id}'";
            sqlCommand.Connection = con;

            try
            {
                con.Open();

                SqlDataReader re = sqlCommand.ExecuteReader();
                while (re.Read())
                {

                    textBox1.Text = dataGridView1.SelectedRows[0].Cells["playerX"].Value.ToString();
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells["playerXScore"].Value.ToString();
                    textBox3.Text = dataGridView1.SelectedRows[0].Cells["PlayerO"].Value.ToString();
                    textBox4.Text = dataGridView1.SelectedRows[0].Cells["playerOScore"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public void clear(){
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Server=DESKTOP-91QVAOD\\SQLEXPRESS;Database=XOGame;trusted_connection=true;trustservercertificate=true";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = $"delete from Score where id='{id}'";


            sqlCommand.Connection = con;

            try
            {
                con.Open();
                int effectedrow = sqlCommand.ExecuteNonQuery();
                label1.Text = $"{effectedrow} effected rows";
                GetData();
                clear();
               



            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
