using System;
using System.Collections.Generic;
using System.Linq;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// Stores graph data and determines if it contains a cycle or not
    /// </summary>
    /// <typeparam name="T">The generic type to store</typeparam>
    public class Graph<T>
    {
        private List<List<int>> adjacencyList;
        private Dictionary<T, int> nodes = new Dictionary<T, int>();

        /// <summary>
        /// Constructs a Graph instance with a number of nodes
        /// </summary>
        /// <param name="numberOfNodes">The number of nodes to add</param>
        public Graph(int numberOfNodes)
        {
            Initialize(numberOfNodes);
        }

        /// <summary>
        /// Constructs a Graph instance with a list of edges
        /// </summary>
        /// <param name="edges">The edges as a tuple of {from,to} nodes</param>
        public Graph(List<Tuple<T, T>> edges) {
            int count = edges.Select(x => x.Item1).Union(edges.Select(x => x.Item2)).Distinct().Count();
            Initialize(count);
            foreach (var edge in edges) {
                AddEdge(edge.Item1, edge.Item2);
            }
        }

        /// <summary>
        /// Add a directed edge to the graph from first to second
        /// </summary>
        /// <param name="first">The "from" node</param>
        /// <param name="second">The "to" node</param>
        public void AddEdge(T first, T second)
        {
            if (!nodes.ContainsKey(first))
            {
                nodes.Add(first, nodes.Count);
            }

            if (!nodes.ContainsKey(second))
            {
                nodes.Add(second, nodes.Count);
            }

            adjacencyList[nodes[first]].Add(nodes[second]);
        }

        /// <summary>
        /// Initializes an empty graph with a number of potential nodes
        /// </summary>
        /// <param name="numberOfNodes">The number of nodes</param>
        private void Initialize(int numberOfNodes) {
            adjacencyList = new List<List<int>>(numberOfNodes);
            for (int i = 0; i < adjacencyList.Capacity; i++)
            {
                adjacencyList.Add(new List<int>());
            }
        }

        /// <summary>
        /// Performs a depth-first search of a given node to detect a cycle
        /// </summary>
        /// <param name="nodeNum">The ith node to examine</param>
        /// <param name="hasVisited">Whether we have visited a given node already</param>
        /// <param name="nodeStack">The nodes to visit</param>
        /// <returns>Whether a cycle was found</returns>
        private bool IsCyclicUtil(int nodeNum, List<bool> hasVisited, List<bool> nodeStack)
        {
            if (hasVisited[nodeNum] == false)
            {
                hasVisited[nodeNum] = true;
                nodeStack[nodeNum] = true;

                foreach (var a in adjacencyList[nodeNum])
                {
                    if (!hasVisited[a] && IsCyclicUtil(a, hasVisited, nodeStack))
                    {
                        return true;
                    }
                    else if (nodeStack[a])
                    {
                        return true;
                    }
                }

            }
            nodeStack[nodeNum] = false;
            return false;
        }

        /// <summary>
        /// Returns true if the graph contains a cycle
        /// </summary>
        public bool IsCyclic
        {
            get
            {
                // Mark all the vertices as not visited and not part of recursion
                // stack
                List<bool> hasVisited = new List<bool>(adjacencyList.Count);
                List<bool> nodeStack = new List<bool>(adjacencyList.Count);
                for (int i = 0; i < adjacencyList.Count; i++)
                {
                    hasVisited.Add(false);
                    nodeStack.Add(false);
                }

                // Call the recursive helper function to detect cycle in different
                // DFS trees
                for (int i = 0; i < adjacencyList.Count; i++)
                {
                    if (IsCyclicUtil(i, hasVisited, nodeStack))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}