using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TargetType TargetType { get; set; }
        //Год в котором поставлена цель
        public int Year { get; set; }
        //Значение периода (номер месяца для TargetType.Month, номер недели для TargetType.Week, номер для в году для TargetType.Day, номер года для TargetType.Year)
        public int PeriodValue { get; set; }
        //дата, к которой решить задачу (по умолчанию - последний день периода PeriodValue)
        public DateTime LastDate { get; set; }
        //Дата пролонгации цели
        public DateTime ProlongationDate { get; set; }
        public Important Important { get; set; }
        //Владелец цели
        public bool Done { get; set; }
        public User Owner { get; set; }
        public List<TargetTask> Tasks { get; set; }

        public DateTime PeriodStart  => DateOperations.PeriodStart(TargetType,  Year, PeriodValue);
        public DateTime PeriodFinish => DateOperations.PeriodFinish(TargetType, Year, PeriodValue);

    }
}
