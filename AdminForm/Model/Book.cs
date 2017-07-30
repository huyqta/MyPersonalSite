namespace AdminForm.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Author { get; set; }

        [StringLength(500)]
        public string Publisher { get; set; }

        public string Description { get; set; }

        [StringLength(500)]
        public string UrlRead { get; set; }

        [StringLength(500)]
        public string UrlDownload { get; set; }

        [StringLength(500)]
        public string UrlImage { get; set; }

        [StringLength(500)]
        public string UrlThumbnail { get; set; }

        public string Category { get; set; }
    }
}
