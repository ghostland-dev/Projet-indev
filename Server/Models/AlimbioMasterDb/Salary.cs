using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alimbio.Server.Models.alimbio_master_db
{
    [Table("salaries")]
    public partial class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }

        [Required]
        public string nom { get; set; }

        [Required]
        public string prenom { get; set; }

        [Required]
        public string fonction { get; set; }

        [Required]
        public int age { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string mobile { get; set; }

        [Required]
        public string fix { get; set; }

        [Required]
        public string numero_employe { get; set; }

        [Required]
        public string numero_securite_sociale { get; set; }

        [Required]
        public double anciennete { get; set; }

        [Required]
        public DateTime date_recuretement { get; set; }

        [Required]
        public double salaire { get; set; }

        [Required]
        public long congres { get; set; }

        [Required]
        public long grade { get; set; }

        [Required]
        public long date_ouverture { get; set; }

        [Required]
        public ulong service_id { get; set; }

        [Required]
        public ulong site_id { get; set; }

        public Service Service { get; set; }

        public Site Site { get; set; }

    }
}