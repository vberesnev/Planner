using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class Target : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        private string name;
        public string Name
        { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        public string Description { get; set; }
        public TargetType TargetType { get; set; }
        //Год в котором поставлена цель
        public int Year { get; set; }
        //Значение периода (номер месяца для TargetType.Month, номер недели для TargetType.Week, номер дня в году для TargetType.Day, номер года для TargetType.Year)
        public int PeriodValue { get; set; }
        //дата, к которой решить задачу (по умолчанию - последний день периода PeriodValue)
        public DateTime? LastDate { get; set; }
        //Дата пролонгации цели
        public DateTime? ProlongationDate { get; set; }
        public Important Important { get; set; }
        //Владелец цели
        public int? UserId { get; set; }
        public User Owner { get; set; }
        public bool ForAllUsers { get; set; }
        private bool done;
        public bool Done
        { get { return done; } set { done = value; OnPropertyChanged("Done"); OnPropertyChanged("BackColor"); } }

        private ObservableCollection<TargetTask> tasks;
        public ObservableCollection<TargetTask> Tasks { get { return tasks; } set { tasks = value; OnPropertyChanged("Tasks"); } }


        public DateTime PeriodStart => DateOperations.PeriodStart(TargetType, Year, PeriodValue);
        public DateTime PeriodFinish => DateOperations.PeriodFinish(TargetType, Year, PeriodValue);

        private string fullView = "Collapsed";
        [NotMapped]
        public string FullView
        { get { return fullView; } set { fullView = value; OnPropertyChanged("FullView"); } }

        [NotMapped]
        public string BackColor
        {
            get
            {
                if (Done) return "LightGreen";

                else if (LastDate < DateTime.Now) return "LightCoral";

                else return "Transparent";
            }
        }


        public Target() { }

        public Target(TargetType targetType)
        {
            TargetType = targetType;
            Tasks = new ObservableCollection<TargetTask>();
        }

        public Target(string name, string desc, TargetType targetType, int year, int periodValue, DateTime? lastDate, Important important, User owner )
        {
            Name = name;
            Description = desc;
            TargetType = targetType;
            Year = year;
            PeriodValue = periodValue;
            if (lastDate == null)
                LastDate = PeriodFinish;
            else
                LastDate = lastDate;
            ProlongationDate = null;
            Important = important;
            if (owner == null)
            {
                Owner = null;
                ForAllUsers = true;
            }
            else
            {
                Owner = owner;
                ForAllUsers = false;
            }
            Done = false;
            Tasks = new ObservableCollection<TargetTask>();
        }

        private Target(int  id, string name, string desc, TargetType targetType, int year, int periodValue, DateTime? lastDate, DateTime? prolongationDate, Important important, User owner, bool done)
        {
            Id = id;
            Name = name;
            Description = desc;
            TargetType = targetType;
            Year = year;
            PeriodValue = periodValue;
            if (lastDate == null)
                LastDate = PeriodFinish;
            else
                LastDate = lastDate;
            ProlongationDate = prolongationDate;
            Important = important;
            if (owner == null)
            {
                Owner = null;
                ForAllUsers = true;
            }
            else
            {
                Owner = owner;
                ForAllUsers = false;
            }
            Done = done;
            Tasks = new ObservableCollection<TargetTask>();
        }

        public void AddTask(TargetTask task)
        {
            Tasks.Add(task);
        }

        public void RemoveTask(TargetTask task)
        {
            Tasks.Remove(task);
        }

        public void EditTask(Target newTarget)
        {
            this.Name = newTarget.Name;
            this.Description = newTarget.Description;
            this.TargetType = newTarget.TargetType;
            this.Year = newTarget.Year;
            this.PeriodValue = newTarget.PeriodValue;

            if (newTarget.LastDate == null)
                this.LastDate = newTarget.PeriodFinish;
            else
                this.LastDate = newTarget.LastDate;

            this.ProlongationDate = newTarget.ProlongationDate;
            this.Important = newTarget.Important;

            if (newTarget.Owner == null)
            {
                this.Owner = null;
                this.ForAllUsers = true;
            }
            else
            {
                Owner = this.Owner;
                this.ForAllUsers = false;
            }

            this.Done = newTarget.Done;
            //this.Tasks = newTarget.Tasks;
        }

        /// <summary>
        /// Скрыть/раскрыть карточку задачи
        /// </summary>
        public void ChangeView()
        {
            if (FullView == "Visible")
                FullView = "Collapsed";
            else
                FullView = "Visible";
        }

        private DateTime? GetDateFromSqliteFormat(string value)
        {
            string pattern = @"(\d{4})-(\d{2})-(\d{2})";
            if (Regex.IsMatch(value, pattern))
            {
                Match match = Regex.Match(value, pattern);
                int year = Convert.ToInt32(match.Groups[1].Value);
                int month = Convert.ToInt32(match.Groups[2].Value);
                int day = Convert.ToInt32(match.Groups[3].Value);

                return new DateTime(year, month, day, 0, 0, 0);
            }
            else
            {
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Target Clone()
        {
            Target cloneTarget = new Target(this.Id, this.Name, this.Description, this.TargetType, this.Year, this.PeriodValue, this.LastDate, this.ProlongationDate, this.Important, this.Owner, this.Done);
            foreach(var task in this.Tasks)
            {
                cloneTarget.Tasks.Add(task.Clone());
            }
            return cloneTarget;
        }
    }
}
