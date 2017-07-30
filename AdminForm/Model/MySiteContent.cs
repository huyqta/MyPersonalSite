namespace AdminForm.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MySiteContent : DbContext
    {
        public MySiteContent()
            : base("name=MySiteContent")
        {
        }

        public virtual DbSet<Book> Books { get; set; }

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
