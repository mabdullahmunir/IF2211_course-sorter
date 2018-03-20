using System;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace coursesorter
{
    public partial class Form1 : Form
    {
        private string filename;
        private TSort ts;
        private int idx;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_dfs_Click(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                treeView1.Nodes.Clear();
                treeView1.Refresh();
                richTextBox1.AppendText("Starting DFS Algorithm\n");
                ts = new TSort(filename);
                ArrayList sortedNode = new ArrayList();
                ts.img.Add(Graphviz.RenderImage(Utility.format1(ts.adjMatx, ts.timestamp, ts.nodename), "jpg"));
                int i = 0, j = 0;
                bool found = false;
                while (j < ts.adjMatx.GetLength(1))
                {
                    if (sortedNode.Contains(j)) { ++j; continue; }
                    found = false;
                    while (!found && i < ts.adjMatx.GetLength(0))
                    {
                        if (ts.adjMatx[i, j]) found = true;
                        ++i;
                    }
                    if (found) { ++j; continue; }
                    // Search the damn thing, starting from node j
                    sortedNode = ts.TopSortDFS(j, sortedNode);
                    ++j;
                }

                for (i = 0; i < ts.message.ToArray().Length; i++)
                {
                    pictureBox1.Image = ts.img[i + 1];
                    pictureBox1.Refresh();
                    richTextBox1.AppendText(ts.message[i]);
                    richTextBox1.Refresh();
                    Thread.Sleep(500);
                }
                
                ts.status = true;
                next_button.Enabled = true;
                play_button.Enabled = true;
                pictureBox1.Image = ts.img[0];
                richTextBox1.AppendText("Done!!\n");

                List<List<int>> tes = ts.SemesterSort(sortedNode);
                for (i=0; i<tes.ToArray().Length; i++)
                {
                    TreeNode[] listcourse = new TreeNode[tes[i].ToArray().Length];
                    for (j=0; j<tes[i].ToArray().Length; j++)
                    {
                        listcourse[j] = new TreeNode(ts.nodename[tes[i][j]]);
                    }
                    TreeNode sem = new TreeNode("Semester " + (i + 1).ToString(), listcourse);
                    treeView1.Nodes.Add(sem);
                }

            }
            else
                richTextBox1.AppendText("Error : File not Found!!\n");
        }

        private void button_bfs_Click(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                treeView1.Nodes.Clear();
                treeView1.Refresh();
                richTextBox1.AppendText("Starting BFS Algorithm\n");
                ts = new TSort(filename);
                ArrayList sortedNode = new ArrayList();
                ts.img.Add(Graphviz.RenderImage(Utility.format2(ts.adjMatx, sortedNode, ts.nodename), "jpg"));
                sortedNode = ts.TopSortBFS(sortedNode);

                for (int i = 0; i < ts.message.ToArray().Length; i++)
                {
                    pictureBox1.Image = ts.img[i + 1];
                    pictureBox1.Refresh();
                    richTextBox1.AppendText(ts.message[i]);
                    richTextBox1.Refresh();
                    Thread.Sleep(500);
                }

                ts.status = true;
                next_button.Enabled = true;
                play_button.Enabled = true;
                pictureBox1.Image = ts.img[0];
                richTextBox1.AppendText("Done!!\n");

                List<List<int>> tes = ts.SemesterSort(sortedNode);
                for (int i = 0; i < tes.ToArray().Length; i++)
                {
                    TreeNode[] listcourse = new TreeNode[tes[i].ToArray().Length];
                    for (int j = 0; j < tes[i].ToArray().Length; j++)
                    {
                        listcourse[j] = new TreeNode(ts.nodename[tes[i][j]]);
                    }
                    TreeNode sem = new TreeNode("Semester " + (i + 1).ToString(), listcourse);
                    treeView1.Nodes.Add(sem);
                }
            }
            else
                richTextBox1.AppendText("Error : File not Found!!\n");
        }

        private void button_open_Click(object sender, EventArgs e)
        {            
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            filename = openFileDialog1.FileName;
            label4.Text = filename;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ts.status)
            {
                next_button.PerformClick();
            }
        }

        private void play_button_Click(object sender, EventArgs e)
        {
            idx = 0;
            pictureBox1.Image = ts.img[0];
            next_button.Enabled = true;
            prev_button.Enabled = false;
            timer1.Enabled = true;
        }

        private void prev_button_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = ts.img[--idx];
            if (idx == 0)
            {
                prev_button.Enabled = false;
            }
            else
            {
                next_button.Enabled = true;
            }
        }

        private void next_button_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = ts.img[++idx];
            if (idx == ts.img.ToArray().Length-1)
            {
                timer1.Enabled = false;
                next_button.Enabled = false;
            }
            else
            {
                prev_button.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Size s = Form1.ActiveForm.Size;
            if (button1.Text.Equals("<<"))
            {
                s.Width = 361;
                Form1.ActiveForm.Size = s;
                button1.Text = ">>";
            }
            else if (button1.Text.Equals(">>"))
            {
                s.Width = 781;
                Form1.ActiveForm.Size = s;
                button1.Text = "<<";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
