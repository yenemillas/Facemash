using System;
using System.Collections.Generic;

namespace Facemash.API.DTO
{
    public class Cat
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public int Ranking { get; set; }
        public List<string> History { get; set; }

        public Cat()
        {
            Ranking = 100;
            History = new List<string>();
        }
    }
}
