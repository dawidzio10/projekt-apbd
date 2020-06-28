using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_projekt.Models
{
    public class Campaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCampaign { get; set; }
        public int IdClient { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerSquareMeter { get; set; }
        public int FromIdBuilding { get; set; }
        public int ToIdBuilding { get; set; }
        [ForeignKey("FromIdBuilding")]
        public virtual Building BuildingFromId { get; set; }
        [ForeignKey("ToIdBuilding")]
        public virtual Building BuildingToId { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        public virtual ICollection<Banner> Banners { get; set; }
    }
}