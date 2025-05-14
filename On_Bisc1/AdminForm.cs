using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace On_Bisc1
{
    public partial class DashboardAdmin : Form
    {
        private string nomeAdmin;
        public DashboardAdmin(string nome)
        {
            InitializeComponent();
            carregarTotais();
            nomeAdmin = nome;
        }
        private void carregarTotais()
        {
            numero_usuarios.Text = ContarRegistros("usuario").ToString();
            numero_servicos.Text = ContarRegistros("servico").ToString();
            numero_solicitacoes.Text = ContarRegistros("solicitacoes").ToString();
            // Defina o texto do label com o nome do usuário logado
            labelUsuario.Text = $"Bem-vindo, {nomeAdmin}!";

        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
        }
        private int ContarRegistros(string tabela)
        {
            int total = 0;

            try
            {
                using (MySqlConnection conn = Conexao.Conectar())
                {
                    string query = $"SELECT COUNT(*) FROM {tabela}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    total = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao contar registros da tabela '{tabela}': " + ex.Message);
            }

            return total;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void numero_usuarios_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
