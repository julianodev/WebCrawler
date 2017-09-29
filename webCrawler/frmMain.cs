using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webCrawler
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCapturarPosts_Click(object sender, EventArgs e)
        {
            var articles = new Crawler();
            
            var sw = new Stopwatch();
            sw.Start();
            loadingPosts.Visible = true;
            dataGridView1.DataSource = articles.LoadPosts();
            label1.Text = $"Quantidade de posts : {articles.LoadPosts().Count}";
            loadingPosts.Visible = false;
            label1.Visible = true;
            sw.Stop();
            MessageBox.Show($"Artigos recuperados com sucesso","Crawler", MessageBoxButtons.OK);
        }
    }
}
