using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace On_Bisc1
{
    public partial class FormChat : Form
    {
        private string nomeCliente;
        private int solicitacaoId;
        private int remetenteId;
        private Timer timerMensagens;
        private DateTime ultimaMensagemRecebida = DateTime.MinValue;

        public FormChat(int solicitacaoId, int remetenteId, string nome)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.solicitacaoId = solicitacaoId;
            this.remetenteId = remetenteId;
            lblSolicitacao.Text = $"Chat da Solicitação ID: {solicitacaoId}";

            CarregarMensagens();
            timerMensagens = new Timer();
            timerMensagens.Interval = 3000; // 3 segundos
            timerMensagens.Tick += TimerMensagens_Tick;
            timerMensagens.Start();
            nomeCliente = nome;
        }
        private void TimerMensagens_Tick(object sender, EventArgs e)
        {
            CarregarMensagensNovas();
        }
        private void CarregarMensagensNovas()
        {
            try
            {
                using (MySqlConnection conn = Conexao.Conectar())
                {
                    string query = @"SELECT m.texto, u.nome, m.data_envio, m.remetente_id
                             FROM mensagens m
                             JOIN usuarios u ON m.remetente_id = u.id
                             WHERE m.solicitacao_id = @solicitacaoId
                               AND m.data_envio > @ultimaData
                             ORDER BY m.data_envio ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@solicitacaoId", solicitacaoId);
                    cmd.Parameters.AddWithValue("@ultimaData", ultimaMensagemRecebida);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string texto = reader.GetString("texto");
                            string autor = reader.GetString("nome");
                            DateTime data = reader.GetDateTime("data_envio");
                            int remetente = reader.GetInt32("remetente_id");

                            bool isCliente = remetente == remetenteId;
                            AdicionarBolha(texto, autor, data, isCliente);

                            // Atualiza a hora da última mensagem recebida
                            if (data > ultimaMensagemRecebida)
                                ultimaMensagemRecebida = data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Evita travar o chat por erro ocasional
                Console.WriteLine("Erro ao carregar mensagens: " + ex.Message);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMensagem.Text)) return;
            string msg = txtMensagem.Text.Trim();

            using (var c = Conexao.Conectar())
            using (var cmd = new MySqlCommand(
                "INSERT INTO chat (solicitacao_id, remetente_id, mensagem) VALUES (@s,@r,@m)", c))
            {
                cmd.Parameters.AddWithValue("@s", solicitacaoId);
                cmd.Parameters.AddWithValue("@r", remetenteId);
                cmd.Parameters.AddWithValue("@m", msg);
                cmd.ExecuteNonQuery();
            }

            txtMensagem.Clear();
            CarregarMensagens();
        }

        private void CarregarMensagens()
        {
            pnlMensagens.Controls.Clear();

            using (var c = Conexao.Conectar())
            using (var cmd = new MySqlCommand(@"
                SELECT u.id AS remetente, u.nome, c.mensagem, c.data_envio
                  FROM chat c
                  JOIN usuario u ON c.remetente_id = u.id
                 WHERE c.solicitacao_id = @s
                 ORDER BY c.data_envio", c))
            {
                cmd.Parameters.AddWithValue("@s", solicitacaoId);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        bool isCliente = r.GetInt32("remetente") == remetenteId;
                        AdicionarBolha(
                            r.GetString("mensagem"),
                            r.GetString("nome"),
                            r.GetDateTime("data_envio"),
                            isCliente);
                    }
                }
            }
        }

        private void AdicionarBolha(string texto, string autor, DateTime hora, bool isCliente)
        {
            string full = $"[{hora:HH:mm}] {autor}\n{texto}";

            // Label da mensagem
            var lbl = new Label
            {
                Text = full,
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Segoe UI", 10),
                ForeColor = isCliente ? Color.White : Color.Black,
                Padding = new Padding(8),
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };

            // Guna2CustomGradientPanel com gradiente de 4 cores
            var bolha = new Guna.UI2.WinForms.Guna2CustomGradientPanel
            {
                BorderRadius = 12,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MaximumSize = new Size(320, 0),
                Padding = new Padding(5),
                Margin = new Padding(0),
                FillColor = isCliente ? Color.Teal : Color.Gainsboro,
                FillColor2 = isCliente ? Color.DarkCyan : Color.LightGray,
                FillColor3 = isCliente ? Color.MediumTurquoise : Color.Silver,
                FillColor4 = isCliente ? Color.DarkSlateGray : Color.LightSlateGray
            };

            bolha.Controls.Add(lbl);

            // Posição da bolha
            int y = pnlMensagens.Controls.Cast<Control>().Select(c => c.Bottom).DefaultIfEmpty(0).Max() + 8;
            int x = isCliente
                ? pnlMensagens.Width - bolha.PreferredSize.Width - 30
                : 10;

            bolha.Location = new Point(x, y);
            pnlMensagens.Controls.Add(bolha);

            // Corrigir scroll para o fim
            pnlMensagens.VerticalScroll.Value = pnlMensagens.VerticalScroll.Maximum;
            pnlMensagens.PerformLayout();
        }



        private void FormChat_Load(object sender, EventArgs e)
        {
            BemVindo.Text = $"Bem-vindo, {nomeCliente}!";
            int clienteId = SessaoUsuario.UsuarioId;
        
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            string nomeCliente = SessaoUsuario.Nome; // ou outro método que você esteja usando

            // Abre novamente o DashboardCliente
            DashboardCliente dashboard = new DashboardCliente(nomeCliente);
            dashboard.Show();

            // Fecha o chat atual
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }
    }
}

