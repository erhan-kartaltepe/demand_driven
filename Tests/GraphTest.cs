using System;
using System.Collections.Generic;
using DemandDriven.Pocos;
using Xunit;

namespace DemandDriven.Tests
{
    public class GraphTest
    {
        [Fact]
        public void TestGraph()
        {
            Graph<string> g = new Graph<string>(3);
            g.AddEdge("A", "B");
            g.AddEdge("A", "C");
            g.AddEdge("B", "C");
            Assert.True(!g.IsCyclic);

            g = new Graph<string>(1);
            g.AddEdge("A", "A");
            Assert.True(g.IsCyclic);

            g = new Graph<string>(4);
            g.AddEdge("A", "B");
            g.AddEdge("A", "C");
            g.AddEdge("B", "B");
            g.AddEdge("C", "C");
            g.AddEdge("C", "D");
            g.AddEdge("D", "D");
            Assert.True(g.IsCyclic);

            List<Tuple<string, string>> edges = new List<Tuple<string, string>>();
            edges.Add(new Tuple<string, string>("A", "B"));
            edges.Add(new Tuple<string, string>("B", "C"));
            edges.Add(new Tuple<string, string>("A", "C"));
            g = new Graph<string>(edges);
            Assert.True(!g.IsCyclic);

        }
    }
}