using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JsonContentReader_Lib;

namespace Read_Json_Content
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JSON Files|*.json|All Files|*.*";
            openFileDialog1.Title = "Select an JSON File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                // Clear existing content in RichTextBox and TreeView
                richTextBox1.Clear();
                richTextBox1.Text = File.ReadAllText(filePath);


            }
        }

        JObject jObject = new JObject();


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            if (jObject.CheckJsonFormat(jObject))
            {
                jObject = JObject.Parse(richTextBox1.Text);
                var d = jObject;
                PopulateTreeView(jObject, treeView1.Nodes);

                richTextBox1.Invoke(new Action(() =>
                {
                    JsonHighlighter.HighlightJson(richTextBox1, richTextBox1.Text);
                }));
            }
            else
            {
                treeView1.Nodes.Add("Invalid JSON Format");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            jObject = JObject.Parse(richTextBox1.Text);
            PopulateTreeView(jObject, treeView1.Nodes);
            JsonHighlighter.HighlightJson(richTextBox1, richTextBox1.Text);

        }


        void PopulateTreeView(JObject jObject, TreeNodeCollection nodes)
        {
            foreach (var property in jObject.Properties())
            {
                var node = new TreeNode(property.Key);
                node.Tag = property.Value;
                nodes.Add(node);

                if (property.Value is JObject nestedObject)
                {
                    PopulateTreeView(nestedObject, node.Nodes);
                }
                else if (property.Value is JArray array)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        var item = array[i];
                        var itemNode = new TreeNode(property.Key + $" [{i}]");
                        itemNode.Tag = item;
                        node.Nodes.Add(itemNode);

                        if (item is JObject arrayObject)
                        {
                            PopulateTreeView(arrayObject, itemNode.Nodes);
                        }
                        else if (item is JArray nestedArray)
                        {
                            PopulateArray(nestedArray, itemNode.Nodes, property.Key);
                        }
                    }
                }
            }
        }

        void PopulateArray(JArray array, TreeNodeCollection nodes, string key=null)
        {
            for (int i = 0; i < array.Count; i++)
            {
                var item = array[i];
                var itemNode = new TreeNode(key+ $" [{i}]");
                itemNode.Tag = item;
                nodes.Add(itemNode);

                if (item is JObject nestedObject)
                {
                    PopulateTreeView(nestedObject, itemNode.Nodes);
                }
                else if (item is JArray nestedArray)
                {
                    PopulateArray(nestedArray, itemNode.Nodes);
                }
            }
        }


        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            if (buttonOpenFile != null)
                buttonOpenFile.Location = new Point((this.ClientSize.Width - buttonOpenFile.Width) / 2,
                                                this.ClientSize.Height - buttonOpenFile.Height - 3);
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            richTextBox2.Text = e.Node.Tag.ToString();

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                var a= new FormString(JObject.Deserialize(jObject));
                a.ShowDialog();
            }    
        }
    }
}
