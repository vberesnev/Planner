using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Target> Targets { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
