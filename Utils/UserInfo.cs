using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Utils
{
    public class UserInfo
    {
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public string IdNumber { get; set; }
        public string Brand { get; set; }
        public string Capacity { get; set; }
        public string VehicleColor { get; set; }
        public string Weight { get; set; }
        public string VIN { get; set; }
        public float Cost { get; set; } = 100f;

        public void FillPasportInfo(string fn, string bd, string id)
        {
            FullName = fn;
            BirthDate = bd;
            IdNumber = id;
        }

        public void FillVehicleInfo(string brand, string capacity, string color, string weight, string vin, float cost)
        {
            Brand = brand;
            Capacity = capacity;
            VehicleColor = color;
            Weight = weight;
            VIN = vin;
            Cost = cost;
        }
    } 
}
