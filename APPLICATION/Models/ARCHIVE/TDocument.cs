using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPLICATION.Models.ARCHIVE
{
    [Table("T_Document", Schema = "dbo")]
    public partial class TDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime? date { get; set; }

        public string Famille_Un { get; set; }

        public string Famille_Deux { get; set; }

        public string Famille_Trois { get; set; }

        public DateTime? Date_Exp { get; set; }

        public DateTime? Date_Alerte { get; set; }

        public string Intitule { get; set; }

    }
}