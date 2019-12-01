using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Planner.Model.Target
{
    public class TargetList
    {
        public IEnumerable<Target> Items { get; set; }

        public int Count => Items.Count();
        public int DoneCount => Items.Where(x => x.Done = true).Count();
        public int OverdueCount => Items.Where(x => x.Done = false).Count();

        private PlannerContext db;

        public TargetList()
        {
            db = new PlannerContext();
            Items = db.Targets.Include(t => t.Tasks).Include(u => u.Owner).ToList();
        } 

        public void Load() { }

        public void Add(Target item) { }

        public void Remove(Target item) { }

        private void Save() { }
    }
}
