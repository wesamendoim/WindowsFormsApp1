using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        XmlDocument DocXml = new XmlDocument();

        private string filePathXml;

        private string fileXml;

        private bool checkFileXml;

        public Form1()
        {
            fileXml = InfoGlobal.filePathProject + @"\DocXml.xml";

            checkFileXml = File.Exists(fileXml);

            if (checkFileXml == false)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = fileDialog.FileName;

                    try
                    {
                        DocXml.Load(filename);

                        File.Copy(filename, fileXml);
                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Arquivo incorreto, selecione o xml correto");


                    }

                }


            }

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //XmlNodeList xmlList;


            TxtHostName.Text = InfoGlobal.HostName; //HostName
            TxtSenha.Text = InfoGlobal.Password; //Password
            TxtLogin.Text = InfoGlobal.Login; //Login

            string password = DocXml.ChildNodes[0].ChildNodes[0].InnerText; //Login
            string hostname = DocXml.ChildNodes[0].ChildNodes[1].InnerText; //Password
            string login = DocXml.ChildNodes[0].ChildNodes[2].InnerText; //HostName

            string passwordecrypt = Encryption.Decrypt(password, @"Valadao");
            string logindecrypt = Encryption.Decrypt(login, @"Valadao");
            string hostnamedecrypt = Encryption.Decrypt(hostname, @"Valadao");

        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Você deseja salvar as informações?", "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DocXml.ChildNodes[1].ChildNodes[0].InnerText = TxtLogin.Text; //Login
                DocXml.ChildNodes[1].ChildNodes[1].InnerText = TxtSenha.Text; //Password
                DocXml.ChildNodes[1].ChildNodes[2].InnerText = TxtHostName.Text; //HostName

                //Criptografar os dados do formulario
                string passwordencrypt = Encryption.Encrypt(TxtSenha.Text, @"Valadao");
                string loginencrypt = Encryption.Encrypt(TxtLogin.Text, @"Valadao");
                string hostnamencrypt = Encryption.Encrypt(TxtHostName.Text, @"Valadao");

                //Salvar no xml o dado criptografado
                DocXml.ChildNodes[1].ChildNodes[0].InnerText = loginencrypt;
                DocXml.ChildNodes[1].ChildNodes[1].InnerText = passwordencrypt;
                DocXml.ChildNodes[1].ChildNodes[2].InnerText = hostnamencrypt;

                //Salvando o arquivo
                DocXml.Save(@"C:\Users\Wesley\Desktop\DocXml.xml");
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}


