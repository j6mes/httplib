using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpKick.HttpLib.Samples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            new Form2().Show();
        }

        private void UpdateText(String text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtResult.Text = text;
            });
        }

     

        private void btnView_Click(object sender, EventArgs e)
        {
            Http.Get(textBox1.Text).OnSuccess((string a) =>
            {
                UpdateText(a);
            }).Go();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            if(dlg.ShowDialog()==DialogResult.OK)
            {
                Http.Get(textBox1.Text).DownloadTo(dlg.FileName, onProgressChanged: (bytesCopied,totalBytes) => 
                    {
                        UpdateText(bytesCopied.ToString());
                    },
                    onSuccess: (headers) =>
                {
                    UpdateText("Download Complete");
                }).Go();
            }
           
        }

    }
}
