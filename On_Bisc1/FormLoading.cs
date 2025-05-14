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

namespace On_Bisc1
{
    public partial class FormLoading : Form
    {
        public FormLoading()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
        }

        private void FormLoading_Load(object sender, EventArgs e)
        {
            // Define a imagem de loading (assumindo que o gif foi adicionado com nome 'loading')
            guna2CirclePictureBox1.Image = Properties.Resources.loader;
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
