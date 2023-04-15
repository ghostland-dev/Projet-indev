using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alimbio.Server.Models.alimbio_master_db
{
    [Table("site")]
    public partial class Site
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }

        [Required]
        public string siret { get; set; }

        [Required]
        public string nom_site { get; set; }

        [Required]
        public string type_site { get; set; }

        [Required]
        public string statut { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public int numbre_employes { get; set; }

        [Required]
        public double capital { get; set; }

        [Required]
        public string adresse { get; set; }

        [Required]
        public string ville { get; set; }

        [Required]
        public string langitude { get; set; }

        [Required]
        public string latittude { get; set; }

        [Required]
        public int effectif { get; set; }

        [Required]
        public DateTime date_creation { get; set; }

        [Required]
        public string tel { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string site_internet { get; set; }

        [Required]
        public ulong entreprise_id { get; set; }

        [Required]
        public ulong ville_id { get; set; }

        public ICollection<Salary> Salaries { get; set; }

        public Entreprise Entreprise { get; set; }

    }
}