using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpKick.HttpLib.Samples
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        private void UpdateText(String text)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtResult.Text = text;
            });
        }



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Http.Post(textBox1.Text).Upload(new[] { new NamedFileStream("myfile", dlg.FileName, "application/octet-stream", File.OpenRead(dlg.FileName)) }, new { a = "b", c = "d" }, (bytesSent, totalBytes) =>
                {
                    UpdateText(bytesSent.ToString());
                }, (totalBytes) => { }).OnSuccess((result) => { UpdateText("Completed"); }).Go();
            }
           
        }
    }
}
