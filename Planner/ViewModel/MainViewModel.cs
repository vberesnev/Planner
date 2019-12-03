using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Planner.Model;
using Planner.Model.Target;

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

                FillMonthsList(currentYear.Data);
                FillWeeksList(currentYear.Data);
                FillDaysList(currentYear.Data);

                if (CurrentMonth != null)
                    CurrentMonth = monthsList.Current(CurrentMonth.Data);
                else
                    CurrentMonth = monthsList.Current(new DateValue(DateTime.Now.Month));

                if (CurrentWeek != null)
                {
                    if (CurrentWeek.Data.Key <= NumberOfLastWeekInYear(CurrentYear.Data))
                        CurrentWeek = weeksList.Current(CurrentWeek.Data);
                    else
                        CurrentWeek = weeksList.Current(new DateValue(NumberOfLastWeekInYear(CurrentYear.Data)));
                }
                else
                    CurrentWeek = weeksList.Current(NumberOfWeek(DateTime.Now));

                if (CurrentDay != null)
                    CurrentDay = daysList.Current(CurrentDay.Data);
                else
                    CurrentDay = daysList.Current(new DateValue(DateTime.Now.DayOfYear));

                SetTitle();

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
                SetTitle();
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

        private RelayCommand previousAnyWeeksCommand;
        public RelayCommand PreviousAnyWeeksCommand
        {
            get
            {
                return previousAnyWeeksCommand ??
                  (previousAnyWeeksCommand = new RelayCommand(obj =>
                  {
                      CurrentWeek = weeksList.MovePrevious(Settings.GetSettings().WeekLongMoveParametr);
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
                      CurrentWeek = weeksList.MoveNext(Settings.GetSettings().WeekLongMoveParametr);
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
                SetTitle();
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
                SetTitle();
                OnPropertyChanged("CurrentDay");
            }
        }
        #endregion

        private RelayCommand setMenuItemCommand;
        public RelayCommand SetMenuItemCommand
        {
            get
            {
                return setMenuItemCommand ??
                  (setMenuItemCommand = new RelayCommand(obj =>
                  {
                      int parametr;
                      if (Int32.TryParse(obj.ToString(), out parametr))
                      {
                          switch (parametr)
                          {
                              case 0:
                                  currentMenuItem = MenuItem.Year;
                                  break;
                              case 1:
                                  currentMenuItem = MenuItem.Month;
                                  break;
                              case 2:
                                  currentMenuItem = MenuItem.Week;
                                  break;
                              case 3:
                                  currentMenuItem = MenuItem.Day;
                                  break;
                              case 4:
                                  currentMenuItem = MenuItem.Overdue;
                                  break;
                              case 5:
                                  currentMenuItem = MenuItem.Done;
                                  break;
                          }
                          SetTitle();
                      }
                  }));
            }
        }

        private RelayCommand addTargetCommand;
        public RelayCommand AddTargetCommand
        {
            get
            {
                return addTargetCommand ??
                  (addTargetCommand = new RelayCommand(obj =>
                  {
                      Target target = new Target("Новая цель", "Описание", TargetType.Year, 2019, 2019, new DateTime(2019, 12, 31), Important.None, null);

                      TargetTask task1 = new TargetTask("Задача для новой цели", "ее описание", target);
                      TargetTask task2 = new TargetTask("Задача 2 для новой цели", "ее описание", target);
                      target.Tasks.Add(task1);
                      target.Tasks.Add(task2);

                      TargetList.Add(target);

                  }));
            }
        }



        private string titleText;
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                OnPropertyChanged("TitleText");
            }
        }

        private DoublyNodeLinkedList<int> yearsList;
        private DoublyNodeLinkedList<DateValue> monthsList;
        private DoublyNodeLinkedList<DateValue> weeksList;
        private DoublyNodeLinkedList<DateValue> daysList;

        private MenuItem currentMenuItem;

        private TargetList targetList;
        public TargetList TargetList
        {
            get { return targetList; }
            set
            {
                targetList = value;
                OnPropertyChanged("TargetList");
            }
        }

        public MainViewModel()
        {
            currentMenuItem = MenuItem.Year;
            FillYearList(DateTime.Now.Year);
            CurrentYear = yearsList.Current(DateTime.Now.Year);
            TargetList = new TargetList();
            TargetList.Load();
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
                DateValue dateValue = new DateValue(i+1, dateStart.AddDays(i).ToString("ddd, d MMM"), dateStart.AddDays(i), dateStart.AddDays(i).AddHours(23).AddMinutes(59).AddSeconds(59));
                daysList.Add(new DoublyNode<DateValue>(dateValue));
            }
        }

        private void SetTitle()
        {
            switch (currentMenuItem)
            {
                case MenuItem.Year:
                    TitleText = $"Цели на {CurrentYear.Data} год";
                    break;
                case MenuItem.Month:
                    TitleText = $"Цели на {CurrentMonth.Data.Value.ToLower()} {CurrentYear.Data} года";
                    break;
                case MenuItem.Week:
                    TitleText = $"Цели на {CurrentWeek.Data.Key} неделю {CurrentYear.Data} года ({CurrentWeek.Data.Value})";
                    break;
                case MenuItem.Day:
                    TitleText = "Цели на " + CurrentDay.Data.Start.ToString("ddd, d MMMM yyyy") +" года";
                    break;
                case MenuItem.Overdue:
                    TitleText = "Просроченные цели";
                    break;
                case MenuItem.Done:
                    TitleText = "Завершенные цели";
                    break;
            }
        }

        private DateValue NumberOfWeek(DateTime date)
        {
            var calendar = new GregorianCalendar();
            return new DateValue(calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }

        private int NumberOfLastWeekInYear(int year)
        {
            DateTime lastDay = new DateTime(year, 12, 31);
            var calendar = new GregorianCalendar();
            return calendar.GetWeekOfYear(lastDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
               
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
