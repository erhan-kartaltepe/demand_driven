using System;
using System.Collections.Generic;

namespace DemandDriven.Models
{
    public partial class Graph
    {
        public Graph()
        {
            Edge = new HashSet<Edge>();
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual ICollection<Edge> Edge { get; set; }
    }
}
