using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using APPLICATION.Models.ARCHIVE;

namespace APPLICATION.Data
{
    public partial class ARCHIVEContext : DbContext
    {
        public ARCHIVEContext()
        {
        }

        public ARCHIVEContext(DbContextOptions<ARCHIVEContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<APPLICATION.Models.ARCHIVE.TFamille>()
              .HasOne(i => i.TFamille1)
              .WithMany(i => i.TFamilles1)
              .HasForeignKey(i => i.parent)
              .HasPrincipalKey(i => i.id);
            this.OnModelBuilding(builder);
        }

        public DbSet<APPLICATION.Models.ARCHIVE.TDocument> TDocuments { get; set; }

        public DbSet<APPLICATION.Models.ARCHIVE.TFamille> TFamilles { get; set; }

        public DbSet<APPLICATION.Models.ARCHIVE.TSociete> TSocietes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}