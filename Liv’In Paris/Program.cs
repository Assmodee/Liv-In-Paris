namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] adjacenceMatrix = new int[34,34];
            List<Node> nodes = new List<Node>();
            List<Link> links = new List<Link>();
            GetFileContent(adjacenceMatrix);

            for (int i = 0; i < 34; i++)
            {
                nodes.Add(new Node(i+1));
            }

            for (int i = 0; i < 34; i++)
            {
                for (int j = 0; j < 34; j++)
                {
                    links.Add(new Link(nodes[i], nodes[j]));
                }
            }

            Graph g = new Graph(nodes, links, adjacenceMatrix);
            g.DepthFirstSearch(new List<int>());
        }


        static void GetFileContent(int[,] matrix)
        {
            string content = File.ReadAllText("soc-karate.mtx");
            string[] lines = content.Split('\n');
            bool metaData = false;

            foreach (string line in lines)
            {
                if (!metaData && line.Length > 0 && line[0] != '%')
                {
                    metaData = true;
                }
                else if (line.Length > 0 && line[0] != '%')
                {
                    string rawValue = "";
                    int nodeValue = 0;
                    foreach (char c in line)
                    {
                        if (c != ' ')
                        {
                            rawValue += c;
                        }
                        else
                        {
                            nodeValue = int.Parse(rawValue);
                            rawValue = "";
                        }
                    }
                    if (rawValue != "")
                    {
                        matrix[nodeValue - 1, int.Parse(rawValue) - 1] = 1;
                        matrix[int.Parse(rawValue) - 1, nodeValue - 1] = 1;
                    }
                }
            }
        }
    }
}