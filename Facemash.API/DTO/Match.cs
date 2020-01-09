using System;
using System.Collections.Generic;
using System.Text;

namespace Facemash.API.DTO
{
    public class Match
    {
        public Cat Cat_1 { get; set; }
        public Cat Cat_2 { get; set; }
        public bool Result { get; set; }
    }
}
