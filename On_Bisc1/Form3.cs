using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace On_Bisc1
{
    public partial class DashboardCliente : Form
    {
        private string nomeCliente;
        public DashboardCliente(string nome)
        {
            InitializeComponent();
            nomeCliente = nome;
            this.WindowState = FormWindowState.Maximized;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            BemVindo.Text= $"Bem-vindo, {nomeCliente}!";
            int clienteId = SessaoUsuario.UsuarioId;
            CarregarUltimasSolicitacoes(clienteId);
         
            guna2DataGridView1.CellClick += guna2DataGridView1_CellClick;
        }
       

        public void CarregarUltimasSolicitacoes(int clienteId)
            {
            try
            {
                using (MySqlConnection conn = Conexao.Conectar())
                {

                    string query = @"SELECT s.id, sv.descricao, s.status, s.data_solicitacao
                         FROM solicitacoes s
                         JOIN servico sv ON s.servico_id = sv.id
                         WHERE s.cliente_id = @clienteId
                         ORDER BY s.data_solicitacao DESC
                         LIMIT 5";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@clienteId", clienteId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Aqui, aplicamos o Guna2DataGridView
                    guna2DataGridView1.DataSource = dt;
                    guna2DataGridView1.ReadOnly = true; // Impede edição direta
                    guna2DataGridView1.AllowUserToAddRows = false; // Remove linha em branco no final
                    guna2DataGridView1.AllowUserToDeleteRows = false; // Impede exclusão
                    guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Seleção da linha inteira
                    guna2DataGridView1.MultiSelect = false; // Apenas uma linha pode ser selecionada
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Colunas se ajustam ao painel

                    // Personalização do DataGridView
                    guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
                    guna2DataGridView1.GridColor = Color.Silver;
                    guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                    guna2DataGridView1.DefaultCellStyle.BackColor = Color.White;
                    guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                    guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.Teal;
                    guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                    guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal;
                    guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    guna2DataGridView1.ColumnHeadersHeight = 40;

                    // Ajuste automático das colunas
                    guna2DataGridView1.AutoResizeColumns();

                    // Alterando as cores das linhas com base no status
                    foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                    {
                        string status = row.Cells["status"].Value.ToString();

                        // Definindo cores para os status
                        if (status == "Pendente")
                        {
                            row.DefaultCellStyle.BackColor = Color.Orange;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (status == "Recusado")
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (status == "Aceite")
                        {
                            row.DefaultCellStyle.BackColor = Color.Green;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (status == "Em andamento")
                        {
                            row.DefaultCellStyle.BackColor = Color.Yellow;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar solicitações: " + ex.Message);
            }

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {
            guna2CustomGradientPanel3.BackColor = Color.Teal;
            
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Obtém o ID da solicitação da linha selecionada
                int solicitacaoId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["id"].Value);

                // Supondo que o ID do usuário logado (remetente) esteja armazenado em uma variável global ou já tenha sido configurado
                int remetenteId = SessaoUsuario.UsuarioId;  // Você pode substituir 'usuarioId' pelo valor real do usuário logado solicitacaoId, remetenteId

                // Instancia o FormChat, passando os parâmetros necessários
                FormChat chat = new FormChat(solicitacaoId, remetenteId, nomeCliente);
                this.Close();
                // Exibe o formulário
                chat.ShowDialog();
            }
            else
            {
                // Caso nenhuma solicitação tenha sido selecionada
                MessageBox.Show("Por favor, selecione uma solicitação para abrir o chat.");
            }
        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewUltimosPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                string status = guna2DataGridView1.Rows[e.RowIndex].Cells["status"].Value.ToString().ToLower();
                string nomeServico = guna2DataGridView1.Rows[e.RowIndex].Cells["descricao"].Value.ToString();

                labelNomeServico.Text = nomeServico;
                labelNomeServico.ForeColor = Color.White;

                switch (status)
                {
                    case "pendente":
                        guna2CustomGradientPanelStatus.FillColor = Color.Goldenrod;
                        guna2CustomGradientPanelStatus.FillColor2 = Color.Khaki;
                        guna2CustomGradientPanelStatus.FillColor3 = Color.DarkGoldenrod;
                        guna2CustomGradientPanelStatus.FillColor4 = Color.Orange;
                        labelStatusServico.Text = "Pendente";
                        break;

                    case "aceite":
                        guna2CustomGradientPanelStatus.FillColor = Color.SeaGreen;
                        guna2CustomGradientPanelStatus.FillColor2 = Color.MediumSeaGreen;
                        guna2CustomGradientPanelStatus.FillColor3 = Color.LightGreen;
                        guna2CustomGradientPanelStatus.FillColor4 = Color.DarkGreen;
                        labelStatusServico.Text = "Aceite";
                        break;

                    case "recusado":
                        guna2CustomGradientPanelStatus.FillColor = Color.IndianRed;
                        guna2CustomGradientPanelStatus.FillColor2 = Color.Tomato;
                        guna2CustomGradientPanelStatus.FillColor3 = Color.Firebrick;
                        guna2CustomGradientPanelStatus.FillColor4 = Color.Maroon;
                        labelStatusServico.Text = "Recusado";
                        break;

                    default:
                        guna2CustomGradientPanelStatus.FillColor = Color.Gray;
                        guna2CustomGradientPanelStatus.FillColor2 = Color.DarkGray;
                        guna2CustomGradientPanelStatus.FillColor3 = Color.LightGray;
                        guna2CustomGradientPanelStatus.FillColor4 = Color.Silver;
                        labelStatusServico.Text = status;
                        break;
                }

                labelStatusServico.ForeColor = Color.White;
            }
            else
            {
                // Resetar painel e labels
                guna2CustomGradientPanelStatus.FillColor = Color.LightGray;
                guna2CustomGradientPanelStatus.FillColor2 = Color.WhiteSmoke;
                guna2CustomGradientPanelStatus.FillColor3 = Color.Gainsboro;
                guna2CustomGradientPanelStatus.FillColor4 = Color.Silver;

                labelStatusServico.Text = "Status do Serviço";
                labelStatusServico.ForeColor = Color.Black;

                labelNomeServico.Text = "Nome do Serviço";
                labelNomeServico.ForeColor = Color.Black;
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            FormNovaSolicitacao novaSolicitacao = new FormNovaSolicitacao();
            DialogResult resultado = novaSolicitacao.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                // Atualiza a tabela imediatamente após a solicitação ser feita
                int clienteId = SessaoUsuario.UsuarioId;
                CarregarUltimasSolicitacoes(clienteId);
            }
        }
        private void CarregarSolicitacoesFiltradas(int clienteId, string termo)
        {
            try
            {
                using (var conn = Conexao.Conectar())
                {
                    string sql = @"
                SELECT s.id, sv.descricao, s.status, s.data_solicitacao
                FROM solicitacoes s
                  JOIN servico sv ON s.servico_id = sv.id
                WHERE s.cliente_id = @clienteId
                  AND (
                    sv.descricao    LIKE @busca
                    OR s.status     LIKE @busca
                    OR DATE_FORMAT(s.data_solicitacao, '%d/%m/%Y') LIKE @busca
                  )
                ORDER BY s.data_solicitacao DESC
            ";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@clienteId", clienteId);
                        cmd.Parameters.AddWithValue("@busca", $"%{termo}%");

                        var da = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);

                        dgvBusca.DataSource = dt;
                        // reaplique aqui as mesmas customizações de cores/Theme que fez no outro grid
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }

        private void guna2CustomGradientPanelStatus_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string termo = txtBusca.Text.Trim();
            int clienteId = SessaoUsuario.UsuarioId;

            if (string.IsNullOrEmpty(termo))
            {
                dgvBusca.DataSource = null;
                return;
            }

            // 2) Buscando em background e retornando o DataTable
            DataTable dt = await Task.Run(() => ObterSolicitacoesFiltradasDataTable(clienteId, termo));

            // 3) Atualizando o DataGridView **no thread da UI**
            dgvBusca.Invoke(new Action(() =>
            {
                dgvBusca.DataSource = dt;

                // Estilo geral
                dgvBusca.BorderStyle = BorderStyle.None;
                dgvBusca.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                dgvBusca.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvBusca.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 88, 255);
                dgvBusca.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvBusca.DefaultCellStyle.BackColor = Color.White;
                dgvBusca.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
                dgvBusca.DefaultCellStyle.Font = new Font("Segoe UI", 10);
                dgvBusca.EnableHeadersVisualStyles = false;
                dgvBusca.RowHeadersVisible = false;
                dgvBusca.AllowUserToAddRows = false;
                dgvBusca.ReadOnly = true;
                dgvBusca.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvBusca.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Cabeçalho
                dgvBusca.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvBusca.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
                dgvBusca.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvBusca.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvBusca.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvBusca.ColumnHeadersHeight = 35;

                // Linhas
                dgvBusca.RowTemplate.Height = 30;
            }));
        }
        private DataTable ObterSolicitacoesFiltradasDataTable(int clienteId, string termo)
        {
            var dt = new DataTable();
            string sql = @"
        SELECT s.id, sv.descricao, s.status, s.data_solicitacao
        FROM solicitacoes s
        JOIN servico sv ON s.servico_id = sv.id
        WHERE s.cliente_id = @clienteId
          AND (
            sv.descricao LIKE @busca
            OR s.status      LIKE @busca
            OR DATE_FORMAT(s.data_solicitacao, '%d/%m/%Y') LIKE @busca
          )
        ORDER BY s.data_solicitacao DESC;
    ";

            using (var conn = Conexao.Conectar())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@clienteId", clienteId);
                cmd.Parameters.AddWithValue("@busca", $"%{termo}%");
                new MySqlDataAdapter(cmd).Fill(dt);
            }
            return dt;
        }

        private void guna2PanelShadowResultados_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnMeusPedidos_Click(object sender, EventArgs e)
        {

        }

        private void btnServicos_Click(object sender, EventArgs e)
        {

        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            FormPerfilCliente perfil = new FormPerfilCliente(SessaoUsuario.UsuarioId, this); // `this` é o DashboardCliente
            perfil.Show();
            this.Hide();
        }
    }
}
