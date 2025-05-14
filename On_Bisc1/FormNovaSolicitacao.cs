using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace On_Bisc1
{
    public partial class FormNovaSolicitacao : Form
    {
        public FormNovaSolicitacao()
        {
            InitializeComponent();
        }

        private void FormNovaSolicitacao_Load(object sender, EventArgs e)
        {
            CarregarServicos();
        }

        private void CarregarServicos()
        {
            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string sql = "SELECT id, descricao FROM servico";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbServicos.DataSource = dt;
                    cbServicos.DisplayMember = "descricao";
                    cbServicos.ValueMember = "id";
                    cbServicos.SelectedIndex = -1; // Nenhum selecionado
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar serviços: " + ex.Message);
            }
        }

        private void btnSelecionarImagem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbImagem.Image = Image.FromFile(openFileDialog.FileName);
                pbImagem.Tag = openFileDialog.FileName;
            }
        }

        private void btnEnviarSolicitacao_Click(object sender, EventArgs e)
        {
            int clienteId = SessaoUsuario.UsuarioId;

            if (cbServicos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um serviço.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int servicoId = Convert.ToInt32(cbServicos.SelectedValue);
            string descricao = txtDescricao.Text.Trim();

            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string sql = "INSERT INTO solicitacoes (cliente_id, servico_id, status) " +
                                 "VALUES (@cliente_id, @servico_id, 'pendente')";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cliente_id", clienteId);
                        cmd.Parameters.AddWithValue("@servico_id", servicoId);

                        cmd.ExecuteNonQuery();
                    }

                    // ✅ Mensagem de sucesso estilizada
                    // Após salvar a solicitação com sucesso
                    DialogResult result = MessageBox.Show(
                        "Solicitação enviada com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    if (result == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK; // Importante para sinalizar ao formulário pai
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar solicitação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LimparCampos()
        {
            cbServicos.SelectedIndex = -1;
            txtDescricao.Clear();
            pbImagem.Image = null;
        }
    }
}

