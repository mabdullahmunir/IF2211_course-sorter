using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace coursesorter
{
    class TSort
    {
        public List<string> message
        {
            get; set;
        }
        public List<Image> img
        {
            get; set;
        }
        public int c
        {
            get; set;
        }
        public string[] timestamp
        {
            get; set;
        }
        public string[] nodename
        {
            get; set;
        }
        public bool[,] adjMatx
        {
            get; set;
        }
        public bool status;

        public TSort(string filename)
        {
            img = new List<Image>();
            message = new List<string>();
            readFile(filename);
            c = 0;
            timestamp = new string[nodename.Length];
            status = false;
        }

        public void readFile(string filename)
        {
            string[] ls;
            ls = File.ReadAllLines(filename);

            //Take node name and formatting input
            nodename = new string[ls.Length];
            for (int i = 0; i < ls.Length; i++)
            {
                string now = ls.GetValue(i).ToString();
                ls.SetValue(Utility.RemoveWhiteSpace(now.Remove(now.Length - 1)), i);
                nodename.SetValue(ls[i].Split(',').GetValue(0).ToString(), i);
            }

            //initialization Adjacency Matrix with false
            adjMatx = new bool[nodename.Length, nodename.Length];
            for (int i = 0; i < nodename.Length; i++)
            {
                for (int j = 0; j < nodename.Length; j++)
                {
                    adjMatx[i, j] = false;
                }
            }

            //fill Adjacency Matrix
            for (int i = 0; i < ls.Length; i++)
            {
                string[] splitted = ls.GetValue(i).ToString().Split(',');
                for (int j = 1; j < splitted.Length; j++)
                {
                    adjMatx[Utility.findarray(nodename, splitted[j]), i] = true;
                }
            }
        }

        // Using adjacency matrix, a 2 dimensional array of boolean.
        // Second index is where the arrow is pointing to
        public ArrayList TopSortDFS(int x, ArrayList sortedNode)
        {
            c++;
            timestamp[x] += c.ToString() + " ";
            img.Add(Graphviz.RenderImage(Utility.format1(adjMatx, timestamp, nodename), "jpg"));
            // For every child, reccursively call the algorithm, if the child hasn't been sorted
            for (int j = 0; j < adjMatx.GetLength(1); ++j)
            {
                if (adjMatx[x, j] == true && !sortedNode.Contains(j))
                {
                    sortedNode = TopSortDFS(j, sortedNode);
                }
            }
            c++;
            timestamp[x] += c.ToString() + " ";
            img.Add(Graphviz.RenderImage(Utility.format1(adjMatx, timestamp, nodename), "jpg"));
            // Insert it to the beginning of the list
            sortedNode.Insert(0, x);
            return sortedNode;
        }


        // Using adjacency matrix, a 2 dimensional array of boolean.
        // Second index is where the arrow is pointing to
        public ArrayList TopSortBFS(ArrayList sortedNode)
        {
            if (sortedNode.ToArray().GetLength(0) == adjMatx.GetLength(0) - 1)
            {
                int x;
                for (x = 0; x < adjMatx.GetLength(0); ++x)
                {
                    if (!sortedNode.Contains(x)) break;
                }
                sortedNode.Add(x);
                message.Add("Remove " + nodename[x] + " from list\n");
                img.Add(Graphviz.RenderImage(Utility.format2(adjMatx, sortedNode, nodename), "jpg"));
                return sortedNode;
            }
            // Search a node that doesn't have an arrow pointing into them 
            int i = 0, j = 0;
            bool found = false;
            while (j < adjMatx.GetLength(1))
            {
                if (sortedNode.Contains(j)) { ++j; continue; }
                found = false;
                while (!found && i < adjMatx.GetLength(0))
                {
                    if (sortedNode.Contains(i)) { ++i; continue; }
                    if (adjMatx[i, j]) found = true;
                    ++i;
                }
                if (found) { ++j; continue; }
                break;
            }
            // remove the node
            sortedNode.Add(j);
            message.Add("Remove " + nodename[j] + " from list\n");
            img.Add(Graphviz.RenderImage(Utility.format2(adjMatx, sortedNode, nodename), "jpg"));
            // reccursively call
            return TopSortBFS(sortedNode);
        }

    }
}
