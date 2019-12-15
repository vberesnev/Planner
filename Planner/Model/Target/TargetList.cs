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

        public void Load(int targetType, int periodValue)
        {
            Items = new ObservableCollection<Target>(db.Targets.Include(t => t.Tasks)
                                                               .Include(u => u.Owner)
                                                               .Where(x => (int)x.TargetType == targetType && x.PeriodValue == periodValue)
                                                               .ToList());
        }

        public void Add(Target item)
        {
            Items.Add(item);
            db.Targets.Add(item);
            Save();
        }

        public void Edit(Target newTarget)
        {
            var editItem = db.Targets.Include(t => t.Tasks).Include(u => u.Owner).FirstOrDefault(x => x.Id == newTarget.Id);
            if (editItem != null)
            {
                //удаляю все старые задачи
                foreach (var task in GetRemovebleTargerTasksList(editItem, newTarget))
                {
                    db.TargetTasks.Remove(task);
                }
                //добавляю все новые задачи
                foreach (var task in newTarget.Tasks.Where(x => x.Id == 0))
                {
                    task.Target = editItem;
                    db.TargetTasks.Add(task);
                }
                //редактирую саму цель
                editItem.EditTask(newTarget);
                Save();
            }
        }

        private List<TargetTask> GetRemovebleTargerTasksList(Target oldTarget, Target newTarget)
        {
            List<TargetTask> list = new List<TargetTask>();
            foreach(var taskOld in oldTarget.Tasks)
            {
                if (newTarget.Tasks.Where(x => x.Id == taskOld.Id).Count() == 0)
                    list.Add(taskOld);
            }
            return list;
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
