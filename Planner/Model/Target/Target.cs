using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
        public bool Done { get; set; }

        public ICollection<TargetTask> Tasks { get; set; }


        public DateTime PeriodStart => DateOperations.PeriodStart(TargetType, Year, PeriodValue);
        public DateTime PeriodFinish => DateOperations.PeriodFinish(TargetType, Year, PeriodValue);


        public Target() { }

        public Target(TargetType targetType)
        {
            TargetType = targetType;
            Tasks = new List<TargetTask>();
        }

        public Target(string name, string desc, TargetType targetType, int year, int periodValue, DateTime? lastDate, Important important, User owner)
        {
            Name = name;
            Description = desc;
            TargetType = targetType;
            Year = year;
            PeriodValue = periodValue;
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
            Tasks = new List<TargetTask>();
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
    }
}
