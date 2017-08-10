namespace AdminForm.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        public Guid Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public string Description { get; set; }
    }
}
