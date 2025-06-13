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
        public string VIN { get; set; }
        public string RegistrationDate { get; set; }
        public string ReleaseYear { get; set; }
        public string Surname { get; set; }
        public string GivenNames { get; set; }

        public float Cost { get; set; } = 100f;

        public void FillPasportInfo(string fn, string bd, string id)
        {
            FullName = fn;
            BirthDate = bd;
            IdNumber = id;
        }
    } 
}
