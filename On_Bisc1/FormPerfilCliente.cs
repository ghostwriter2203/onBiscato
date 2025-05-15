using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace On_Bisc1
{
    public partial class FormPerfilCliente : Form
    {
        private int clienteId;
        private Form DashboardCliente;
        public FormPerfilCliente(int idCliente, Form dashboardForm)
        {
            InitializeComponent();
            clienteId = idCliente;
            DashboardCliente = dashboardForm;
            CarregarDadosCliente();
        }

        private void CarregarDadosCliente()
        {
            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string query = "SELECT nome, email, telefone FROM usuario WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", clienteId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["nome"].ToString();
                                txtEmail.Text = reader["email"].ToString();
                                txtTelefone.Text = reader["telefone"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do cliente: " + ex.Message);
            }
        }

        private void FormPerfilCliente_Load(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            txtName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtTelefone.ReadOnly = false;
        }

        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string update = @"UPDATE usuario 
                                      SET nome = @nome, email = @email, telefone = @telefone 
                                      WHERE id = @id";
                    using (var cmd = new MySqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", clienteId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dados atualizados com sucesso!");

                        DashboardCliente.Show(); // Mostra o formulário anterior
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar perfil: " + ex.Message);
            }
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}

