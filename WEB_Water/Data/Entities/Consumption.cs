using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Consumption : IEntity
    {
        public int Id { get; set; }

        public int Section { get; set; }

        public int LowerBound { get; set; }

        public int UpperBound { get; set; }

        public double Price { get; set; }
    }
}
