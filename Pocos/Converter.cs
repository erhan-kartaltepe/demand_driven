using System;
using System.Collections.Generic;
using System.Linq;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Converter {
        public const string CYCLIC_GRAPH_ERROR = "The graph must be acyclic";

        public IList<Entry> Entries {get; protected set;} = new List<Entry>();

        public bool IsValid 
        {
            get {
                var edges = Entries.Select(x => new Tuple<string, string>(x.ParentName, x.ChildName)).ToList();
                Graph<string> graph = new Graph<string>(edges);
                return !graph.IsCyclic;
            }
        }

        /// <summary>
        /// Constructs and returns the file in the native format
        /// </summary>
        /// <returns>The file as a string</returns>
        public abstract string File {get;}
    }
}