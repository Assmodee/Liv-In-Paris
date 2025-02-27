using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Liv_In_Paris
{
    internal class Drawing
    {
        /// <summary>
        /// cette methode permet de generer et sauvgarder une image de graphe
        /// </summary>
        /// <param name="adjacencyMatrix"></param>
        /// <param name="degrees"></param>
        /// <param name="filename"></param>
        public static void DrawGraph(List<List<int>> adjacenceMatrix, int[] degrees, string filename)
        {
            ///On déclare simplement les variables (nombre noeuds, taille d'image, rayon des cercles etc)
            int nodeCount = adjacenceMatrix.Count;
            int width = 500, height = 500;
            int radius = 20;
            int centerX = width / 2, centerY = height / 2;
            int graphRadius = 180;

            ///pointF saufgarde les coordonnées de chaque point
            PointF[] positions = new PointF[nodeCount];

            ///On calcule la position des sommets
            for (int i = 0; i < nodeCount; i++)
            {
                /// On met chaque point dans un point d'un circle imaginaire
                /// ///Chaque point est mis a un angle mis en bas
                double angle = i * 2 * Math.PI / nodeCount;
                /// on calcule la position avec des coordonnées polaires
                positions[i] = new PointF(
                    centerX + (float)(graphRadius * Math.Cos(angle)),
                    centerY + (float)(graphRadius * Math.Sin(angle))
                );
            }

            ///Céation de l'image
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                ///smoothing mode ameliore la qualité du dessin
                g.SmoothingMode = SmoothingMode.AntiAlias;
                ///on remplit l'image de blanc
                g.Clear(Color.White);

                ///on dessine les arêtes

                ///on créu un stylo pour dessiner
                Pen edgePen = new Pen(Color.Black, 2);

                /// on parcourt la mat pour voir les sommets connectés et dessiner la ligne les reliant
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = i + 1; j < nodeCount; j++)
                    {
                        if (adjacenceMatrix[i][j] == 1)
                        {
                            g.DrawLine(edgePen, positions[i], positions[j]);
                        }
                    }
                }

                ///On dessine les sommets
                
                for (int i = 0; i < nodeCount; i++)
                {
                    ///calcul de la position du sommet exacte
                    float x = positions[i].X - radius / 2;
                    float y = positions[i].Y - radius / 2;
                    ///dessin du cercle bleu de contour noir
                    g.FillEllipse(Brushes.LightBlue, x, y, radius, radius);
                    g.DrawEllipse(Pens.Black, x, y, radius, radius);
                    ///pour faciliter la lecture on ajoute des labeels à côté des sommet
                    g.DrawString(i.ToString(), new Font("Arial", 10), Brushes.Black, x + 5, y + 3);
                    g.DrawString($"d={degrees[i]}", new Font("Arial", 8), Brushes.Red, x + 15, y - 15);
                }
                /// on saufgarde l'image
                bitmap.Save(filename);

                ///on affiche l'image
                Process.Start(new ProcessStartInfo()
                {
                    FileName = filename,
                    UseShellExecute = true
                });
                Console.WriteLine($"Image enregistrée sous : {Path.GetFullPath(filename)}");

            }

        }



    }
}
