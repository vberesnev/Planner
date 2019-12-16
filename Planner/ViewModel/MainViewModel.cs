using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Planner.Model;
using Planner.Model.Target;

namespace Planner.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private async void SetTargetList(int targetType, int periodValue)
        {
            SetTitle();
            InfoText = "Подождите, идёт загрузка . . .";
            InfoTextZIndex = 100;
            await Task.Run(() => Load(targetType, periodValue));
        }

        private void Load(int targetType, int periodValue)
        {
            Thread.Sleep(2000);
            TargetList.Load(targetType, periodValue);
            if (TargetList.Items.Count > 0)
                InfoTextZIndex = 0;
            else
                InfoText = "Целей нет, придумай что-нибудь";
        }
        
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

                OnPropertyChanged("CurrentYear");
                SetTargetList(365, CurrentYear.Data);
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
                TargetList.Load(30, CurrentMonth.Data.Key);
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
                TargetList.Load(7, CurrentWeek.Data.Key);
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
                TargetList.Load(1, CurrentDay.Data.Key);
            }
        }
        #endregion

        #region Команда установки пункта меню
        private RelayCommand setMenuItemCommand;
        public RelayCommand SetMenuItemCommand => setMenuItemCommand;

        private void SetMenuItem(object obj)
        {
            int parametr;
            if (Int32.TryParse(obj.ToString(), out parametr))
            {
                switch (parametr)
                {
                    case 0:
                        currentMenuItem = MenuItem.Year;
                        TargetList.Load(365, CurrentYear.Data);
                        break;
                    case 1:
                        currentMenuItem = MenuItem.Month;
                        TargetList.Load(30, CurrentMonth.Data.Key);
                        break;
                    case 2:
                        currentMenuItem = MenuItem.Week;
                        TargetList.Load(7, CurrentWeek.Data.Key);
                        break;
                    case 3:
                        currentMenuItem = MenuItem.Day;
                        TargetList.Load(1, CurrentDay.Data.Key);
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
        }
        #endregion

        #region Команда установки восклицательного знака в зависимости от важности цели
        private RelayCommand setSelectedTargetImportantValueCommand;
        public RelayCommand SetSelectedTargetImportantValueCommand => setSelectedTargetImportantValueCommand;

        private void SetSelectedTargetImportantValue(object obj)
        {
            int parametr;
            if (Int32.TryParse(obj.ToString(), out parametr))
            {
                if (parametr == (int)SelectedTarget.Important)
                {
                    SelectedTarget.Important = 0;
                    parametr = 0;
                }
                else
                    SelectedTarget.Important = (Important)parametr;

                SetExclamationPointButton(parametr);
            }
        }

        private void SetExclamationPointButton(int parametr)
        {
            LowImportantButtonColor = "Gray";
            MiddleImportantButtonColor = "Gray";
            HighImportantButtonColor = "Gray";

            switch (parametr)
            {
                case 0:
                    break;
                case 1:
                    LowImportantButtonColor = "LightCoral";
                    break;
                case 2:
                    LowImportantButtonColor = "LightCoral";
                    MiddleImportantButtonColor = "LightCoral";
                    break;
                case 3:
                    LowImportantButtonColor = "LightCoral";
                    MiddleImportantButtonColor = "LightCoral";
                    HighImportantButtonColor = "LightCoral";
                    break;
            }
        }

        private string lowImportantButtonColor;
        public string LowImportantButtonColor
        {
            get { return lowImportantButtonColor; }
            set { lowImportantButtonColor = value; OnPropertyChanged("LowImportantButtonColor"); }
        }

        private string middleImportantButtonColor;
        public string MiddleImportantButtonColor
        {
            get { return middleImportantButtonColor; }
            set { middleImportantButtonColor = value; OnPropertyChanged("MiddleImportantButtonColor"); }
        }

        private string highImportantButtonColor;
        public string HighImportantButtonColor
        {
            get { return highImportantButtonColor; }
            set { highImportantButtonColor = value; OnPropertyChanged("HighImportantButtonColor"); }
        }
        #endregion

        #region Команда добавления новой цели в список целей
        private RelayCommand addTargetCommand;
        public RelayCommand AddTargetCommand => addTargetCommand;
        
        private void AddTarget(object obj)
        {
            if (SelectedTarget.Id == 0)
            {
                int periodValue = 0;
                switch (SelectedTarget.TargetType)
                {
                    case TargetType.Year:
                        periodValue = CurrentYear.Data;
                        break;
                    case TargetType.Month:
                        periodValue = currentMonth.Data.Key;
                        break;
                    case TargetType.Week:
                        periodValue = CurrentWeek.Data.Key;
                        break;
                    case TargetType.Day:
                        periodValue = CurrentDay.Data.Key;
                        break;
                }
                Target newTarget = new Target(SelectedTarget.Name,
                                              SelectedTarget.Description,
                                              SelectedTarget.TargetType,
                                              CurrentYear.Data, periodValue,
                                              SelectedTarget.LastDate,
                                              SelectedTarget.Important, null);
                newTarget.Tasks = SelectedTarget.Tasks;
                TargetList.Add(newTarget);
            }
            else
            {
                TargetList.Edit(SelectedTarget);
            }
            CloseAction();
        }
        #endregion

        #region Команда удаления цели из списка целей (АСИНХРОННО, для отмены используется счетчик и кнопка отмены)
        private RelayCommand deleteTargetCommand;
        public RelayCommand DeleteTargetCommand => deleteTargetCommand;
        
        private async void DeleteTarget(object obj)
        {
            var result = await Task.Run(() => TikTak());
            if (result)
            {
                Target deleteTarget = obj as Target;
                if (deleteTarget != null)
                {
                    TargetList.Remove(deleteTarget);
                }
            }
        }

        private bool undo;

        private RelayCommand undoCommand;
        public RelayCommand UndoCommand => undoCommand;

        private int undoCounter;
        public int UndoCounter
        {
            get { return undoCounter; }
            set
            {
                undoCounter = value;
                OnPropertyChanged("UndoCounter");
            }
        }

        private string undoVisibility;
        public string UndoVisibility
        {
            get { return undoVisibility; }
            set
            {
                undoVisibility = value;
                OnPropertyChanged("UndoVisibility");
            }
        }

        private bool TikTak()
        {
            UndoVisibility = "Visible";
            for (int i = 5; i > 0; i--)
            {
                UndoCounter = i;
                Thread.Sleep(1000);
                if (undo)
                {
                    UndoVisibility = "Hidden";
                    undo = false;
                    return false;
                }
            }
            UndoVisibility = "Hidden";
            undo = false;
            return true;
        }
        #endregion

        #region Команда разворачивание и сворачивание карточки цели
        private RelayCommand minimizeMaximizeItemCommand;
        public RelayCommand MinimizeMaximizeItemCommand => minimizeMaximizeItemCommand;
        public void MinimizeMaximizeItem(object obj)
        {
            Target target = obj as Target;
            if (target != null)
                target.ChangeView();
        }
        #endregion

        private RelayCommand addTargetTaskCommand;
        public RelayCommand AddTargetTaskCommand
        {
            get
            {
                return addTargetTaskCommand ??
                  (addTargetTaskCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrWhiteSpace(SelectedTargetTask.Name))
                      {
                          SelectedTarget.AddTask(SelectedTargetTask);
                          SelectedTargetTask = new TargetTask();
                      }
                  }));
            }
        }

        private RelayCommand deleteTargetTaskCommand;
        public RelayCommand DeleteTargetTaskCommand
        {
            get
            {
                return deleteTargetTaskCommand ??
                  (deleteTargetTaskCommand = new RelayCommand(obj =>
                  {
                      TargetTask deleteTargetTask = obj as TargetTask;
                      if (deleteTargetTask != null)
                      {
                          SelectedTarget.RemoveTask(deleteTargetTask);
                      }
                  }));
            }
        }

        //Текст заголовка на форме
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

        //Двунаправленные связанные списки (лет, месяцев, недель, дней). Нужны для каруселей выбора дат
        private DoublyNodeLinkedList<int> yearsList;
        private DoublyNodeLinkedList<DateValue> monthsList;
        private DoublyNodeLinkedList<DateValue> weeksList;
        private DoublyNodeLinkedList<DateValue> daysList;

        /// <summary>
        /// Текущее значение пункта меню
        /// </summary>
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

        private Target selectedTarget;
        public Target SelectedTarget
        {
            get { return selectedTarget; }
            set
            {
                selectedTarget = value;
                OnPropertyChanged("SelectedTarget");
            }
        }

        private TargetTask selectedTargetTask;
        public TargetTask SelectedTargetTask
        {
            get { return selectedTargetTask; }
            set
            {
                selectedTargetTask = value;
                OnPropertyChanged("SelectedTargetTask");
            }
        }

        private string infoText;
        public string InfoText
        {
            get { return infoText; }
            set
            {
                infoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        //Z индекс для строки "Нет целей"
        private int infoTextZIndex;
        public int InfoTextZIndex
        {
            get { return infoTextZIndex; }
            set
            {
                infoTextZIndex = value;
                OnPropertyChanged("InfoTextZIndex");
            }
        }

        public Action CloseAction { get; set; }

        public MainViewModel()
        {
            TargetList = new TargetList();

            setMenuItemCommand = new RelayCommand(SetMenuItem);
            setSelectedTargetImportantValueCommand = new RelayCommand(SetSelectedTargetImportantValue);
            undoCommand = new RelayCommand((obj) => undo = true); //команда Отмены удаления (ставит флаг undo в true)
            minimizeMaximizeItemCommand = new RelayCommand(MinimizeMaximizeItem);

            addTargetCommand = new RelayCommand(AddTarget);
            deleteTargetCommand = new RelayCommand(DeleteTarget);

            currentMenuItem = MenuItem.Year;
            FillYearList(DateTime.Now.Year);
            CurrentYear = yearsList.Current(DateTime.Now.Year);

            LowImportantButtonColor = "Gray";
            MiddleImportantButtonColor = "Gray";
            HighImportantButtonColor = "Gray";

            UndoVisibility = "Hidden";
        }

        /// <summary>
        /// Метод определения типа "Выбранной Цели" (годовая, месячная, недельная, дневная). Используется при нажатии кнопки "Добавить цель"
        /// </summary>
        public void SetNewSelectedTarget()
        {
            switch (currentMenuItem)
            {
                case MenuItem.Year:
                    SelectedTarget = new Target(TargetType.Year);
                    break;
                case MenuItem.Month:
                    SelectedTarget = new Target(TargetType.Month);
                    break;
                case MenuItem.Week:
                    SelectedTarget = new Target(TargetType.Week);
                    break;
                case MenuItem.Day:
                    SelectedTarget = new Target(TargetType.Day);
                    break;
                default:
                    break;
            }
            SelectedTargetTask = new TargetTask();
        }

        public void SetNewSelectedTarget(int id)
        {
            SelectedTarget = TargetList.Items.FirstOrDefault(x => x.Id == id).Clone();
            SetExclamationPointButton((int)SelectedTarget.Important);
            SelectedTargetTask = new TargetTask();
        }

        /// <summary>
        /// Заполнить двунаправленный связанный список лет
        /// </summary>
        /// <param name="year">Год, от которого вести отчёт</param>
        private void FillYearList(int year)
        {
            yearsList = new DoublyNodeLinkedList<int>();
            for (int i = year - 10; i < year+10 ; i++)
            {
                yearsList.Add(new DoublyNode<int>(i));
            }
        }

        /// <summary>
        /// Заполнить двунаправленный связанный список месяцев
        /// </summary>
        /// <param name="year">Год (от значения года зависит високосный год или нет)</param>
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

        /// <summary>
        /// Заполнить двунаправленный связанный список недель в году
        /// </summary>
        /// <param name="year">От значения года зависит дата первого дня первой недели</param>
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

        /// <summary>
        /// Заполнить двунаправленный связанный список дней в году
        /// </summary>
        /// <param name="year">Год (от значения года зависит високосный год или нет)</param>
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

        /// <summary>
        /// Метод установки заголовка 
        /// </summary>
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
                    TitleText = $"Цели на {CurrentWeek.Data.Key}-ю неделю {CurrentYear.Data} года ({CurrentWeek.Data.Value})";
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

        /// <summary>
        /// Метод получения номера недели в зависимости от даты 
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Объект DateValue с ключом - значением номером недели в году</returns>
        private DateValue NumberOfWeek(DateTime date)
        {
            var calendar = new GregorianCalendar();
            return new DateValue(calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }

        /// <summary>
        /// Метод получения номера последней недели в году
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Номер недели</returns>
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
