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
using System.Xml;
using WcfCore.Helpers;
using WcfCore.Soap;
using WcfWinform.Helpers;

namespace WcfWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var wsdl = await WsdlHelper.Build(textBox1.Text + "?wsdl");

            ClearForm();
            comboBox1.Items.Clear();

            wsdl.Definitions.PortType.Operations.ForEach(o =>
            {
                comboBox1.Items.Add(o.Name);
            });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var wsdl = await WsdlHelper.Build(textBox1.Text + "?wsdl");
            var soapClient = new SoapClient(textBox1.Text, wsdl.Definitions.Binding.OperationsBinding.First(o => o.Name == comboBox1.SelectedItem.ToString()).Action);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(richTextBox1.Text);
            var soapResponse = await soapClient.PostAsync("POST", xmlDocument);
            richTextBox2.Text = XmlFormatParserHelper.GetFormattedXml(soapResponse);
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearForm();
            var soapRequest = await SoapHelper.BuildSoapRequest(textBox1.Text, comboBox1.SelectedItem.ToString());
            richTextBox1.Text = XmlFormatParserHelper.GetFormattedXml(soapRequest);
        }

        private void ClearForm()
        {            
            richTextBox1.Text = string.Empty;
            richTextBox2.Text = string.Empty;
        }
    }
}
