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

namespace semana06
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CargarClientes();
            cargarOrdenes();
            dataGridView1.CellMouseEnter += dataGridView1_CellContentClick;

        }
        private void CargarClientes()
        {
            string connectionString = Properties.Settings.Default.cn;

            string procedureName = "pa_listaclientes";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message);
            }
        }
        void cargarOrdenes()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    SqlCommand cmd = new SqlCommand("pa_listaOrdenes", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ordenes");
                    dataGridView2.DataSource = ds.Tables["ordenes"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string idCliente = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(idCliente))
            {
                cargarOrdenesPorCliente(idCliente);
            }
            else
            {
                cargarOrdenes();
            }
        }
        void cargarOrdenesPorCliente(string idCliente)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cn))
                {
                    SqlCommand cmd = new SqlCommand("pa_listaOrdenesPorCliente", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcliente", idCliente);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ordenes");
                    dataGridView2.DataSource = ds.Tables["ordenes"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string idCliente = dataGridView1.Rows[e.RowIndex].Cells["IdCliente"].Value.ToString();
                cargarOrdenesPorCliente(idCliente);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}


