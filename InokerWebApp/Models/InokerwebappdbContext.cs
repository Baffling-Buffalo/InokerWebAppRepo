using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InokerWebApp.Models
{
    public partial class InokerwebappdbContext : DbContext
    {
        public InokerwebappdbContext()
        {
        }

        public InokerwebappdbContext(DbContextOptions<InokerwebappdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<JumboImg> JumboImgs { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InokerWebAppDb;Integrated Security=True");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JumboImg>(entity =>
            {
                entity.Property(e => e.JumboImgId).ValueGeneratedNever();

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.JumboImgs)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JumboImg_Model");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.CollectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Collection");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Model");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
