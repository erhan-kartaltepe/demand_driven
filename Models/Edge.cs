using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemandDriven.Models
{
    public partial class Edge
    {
        public int Id { get; set; }
        [Required]
        public int ParentNodeId { get; set; }
        [Required]
        public int ChildNodeId { get; set; }
        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }

        public virtual Node ChildNode { get; set; }
        public virtual Node ParentNode { get; set; }
    }
}
