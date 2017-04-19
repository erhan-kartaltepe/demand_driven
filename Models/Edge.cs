using System;
using System.Collections.Generic;

namespace DemandDriven.Models
{
    public partial class Edge
    {
        public int Id { get; set; }
        public int ParentNodeId { get; set; }
        public int ChildNodeId { get; set; }
        public byte Quantity { get; set; }
        public int GraphId { get; set; }

        public virtual Node ChildNode { get; set; }
        public virtual Graph Graph { get; set; }
        public virtual Node ParentNode { get; set; }
    }
}
