using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facemash.WPF.DTO
{
    public class Cat
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public int Ranking { get; set; }
        public List<string> History { get; set; }

    }
}
