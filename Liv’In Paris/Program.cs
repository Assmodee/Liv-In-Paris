namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<int>> adjacenceMatrix = new List<List<int>>();
            List<List<int>> incidenceMatrix = new List<List<int>>();
            List<Node> nodes = new List<Node>();
            FillAdjacenceMatrix(adjacenceMatrix);
            
            for (int i = 0; i < adjacenceMatrix.Count; i++)
            {
                List<int> incidenceList = new List<int>();
                for (int j = 0; j < adjacenceMatrix[i].Count; j++)
                {
                    if (adjacenceMatrix[i][j] == 1)
                    {
                        incidenceList.Add(j+1);
                    }
                }
                incidenceMatrix.Add(incidenceList);
                nodes.Add(new Node(i + 1, incidenceList));
            }
        }


        static void FillAdjacenceMatrix(List<List<int>> adjacenceMatrix)
        {
            string content = File.ReadAllText("soc-karate.mtx");
            string[] lines = content.Split('\n');
            int nodesCount = 0;

            for (int i = 24; i < lines.Length && lines[i].Length > 0; i++)
            {
                int node1 = 0;
                int node2 = 0;
                int temp;
                int j = 0;
                while (j < lines[i].Length && int.TryParse(lines[i][j].ToString(), out temp))
                {
                    node1 = node1 * 10 + temp;
                    j++;
                }
                j++;
                while (j < lines[i].Length && int.TryParse(lines[i][j].ToString(), out temp))
                {
                    node2 = node2 * 10 + temp;
                    j++;
                }
                if (Math.Max(node1, node2) > nodesCount)
                {
                    for (int k = 0; k < Math.Max(node1, node2); k++)
                    {
                        if (k < nodesCount)
                        {
                            for (int l = 0; l < Math.Max(node1, node2) - nodesCount; l++)
                            {
                                adjacenceMatrix[k].Add(0);
                            }
                        }
                        else
                        {
                            adjacenceMatrix.Add(new List<int>());
                            for (int l = 0; l < Math.Max(node1, node2); l++)
                            {
                                adjacenceMatrix[k].Add(0);
                            }
                        }
                    }
                    nodesCount = Math.Max(node1, node2);
                }
                adjacenceMatrix[node1-1][node2-1] = 1;
                adjacenceMatrix[node2-1][node1-1] = 1;
            }
        }


        static void DisplayMatrix(List<List<int>> matrix)
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}