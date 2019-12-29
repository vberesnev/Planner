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
        [Key]
        public int Id { get; set; }
        private string name;
        public string Name
        { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        public bool Done { get; set; }
        public int? TargetId { get; set; }

        [Required]
        public Target Target { get; set; }
        

        public TargetTask() { }

        public TargetTask(string name, Target target)
        {
            Name = name;
            Done = false;
            Target = target;
        }

        private TargetTask(int id, string name, bool done, Target target)
        {
            Id = id;
            Name = name;
            Done = done;
            Target = target;
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public TargetTask Clone()
        {
            return new TargetTask(this.Id, this.Name, this.Done, this.Target);
        }
    }
}
