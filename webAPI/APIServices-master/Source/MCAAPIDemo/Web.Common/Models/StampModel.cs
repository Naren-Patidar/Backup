using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class StampModel
    {
        public MVCURL Stamp1 { get; set; }
        public MVCURL Stamp2 { get; set; }
        public MVCURL Stamp3 { get; set; }
        public MVCURL Stamp4 { get; set; }
        public MVCURL Stamp5 { get; set; }
        public MVCURL Stamp6 { get; set; }
        public MVCURL Stamp7 { get; set; }
        public MVCURL Stamp8 { get; set; }
        public MVCURL Stamp9 { get; set; }
    }

    public class MVCURL
    {
        public string  Controller { get; set; }
        public string Action { get; set; }
    }
}
