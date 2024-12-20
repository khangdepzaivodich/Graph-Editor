﻿using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Editor
{
    class AStar
    {
        public static void Algorithm(int n, int start, int end, List<List<Guna2CircleButton>> adjList, List<Guna2CircleButton> nodes, Dictionary<(int, int, Color), int> edges, Color defaultColor, Color visColor, Color underVisColor)
        {   
            int[] g = new int[n]; 
            double[] f = new double[n]; 
            double[] h = new double[n]; 
            bool[] vis = new bool[n]; 
            int[] save = new int[n];
            Array.Fill(g, int.MaxValue); 
            Array.Fill(f, int.MaxValue);
            g[start] = 0;
            f[start] = 0;
            nodes[start].FillColor = Color.Red;
            nodes[end].FillColor = Color.Green;
            for(int i = 0; i < n; ++i)
            {
                h[i] = Dist(nodes[i], nodes[end]);
            }
            save[start] = -1;
            var pq = new PriorityQueue<int, double>();
            pq.Enqueue(start, f[start]);
            while(pq.Count > 0)
            {
                int node = pq.Dequeue();
                if(node == end) break;
                vis[node] = true;
                foreach(Guna2CircleButton button in adjList[node])
                {
                    int neighbor = int.Parse(button.Text);
                    if (vis[neighbor])
                    {
                        continue;
                    }
                    int weight;
                    if(edges.ContainsKey((node, neighbor, defaultColor)))
                    {
                        weight = edges[(node, neighbor, defaultColor)];
                    }
                    else
                    {
                        weight = edges[(neighbor, node, defaultColor)];
                    }
                    int dist = weight + g[node];
                    if(dist < g[neighbor])
                    {
                        save[neighbor] = node;
                        g[neighbor] = dist;
                        f[neighbor] = dist + h[neighbor];
                        if (edges.ContainsKey((node, neighbor, defaultColor)))
                        {
                            edges.Remove((node, neighbor, defaultColor));   
                        }
                        else
                        {
                            edges.Remove((neighbor, node, defaultColor));
                        }
                        edges[(node,neighbor,visColor)] = weight;
                        pq.Enqueue(neighbor, f[neighbor]);
                    }
                }
            }
        }
        private static double Dist(Guna2CircleButton node1, Guna2CircleButton node2)
        {
            double x1 = node1.Location.X;
            double y1 = node1.Location.Y;
            double x2 = node2.Location.X;
            double y2 = node2.Location.Y;
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

    }
}