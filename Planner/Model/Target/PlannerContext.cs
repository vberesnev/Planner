using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    class PlannerContext: DbContext
    {
        public PlannerContext() : base("PlannerDbConnection") { }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetTask> TargetTasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
