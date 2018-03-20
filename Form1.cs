using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

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
            //sortedNode.Reverse();
            foreach (var x in sortedNode) richTextBox1.AppendText(ts.nodename[int.Parse(x.ToString())] + "\n");

            for (i=0; i<ts.timestamp.Length; i++)
            {
                richTextBox1.AppendText(ts.timestamp[i]+"\n");
            }

            string[] msg = ts.message.ToArray();
            for (i=0; i<msg.Length; i++)
            {
                richTextBox1.AppendText(msg[i]);
            }

            ts.status = true;
            next_button.Enabled = true;
            play_button.Enabled = true;
            pictureBox1.Image = ts.img[0];
        }

        private void button_bfs_Click(object sender, EventArgs e)
        {
            ts = new TSort(filename);
            ArrayList sortedNode = new ArrayList();
            ts.img.Add(Graphviz.RenderImage(Utility.format2(ts.adjMatx, sortedNode, ts.nodename), "jpg"));
            sortedNode = ts.TopSortBFS(sortedNode);
            string[] msg = ts.message.ToArray();
            for (int i=0; i<msg.Length; i++)
            {
                richTextBox1.AppendText(msg[i]);
            }
            foreach (var x in sortedNode) richTextBox1.AppendText(ts.nodename[int.Parse(x.ToString())] + "\n");
            ts.status = true;
            next_button.Enabled = true;
            play_button.Enabled = true;
            pictureBox1.Image = ts.img[0];
        }

        private void button_open_Click(object sender, EventArgs e)
        {            
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            filename = openFileDialog1.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
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
    }
}
