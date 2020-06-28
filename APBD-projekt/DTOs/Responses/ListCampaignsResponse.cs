using APBD_projekt.Models;
using System;
using System.Collections.Generic;

namespace APBD_projekt.DTOs.Responses
{
    public class ListCampaignsResponse
    {
        public int IdCampaign { get; set; }
        public int IdClient { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerSquareMeter { get; set; }
        public int FromIdBuilding { get; set; }
        public int ToIdBuilding { get; set; }
        public ClientResponse Client { get; set; }
        public List<Banner> Banners { get; set; }
    }
}