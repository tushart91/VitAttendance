using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace VitAttendance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || dropDown.SelectedItem != null)
            {
                webBrowser1.Visible = false;
                webBrowser1.Navigate("https://academics.vit.ac.in/parent/parent_login.asp");
                textBox1.Text = textBox1.Text.ToUpper();
                webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted5);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
                progressBar1.PerformStep();
                progressBar1.Visible = true;
            }
            else if(dropDown.SelectedItem == null)
                MessageBox.Show("Please Select a Action from the DropDown", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("Enter a Registration Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!webBrowser1.IsBusy)
            {
                webBrowser1.Visible = false;
                progressBar1.PerformStep();
                HtmlElementCollection ele = webBrowser1.Document.GetElementsByTagName("input");
                ele[1].SetAttribute("value", textBox1.Text);
                HtmlElementCollection font;
                font = webBrowser1.Document.GetElementsByTagName("font");
                string captcha = font[5].InnerText;
                captcha = captcha.Trim();
                string temp = captcha.Replace(" ", string.Empty);

                ele[2].SetAttribute("value", temp);

                HtmlElementCollection form = webBrowser1.Document.Forms;
                form[0].InvokeMember("submit");
                progressBar1.PerformStep();
                webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted2);
            }
        }

        private void webBrowser1_DocumentCompleted2(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Visible = false;
            progressBar1.PerformStep();
            if(dropDown.SelectedIndex == 0)
                webBrowser1.Navigate("https://academics.vit.ac.in/parent/attn_report.asp");
            else if (dropDown.SelectedIndex == 1)
                webBrowser1.Navigate("https://academics.vit.ac.in/parent/marks.asp");
            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted2);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted3);
            progressBar1.PerformStep();
            if (dropDown.SelectedIndex == 1)
                progressBar1.Value = 100;
        }

        private void webBrowser1_DocumentCompleted3(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (dropDown.SelectedIndex == 1)
            {
                webBrowser1.Visible = true;
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted3);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted5);
                return;
            }
            webBrowser1.Visible = false;
            progressBar1.PerformStep();
            HtmlElementCollection date = webBrowser1.Document.GetElementsByTagName("input");
            date[0].SetAttribute("value", "02-Jan-2012");
            HtmlElement toDate = webBrowser1.Document.GetElementById("to_date");
            date[1].SetAttribute("value", DateTime.Now.ToString("dd-MMM-yyyy"));
            date[2].InvokeMember("Click");
            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted3);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted4);
            progressBar1.PerformStep();
        }

        private void webBrowser1_DocumentCompleted4(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Visible = true;
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            webBrowser1.Document.Cookie.Remove(0, webBrowser1.Document.Cookie.Length);
            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted4);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted5);
        }

        private void webBrowser1_DocumentCompleted5(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void reset_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = false;
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            textBox1.SelectAll();
            webBrowser1.Document.Cookie.Remove(0, webBrowser1.Document.Cookie.Length);
            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted5);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dropDown.SelectedIndex = 0;
        }
    }
}
