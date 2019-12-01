using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Planner.Model;

namespace Planner.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Карусель ГОД
        private RelayCommand previousYearCommand;
        public RelayCommand PreviousYearCommand
        {
            get
            {
                return previousYearCommand ??
                  (previousYearCommand = new RelayCommand(obj =>
                  {
                      CurrentYear = yearsList.MovePrevious();
                  }));
            }
        }

        private RelayCommand nextYearCommand;
        public RelayCommand NextYearCommand
        {
            get
            {
                return nextYearCommand ??
                  (nextYearCommand = new RelayCommand(obj =>
                  {
                      CurrentYear = yearsList.MoveNext();
                  }));
            }
        }

        private DoublyNode<int> currentYear;
        public DoublyNode<int> CurrentYear
        {
            get { return currentYear; }
            set
            {
                currentYear = value;
                OnPropertyChanged("CurrentYear");
            }
        }
        #endregion

        #region Карусель МЕСЯЦ
        private RelayCommand previousMonthCommand;
        public RelayCommand PreviousMonthCommand
        {
            get
            {
                return previousMonthCommand ??
                  (previousMonthCommand = new RelayCommand(obj =>
                  {
                      CurrentMonth = monthsList.MovePrevious();
                  }));
            }
        }

        private RelayCommand nextMonthCommand;
        public RelayCommand NextMonthCommand
        {
            get
            {
                return nextMonthCommand ??
                  (nextMonthCommand = new RelayCommand(obj =>
                  {
                      CurrentMonth = monthsList.MoveNext();
                  }));
            }
        }

        private DoublyNode<DateValue> currentMonth;
        public DoublyNode<DateValue> CurrentMonth
        {
            get { return currentMonth; }
            set
            {
                currentMonth = value;
                OnPropertyChanged("CurrentMonth");
            }
        }
        #endregion

        #region Карусель НЕДЕЛЯ
        private RelayCommand previousWeekommand;
        public RelayCommand PreviousWeekCommand
        {
            get
            {
                return previousWeekommand ??
                  (previousWeekommand = new RelayCommand(obj =>
                  {
                      CurrentWeek = weeksList.MovePrevious();
                  }));
            }
        }

        private RelayCommand previousAnyWeekommand;
        public RelayCommand PreviousAnyWeekommand
        {
            get
            {
                return previousAnyWeekommand ??
                  (previousAnyWeekommand = new RelayCommand(obj =>
                  {
                      CurrentWeek = weeksList.MovePrevious();
                  }));
            }
        }

        private RelayCommand nextWeekCommand;
        public RelayCommand NextWeekCommand
        {
            get
            {
                return nextWeekCommand ??
                  (nextWeekCommand = new RelayCommand(obj =>
                  {
                      CurrentWeek = weeksList.MoveNext();
                  }));
            }
        }

        private RelayCommand nextAnyWeeksCommand;
        public RelayCommand NextAnyWeeksCommand
        {
            get
            {
                return nextAnyWeeksCommand ??
                  (nextAnyWeeksCommand = new RelayCommand(obj =>
                  {
                      CurrentWeek = weeksList.MoveNext(Settings.GetSettings().DayLongMoveParametr);
                  }));
            }
        }

        private DoublyNode<DateValue> currentWeek;
        public DoublyNode<DateValue> CurrentWeek
        {
            get { return currentWeek; }
            set
            {
                currentWeek = value;
                OnPropertyChanged("CurrentWeek");
            }
        }
        #endregion

        #region Карусель ДЕНЬ
        private RelayCommand previousDaykommand;
        public RelayCommand PreviousDayCommand
        {
            get
            {
                return previousDaykommand ??
                  (previousDaykommand = new RelayCommand(obj =>
                  {
                      CurrentDay = daysList.MovePrevious();
                  }));
            }
        }

        private RelayCommand previousAnyDaysCommand;
        public RelayCommand PreviousAnyDaysCommand
        {
            get
            {
                return previousAnyDaysCommand ??
                  (previousAnyDaysCommand = new RelayCommand(obj =>
                  {
                      CurrentDay = daysList.MovePrevious(Settings.GetSettings().DayLongMoveParametr);
                  }));
            }
        }

        private RelayCommand nextDayCommand;
        public RelayCommand NextDayCommand
        {
            get
            {
                return nextDayCommand ??
                  (nextDayCommand = new RelayCommand(obj =>
                  {
                      CurrentDay = daysList.MoveNext();
                  }));
            }
        }

        private RelayCommand nextAnyDaysCommand;
        public RelayCommand NextAnyDaysCommand
        {
            get
            {
                return nextAnyDaysCommand ??
                  (nextAnyDaysCommand = new RelayCommand(obj =>
                  {
                      CurrentDay = daysList.MoveNext(Settings.GetSettings().DayLongMoveParametr);
                  }));
            }
        }

        private DoublyNode<DateValue> currentDay;
        public DoublyNode<DateValue> CurrentDay
        {
            get { return currentDay; }
            set
            {
                currentDay = value;
                OnPropertyChanged("CurrentDay");
            }
        }
        #endregion

        private DoublyNodeLinkedList<int> yearsList;
        private DoublyNodeLinkedList<DateValue> monthsList;
        private DoublyNodeLinkedList<DateValue> weeksList;
        private DoublyNodeLinkedList<DateValue> daysList;

        public MainViewModel()
        {
            FillYearList(DateTime.Now.Year);
            FillMonthsList(DateTime.Now.Year);
            FillWeeksList(DateTime.Now.Year);
            FillDaysList(DateTime.Now.Year);

            CurrentYear = yearsList.Current(DateTime.Now.Year);
            CurrentMonth = monthsList.Current(new DateValue(DateTime.Now.Month));
            CurrentWeek = weeksList.Current(NumberOfWeek(DateTime.Now));
            CurrentDay = daysList.Current(new DateValue(DateTime.Now.DayOfYear));
        }

        private void FillYearList(int year)
        {
            yearsList = new DoublyNodeLinkedList<int>();
            for (int i = year - 10; i < year+10 ; i++)
            {
                yearsList.Add(new DoublyNode<int>(i));
            }
        }

        private void FillMonthsList(int year)
        {
            monthsList = new DoublyNodeLinkedList<DateValue>();
            
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(1, "январь", new DateTime(year, 1, 1, 0, 0, 0), new DateTime(year, 1, 31, 23, 59, 59))));
            if (DateTime.IsLeapYear(year))
                monthsList.Add(new DoublyNode<DateValue>(new DateValue(2, "февраль", new DateTime(year, 2, 1, 0, 0, 0), new DateTime(year, 2, 29, 23, 59, 59))));
            else
                monthsList.Add(new DoublyNode<DateValue>(new DateValue(2, "февраль", new DateTime(year, 2, 1, 0, 0, 0), new DateTime(year, 2, 28, 23, 59, 59))));

            monthsList.Add(new DoublyNode<DateValue>(new DateValue(3, "март", new DateTime(year, 3, 1, 0, 0, 0), new DateTime(year, 3, 31, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(4, "апрель", new DateTime(year, 4, 1, 0, 0, 0), new DateTime(year, 4, 30, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(5, "май", new DateTime(year, 5, 1, 0, 0, 0), new DateTime(year, 5, 31, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(6, "июнь", new DateTime(year, 6, 1, 0, 0, 0), new DateTime(year, 6, 30, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(7, "июль", new DateTime(year, 7, 1, 0, 0, 0), new DateTime(year, 7, 31, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(8, "август", new DateTime(year, 8, 1, 0, 0, 0), new DateTime(year, 8, 31, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(9, "сентябрь", new DateTime(year, 9, 1, 0, 0, 0), new DateTime(year, 9, 30, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(10, "октябрь", new DateTime(year, 10, 1, 0, 0, 0), new DateTime(year, 10, 31, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(11, "ноябрь", new DateTime(year, 11, 1, 0, 0, 0), new DateTime(year, 11, 30, 23, 59, 59))));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(12, "декабрь", new DateTime(year, 12, 1, 0, 0, 0), new DateTime(year, 12, 31, 23, 59, 59))));
        }

        private void FillWeeksList(int year)
        {
            weeksList = new DoublyNodeLinkedList<DateValue>();
            DateTime weekStart = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime weekEnd = new DateTime(year, 1, 1, 23, 59, 59);
            int weekNumber = 0;

            while(true)
            {
                if (weekEnd.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekNumber++;
                    DateValue dateValue = new DateValue(weekNumber, weekStart.ToString("d MMM") + " - " + weekEnd.ToString("d MMM"), weekStart, weekEnd);
                    weeksList.Add(new DoublyNode<DateValue>(dateValue));
                    weekStart = weekEnd.AddSeconds(1);
                    weekEnd = weekEnd.AddDays(7);
                }
                else
                {
                    weekEnd = weekEnd.AddDays(1);
                }

                if (weekEnd.Year > year)
                {
                    if (weekStart.Year > year) break;
               
                    do
                    {
                        weekEnd = weekEnd.AddDays(-1);
                    }
                    while (weekEnd.Date != new DateTime(year, 12, 31));
                    weekNumber++;
                    DateValue dateValue = new DateValue(weekNumber, weekStart.ToString("d MMM") + " - " + weekEnd.ToString("d MMM"), weekStart, weekEnd);
                    weeksList.Add(new DoublyNode<DateValue>(dateValue));
                    break;
                }
            }
        }

        private void FillDaysList(int year)
        {
            daysList = new DoublyNodeLinkedList<DateValue>();
            int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
            DateTime dateStart = new DateTime(year, 1, 1, 0, 0, 0);
            for (int i = 0; i < daysInYear; i++)
            {
                DateValue dateValue = new DateValue(i+1, dateStart.AddDays(i).ToString("ddd, d MMM"));
                daysList.Add(new DoublyNode<DateValue>(dateValue));
            }
        }

        private DateValue NumberOfWeek(DateTime date)
        {
            var calendar = new GregorianCalendar();
            return new DateValue(calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }
               
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
