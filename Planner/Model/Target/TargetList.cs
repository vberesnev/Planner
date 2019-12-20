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

        private int yearTargetsCount;
        public int YearTargetsCount { get { return yearTargetsCount; }  set { yearTargetsCount = value; OnPropertyChanged("YearTargetsCount"); } }
        private int monthTargetsCount;
        public int MonthTargetsCount { get { return monthTargetsCount; } set { monthTargetsCount = value; OnPropertyChanged("MonthTargetsCount"); } }
        private int weekTargetsCount;
        public int WeekTargetsCount { get { return weekTargetsCount; } set { weekTargetsCount = value; OnPropertyChanged("WeekTargetsCount"); } }
        private int dayTargetsCount;
        public int DayTargetsCount { get { return dayTargetsCount; } set { dayTargetsCount = value; OnPropertyChanged("DayTargetsCount"); } }

        private PlannerContext db;

        public TargetList()
        {
            db = new PlannerContext();
        } 

        public void Load(int year, int targetType, int periodValue)
        {
            Items = new ObservableCollection<Target>(db.Targets.Include(t => t.Tasks)
                                                               .Include(u => u.Owner)
                                                               .Where(x => x.Year == year && (int)x.TargetType == targetType && x.PeriodValue == periodValue)
                                                               .ToList());
            switch (targetType)
            {
                case 365:
                    YearTargetsCount = Items.Count;
                    break;
                case 30:
                    MonthTargetsCount = Items.Count;
                    break;
                case 7:
                    WeekTargetsCount = Items.Count;
                    break;
                case 1:
                    DayTargetsCount = Items.Count;
                    break;
            }
            YearTargetsCount = db.Targets.Where(x => x.Year == year && (int)x.TargetType == 365 && x.PeriodValue == year && x.Done == false).Count();
        }

        public void LoadCounters(int yearPeriod, int monthPeriod, int weekPeriod, int dayPeriod)
        {
            YearTargetsCount = db.Targets.Where(x => x.Year == yearPeriod && (int)x.TargetType == 365 && x.PeriodValue == yearPeriod && x.Done == false).Count();
            MonthTargetsCount = db.Targets.Where(x => x.Year == yearPeriod && (int)x.TargetType == 30 && x.PeriodValue == monthPeriod && x.Done == false).Count();
            WeekTargetsCount = db.Targets.Where(x => x.Year == yearPeriod && (int)x.TargetType == 7 && x.PeriodValue == weekPeriod && x.Done == false).Count();
            DayTargetsCount = db.Targets.Where(x => x.Year == yearPeriod && (int)x.TargetType == 1 && x.PeriodValue == dayPeriod && x.Done == false).Count();
        }

        public void LoadDone()
        {
            Items = new ObservableCollection<Target>(db.Targets.Include(t => t.Tasks)
                                                              .Include(u => u.Owner)
                                                              .Where(x=> x.Done == true)
                                                              .ToList());
        }

        public void LoadOverdue()
        {
            Items = new ObservableCollection<Target>(db.Targets.Include(t => t.Tasks)
                                                              .Include(u => u.Owner)
                                                              .Where(x => x.LastDate < DateTime.Now && x.Done == false)
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
                editItem.EditTarget(newTarget);
                Save();
            }
        }

        public void Done(Target target)
        {
            var editItem = db.Targets.FirstOrDefault(x => x.Id == target.Id);
            Save();
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
