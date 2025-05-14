using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace On_Bisc1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormCadastro form2 = new FormCadastro();
            form2.ShowDialog();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUÁRIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.White; // Cor do texto digitado
            }
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUÁRIO";
                txtUsuario.ForeColor = Color.Gray; // Cor do placeholder
            }
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_Leave(object sender, EventArgs e)
        {
            if (txtSenha.Text == "")
            {
                txtSenha.Text = "SENHA";
                txtSenha.ForeColor = Color.Gray;
                txtSenha.UseSystemPasswordChar = false; 
            }
        }

        private void guna2TextBox2_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "SENHA")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = Color.Gray;
                txtSenha.UseSystemPasswordChar = true;
                txtSenha.PasswordChar = '*'; 
            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {


        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text;

            if (usuario == "" || senha == "")
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string sql = "SELECT id, senha, tipo, nome FROM usuario WHERE nome_usuario = @usuario";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string senhaHashDB = reader.GetString("senha");
                            string tipo = reader.GetString("tipo");
                            string senhaHashDigitada = GerarHashSHA256(senha);
                            string nomeCompleto = reader.GetString("nome");

                            if (senhaHashDigitada == senhaHashDB)
                            {
                                // Login bem-sucedido, redireciona conforme o tipo de usuário
                                SessaoUsuario.UsuarioId = Convert.ToInt32(reader["id"]);
                                SessaoUsuario.Nome = reader["nome"].ToString();
                                SessaoUsuario.TipoUsuario = reader["tipo"].ToString();
                                this.Hide();

                                FormLoading loading = new FormLoading();
                                loading.Show();
                                loading.Refresh();

                                // Espera 1.5 segundos para simular o carregamento
                                await Task.Delay(1500);

                                loading.Close();
                                this.Hide();


                                if (tipo == "admin")
                                {
                                    new DashboardAdmin(nomeCompleto).Show();
                                }
                                else if (tipo == "prestador")
                                {
                                    new DashboardPrestador().Show();
                                }
                                else if (tipo == "cliente")
                                {
                                    new DashboardCliente(nomeCompleto).Show();

                                }
                                else
                                {
                                    MessageBox.Show("Tipo de usuário desconhecido.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Senha incorreta.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuário não encontrado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar: " + ex.Message);
            }
        }
        

        private string GerarHashSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
