using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alimbio.Server.Models.alimbio_master_db
{
    [Table("entreprise")]
    public partial class Entreprise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong id { get; set; }

        [Required]
        public string siren { get; set; }

        [Required]
        public string siren_formate { get; set; }

        [Required]
        public string nom_entreprise { get; set; }

        [Required]
        public string description_activite { get; set; }

        public bool? personne_moral { get; set; }

        [Required]
        public string denomenation { get; set; }

        [Required]
        public string siret_siege { get; set; }

        [Required]
        public string siret_formate_siege { get; set; }

        [Required]
        public long nic { get; set; }

        [Required]
        public long numero_voie { get; set; }

        [Required]
        public string libelle_voie { get; set; }

        [Required]
        public string complement_adresse { get; set; }

        [Required]
        public string adresse_complete { get; set; }

        [Required]
        public string code_postal { get; set; }

        [Required]
        public string ville { get; set; }

        [Required]
        public string region { get; set; }

        [Required]
        public string lattitude { get; set; }

        [Required]
        public string langitude { get; set; }

        public DateTime date_creation { get; set; }

        public bool entreprise_enmployeuse { get; set; }

        [Required]
        public double capital { get; set; }

        [Required]
        public int effectif { get; set; }

        [Required]
        public string statut_rcs { get; set; }

        [Required]
        public DateTime inscrit_le { get; set; }

        [Required]
        public string site_internet { get; set; }

        [Required]
        public string immatriculation_tva { get; set; }

    }
}