using System;
using System.ComponentModel.DataAnnotations;

namespace APBD_projekt.DTOs.Requests
{
    public class CreateNewCampaignRequest
    {
        [Required]
        public int IdClient { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal PricePerSquareMeter { get; set; }
        [Required]
        public int FromIdBuilding { get; set; }
        [Required]
        public int ToIdBuilding { get; set; }
    }
}