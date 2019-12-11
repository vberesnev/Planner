using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public class TargetTask : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;
        public string Name
        { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private string description;
        public string Description
        { get { return description; } set { description = value; OnPropertyChanged("Description"); } }
        public bool Done { get; set; }
        public int? TargetId { get; set; }

        [Required]
        public Target Target { get; set; }
        

        public TargetTask() { }

        public TargetTask(string name, string desc, Target target)
        {
            Name = name;
            Description = desc;
            Done = false;
            Target = target;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
