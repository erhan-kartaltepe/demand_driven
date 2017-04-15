using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DemandDriven.Models;
using DemandDriven.Pocos;
using Microsoft.EntityFrameworkCore;

namespace DemandDriven.Data
{

    public class Context {

        private const int BUFFER_SIZE = 1000;
        private const string MERGE_STATEMENT = @"MERGE INTO Node as target
                    USING (VALUES {0}) AS source (Name) on target.Name = source.Name
                    WHEN NOT MATCHED BY TARGET THEN
                    INSERT (Name) VALUES (name);";

        private const string INSERT_EDGES_STATEMENT = @"INSERT INTO Edge (ParentId, ChildId, Quantity)
                    USING (VALUES {0}) AS source (Name) on target.Name = source.Name
                    WHEN NOT MATCHED BY TARGET THEN
                    INSERT (Name) VALUES (name);";

        /// <summary>
        /// Adds only nodes that aren't already in the database;
        /// Implemented as a merge statement for efficiency
        /// </summary>
        /// <param name="names">The names to add</param>
        public void UpsertNodeNames(IList<string> names) {
                    Console.WriteLine("----" + names.Count);

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
        public void InsertEdges(IList<Entry> entries) {
            using (var context = new MyDbDataContext()) {

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
                        context.Set<Edge>().Add(edge);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}