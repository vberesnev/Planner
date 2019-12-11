using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Planner.Model.Target
{
    public class TargetList: INotifyPropertyChanged
    {
        private ObservableCollection<Target> items;
        public ObservableCollection<Target> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public int Count => Items.Count();
        public int DoneCount => Items.Where(x => x.Done = true).Count();
        public int OverdueCount => Items.Where(x => x.Done = false).Count();

        private PlannerContext db;

        public TargetList()
        {
            db = new PlannerContext();
        } 

        public void Load()
        {
            Items = new ObservableCollection<Target>(db.Targets.Include(t => t.Tasks).Include(u => u.Owner).ToList());
        }

        public void Add(Target item)
        {
            Items.Add(item);
            db.Targets.Add(item);
            Save();
        }

        public void Remove(Target item)
        {
            Items.Remove(item);
            db.Targets.Remove(item);
            Save();
        }

        private void Save()
        {
            db.SaveChanges();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
