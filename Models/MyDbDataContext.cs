using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemandDriven.Models
{
    public partial class MyDbDataContext : DbContext
    {
        public virtual DbSet<Edge> Edge { get; set; }
        public virtual DbSet<Graph> Graph { get; set; }
        public virtual DbSet<Node> Node { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=xxxx;Database=demand_driven_db;user id=xxxx;password=xxxx");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edge>(entity =>
            {
                entity.HasOne(d => d.ChildNode)
                    .WithMany(p => p.EdgeChildNode)
                    .HasForeignKey(d => d.ChildNodeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ChildNodeId");

                entity.HasOne(d => d.Graph)
                    .WithMany(p => p.Edge)
                    .HasForeignKey(d => d.GraphId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Graph_Id");

                entity.HasOne(d => d.ParentNode)
                    .WithMany(p => p.EdgeParentNode)
                    .HasForeignKey(d => d.ParentNodeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ParentNodeId");
            });

            modelBuilder.Entity<Graph>(entity =>
            {
                entity.Property(e => e.CreatedDateTime).HasDefaultValueSql("sysutcdatetime()");

                entity.Property(e => e.Guid).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<Node>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(3)");
            });
        }
    }
}