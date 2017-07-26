namespace MySite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Books : DbContext
    {
        public Books()
            : base("name=Books")
        {
        }

        public virtual DbSet<Book> DsBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.UrlRead)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.UrlDownload)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.UrlImage)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.UrlThumbnail)
                .IsUnicode(false);
        }
    }
}
