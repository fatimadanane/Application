using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPLICATION.Models.ARCHIVE
{
    [Table("T_Societe", Schema = "dbo")]
    public partial class TSociete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string intitule { get; set; }

        public string adresse { get; set; }

        public string telephone { get; set; }

        public string fax { get; set; }

        public string email { get; set; }

        public string site_web { get; set; }

        public string rc { get; set; }

        public string idf { get; set; }

        public string cnss { get; set; }

        public string patente { get; set; }

        public string ice { get; set; }

    }
}