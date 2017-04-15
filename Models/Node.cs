using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemandDriven.Models
{
    public partial class Node
    {
        public Node()
        {
            EdgeChildNode = new HashSet<Edge>();
            EdgeParentNode = new HashSet<Edge>();
        }

        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string Name { get; set; }

        public virtual ICollection<Edge> EdgeChildNode { get; set; }
        public virtual ICollection<Edge> EdgeParentNode { get; set; }
    }
}
