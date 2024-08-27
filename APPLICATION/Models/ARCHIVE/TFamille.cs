using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPLICATION.Models.ARCHIVE
{
    [Table("T_Famille", Schema = "dbo")]
    public partial class TFamille
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string intitule { get; set; }

        public int? niveau { get; set; }

        public int? parent { get; set; }

        public TFamille TFamille1 { get; set; }

        public ICollection<TFamille> TFamilles1 { get; set; }

    }
}