using APBD_projekt.Models;
using System.Collections.Generic;
using System.Linq;

namespace APBD_projekt.Services
{
    public class CalculatorService : ICalculatorService
    {
        public decimal[] CalculateMinBanerArea(List<Building> buildings, Building fromBuilding, Building toBuilding)
        {
            var highest = buildings.First();
            var secondHighest = buildings.ElementAt(1);

            decimal[] bannerSize = new decimal[2];

            if(highest == fromBuilding || highest == toBuilding)
            {
                bannerSize[0] = 1 * highest.Height;
                bannerSize[1] = (buildings.Count - 1) * secondHighest.Height ;
            }
            else if(secondHighest == fromBuilding || secondHighest == toBuilding)
            {
                bannerSize[0] = (buildings.Count - 1) * highest.Height;
                bannerSize[1] = 1 * secondHighest.Height;
            }
            else
            {
                if (highest.StreetNumber > secondHighest.StreetNumber)
                {
                    bannerSize[0] = secondHighest.Height * buildings.Count(p => p.StreetNumber <= secondHighest.StreetNumber);
                    bannerSize[1] = highest.Height * buildings.Count(p => p.StreetNumber > secondHighest.StreetNumber);
                }
                else
                {
                    bannerSize[0] = highest.Height * buildings.Count(p => p.StreetNumber <= highest.StreetNumber);
                    bannerSize[1] = secondHighest.Height * buildings.Count(p => p.StreetNumber > highest.StreetNumber);
                }
            }
            return bannerSize;
        }
    }
}
