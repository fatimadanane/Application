using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPLICATION.Models.ARCHIVE
{
    [Table("Famille", Schema = "dbo")]
    public partial class Famille
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string intitule { get; set; }

        public int? Niveau { get; set; }//it could be 0 for family1 or 1 family2 or 2 for family3 it is the level of the hierarchy

        public int? Parent { get; set; }//id of the parent

    }
}