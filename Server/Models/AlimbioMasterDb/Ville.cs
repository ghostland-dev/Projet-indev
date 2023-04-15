using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alimbio.Server.Models.alimbio_master_db
{
    [Table("villes")]
    public partial class Ville
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }

        [Required]
        public string nom_ville { get; set; }

        [Required]
        public long code_postal { get; set; }

        public ICollection<Site> Sites { get; set; }

    }
}