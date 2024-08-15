using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Read_Json_Content
{
    public class FormString:Form
    {
        private Button buttonOpenFile;
        private RichTextBox richTextBox1;
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(334, 224);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.buttonOpenFile.Location = new System.Drawing.Point(283, 242);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(63, 22);
            this.buttonOpenFile.TabIndex = 3;
            this.buttonOpenFile.Text = "OK";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // FormString
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(358, 271);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormString";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }




        #endregion




        public FormString()
        {
            InitializeComponent();
        }

        public FormString(string jsonContent) : base()
        {
            InitializeComponent();
            richTextBox1.Text = jsonContent;
            JsonHighlighter.HighlightJson(richTextBox1, jsonContent);
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
