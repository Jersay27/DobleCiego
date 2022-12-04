using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DobleCiego.Models
{
    public class modelTracking
    {
        public int idTracking { get; set; }
        public string titulo { get; set; }
        public string revisorName { get; set; }
        public string escritorName { get; set; }
        public string docRefEscritor { get; set; }
        public string docRefRevisor { get; set; }
        public DateTime? revisorDate { get; set; }
        public DateTime? escritorDate { get; set; }
        public string imgRef { get; set; }
        public string descripcion { get; set; }
    }
}