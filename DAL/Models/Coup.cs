using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Coup
    {
        public int Id { get; set; }
        public string Depart {  get; set; }
        public string Arrive { get; set; }
        public char Piece { get; set; }  //char c est un seul character
        public int IdLigne { get; set; }
        public int? ParentCoupId { get; set; }
        public int Ordre {  get; set; }
        public string Description { get; set; }
    }
}
