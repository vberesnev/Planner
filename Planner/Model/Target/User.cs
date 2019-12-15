using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Target> Targets { get; set; }

        public User() { }

        public User(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
