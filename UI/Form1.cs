using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var produto = new Produto();
            produto.IdProd = txtId.Text;
            produto.Nome = txtNome.Text;
            produto.Descricao = txtDescricao.Text;
            produto.Salvar();
            MessageBox.Show("Produto cadastrado com sucesso!");
        }
    }
}
