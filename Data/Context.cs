using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemandDriven.Models;
using DemandDriven.Pocos;
using Microsoft.EntityFrameworkCore;

namespace DemandDriven.Data
{

    /// <summary>
    /// Representation of the Data Context
    /// </summary>
    public class Context {

        private const int BUFFER_SIZE = 1000;
        private const string MERGE_STATEMENT = @"MERGE INTO Node as target
                    USING (VALUES {0}) AS source (Name) on target.Name = source.Name
                    WHEN NOT MATCHED BY TARGET THEN
                    INSERT (Name) VALUES (name);";

        /// <summary>
        /// Adds only nodes that aren't already in the database;
        /// Implemented as a merge statement for efficiency
        /// </summary>
        /// <param name="names">The names to add</param>
        public void UpsertNodeNames(IList<string> names) {
            if (names == null) {
                throw new ArgumentException("names cannot be null");
            }

            using (var context = new MyDbDataContext()) {

                for (int i = 0; i < names.Count; i+=BUFFER_SIZE) {
                    var buffer = names.Skip(BUFFER_SIZE * i).Take(BUFFER_SIZE + i * BUFFER_SIZE);
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in buffer) {
                        sb.Append(",('");
                        sb.Append(s);
                        sb.Append("')");
                    }

                    string insert = sb.ToString().Substring(1);
                    context.Database.ExecuteSqlCommand(string.Format(MERGE_STATEMENT, insert));
                }
            }
        }

        /// <summary>
        /// Inserts edges into the database
        /// </summary>
        /// <param name="entries">The entries to add to the database</param>
        public string AddGraph(IList<Entry> entries) {
            if (entries == null) {
                throw new ArgumentException("entries cannot be null");
            }

            using (var context = new MyDbDataContext()) {

                Graph graph = new Graph();
                context.Graph.Add(graph);
                context.SaveChanges();

                for (int i = 0; i < entries.Count; i+=BUFFER_SIZE) {
                    var buffer = entries.Skip(BUFFER_SIZE * i).Take(BUFFER_SIZE + i * BUFFER_SIZE);

                    var names = buffer.Select(x => x.ParentName);
                    names = names.Union(buffer.Select(x => x.ChildName));
                    names = names.ToList();

                    // Prefetch less than 2 * BUFFER_SIZE node Ids for inserting once per BUFFER_SIZE entries
                    var nodes = context.Node.Where(x => names.Contains(x.Name)).
                                Select(d => new {d.Name, d.Id}).
                                ToDictionary(d => d.Name, d => d.Id);
                    foreach (var e in buffer) {
                        Edge edge = new Edge();
                        edge.ParentNodeId = nodes[e.ParentName];
                        edge.ChildNodeId = nodes[e.ChildName];
                        edge.Quantity = e.Quantity;
                        edge.GraphId = graph.Id;
                        context.Set<Edge>().Add(edge);
                    }
                    context.SaveChanges();
                    return graph.Guid.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Reconstruct the graph from the data model
        /// </summary>
        /// <param name="guid">The idenifier of the graph</param>
        /// <returns>A list of edges</returns>
        public IList<Entry> GetGraph(string guid) {
            Guid guidOutput;
            bool isValid = Guid.TryParse(guid, out guidOutput);

            if (!isValid) {
                throw new ArgumentException("guid must be a proper GUID");
            }

            using (var context = new MyDbDataContext()) {
                return context.Edge.Where(x => x.Graph.Guid.ToString() == guid)
                .Select(x => new Entry(x.ParentNode.Name, x.ChildNode.Name, x.Quantity)).ToList();
            }
        }
    }
}