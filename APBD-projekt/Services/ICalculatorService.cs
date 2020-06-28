using APBD_projekt.Models;
using System.Collections.Generic;

namespace APBD_projekt.Services
{
    public interface ICalculatorService
    {
        public decimal[] CalculateMinBanerArea(List<Building> buildings, Building fromBuilding, Building toBuilding);
    }
}
