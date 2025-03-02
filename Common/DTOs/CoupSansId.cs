using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class CoupSansId
    {
        public string Depart { get; set; }
        public string Arrive { get; set; }
        public char Piece { get; set; }  
        public int IdLigne { get; set; }
        public int? ParentCoupId { get; set; }
        public int Ordre { get; set; }
        public string Description { get; set; }
    }
}
