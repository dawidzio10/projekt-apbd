using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_projekt.Models
{
    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBuilding { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        public decimal Height { get; set; }
        public virtual ICollection<Campaign> CampaignsFromId { get; set; }
        public virtual ICollection<Campaign> CampaignsToId { get; set; }
    }
}