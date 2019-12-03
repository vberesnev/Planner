using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class TargetTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }

        public Target Target { get; set; }
        public int? TargetId { get; set; }

        public TargetTask() { }

        public TargetTask(string name, string desc, Target target)
        {
            Name = name;
            Description = desc;
            Done = false;
            Target = target;
        }
    }
}
