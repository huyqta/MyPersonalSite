namespace MySite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Article
    {
        public Guid Id { get; set; }

        public string PostContent { get; set; }

        public string RelatedPost { get; set; }

        public Guid? CategoryId { get; set; }

        public DateTime? PostedDate { get; set; }

        public string Description { get; set; }
    }
}
