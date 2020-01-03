using Planner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Planner.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private int weekLongMoveParametr;
        public int WeekLongMoveParametr
        {
            get { return weekLongMoveParametr; }
            set
            {
                weekLongMoveParametr = value;
                OnPropertyChanged("WeekLongMoveParametr");
            }
        }

        private int dayLongMoveParametr;
        public int DayLongMoveParametr
        {
            get { return dayLongMoveParametr; }
            set
            {
                dayLongMoveParametr = value;
                OnPropertyChanged("DayLongMoveParametr");
            }
        }

        private int daysBeforeTaskFinish;
        public int DaysBeforeTaskFinish
        {
            get { return daysBeforeTaskFinish; }
            set
            {
                daysBeforeTaskFinish = value;
                OnPropertyChanged("DaysBeforeTaskFinish");
            }
        }

        public Action CloseAction { get; set; }

        private RelayCommand saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => saveSettingsCommand;

        public SettingsViewModel()
        {
            DayLongMoveParametr  = Settings.GetSettings().DayLongMoveParametr;
            WeekLongMoveParametr = Settings.GetSettings().WeekLongMoveParametr;
            DaysBeforeTaskFinish = Settings.GetSettings().DaysBeforeTaskFinish;
            saveSettingsCommand = new RelayCommand(SaveSettings);
        }

        private void SaveSettings(object obj)
        {
            Settings.GetSettings().DayLongMoveParametr = DayLongMoveParametr;
            Settings.GetSettings().WeekLongMoveParametr = WeekLongMoveParametr;
            Settings.GetSettings().DaysBeforeTaskFinish = DaysBeforeTaskFinish;
            Settings.Save();
            CloseAction();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
