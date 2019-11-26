using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Planner.Model;

namespace Planner.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private RelayCommand previousYearCommand;
        public RelayCommand PreviousYearCommand
        {
            get
            {
                return previousYearCommand ??
                  (previousYearCommand = new RelayCommand(obj =>
                  {
                      CurrentYear = yearsList.MovePrevious();
                      PreviousYear = CurrentYear.Previous;
                      NextYear = CurrentYear.Next;
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
                      PreviousYear = CurrentYear.Previous;
                      NextYear = CurrentYear.Next;
                  }));
            }
        }


        private DoublyNode<int> previousYear;
        public DoublyNode<int> PreviousYear
        {
            get { return previousYear; }
            set
            {
                previousYear = value;
                OnPropertyChanged("PreviousYear");
            }
        }

        private DoublyNode<int> nextYear;
        public DoublyNode<int> NextYear
        {
            get { return nextYear; }
            set
            {
                nextYear = value;
                OnPropertyChanged("NextYear");
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

        private DoublyNodeLinkedList<int> yearsList;
        private DoublyNodeLinkedList<DateValue> monthsList;
        private DoublyNodeLinkedList<int> weeksList;
        private DoublyNodeLinkedList<int> daysList;

        public MainViewModel()
        {
            FillYearList(DateTime.Now);
            FillMonthsList();

            CurrentYear = yearsList.Current(DateTime.Now.Year);
            PreviousYear = CurrentYear.Previous;
            NextYear = CurrentYear.Next;

        }

        private void FillYearList(DateTime date)
        {
            yearsList = new DoublyNodeLinkedList<int>();
            for (int i = date.Year - 10; i < date.Year+10 ; i++)
            {
                yearsList.Add(new DoublyNode<int>(i));
            }
        }

        private void FillMonthsList()
        {
            monthsList = new DoublyNodeLinkedList<DateValue>();
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(1, "январь")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(2, "февраль")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(3, "март")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(4, "апрель")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(5, "май")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(6, "июнь")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(7, "июль")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(8, "август")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(9, "сентябрь")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(10, "октябрь")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(11, "ноябрь")));
            monthsList.Add(new DoublyNode<DateValue>(new DateValue(12, "декабрь")));
        }

        private void FillWeeksList(DateTime date)
        {

        }

        private void FillDaysList(DateTime date)
        {
            daysList = new DoublyNodeLinkedList<int>();
            int daysInYear = DateTime.IsLeapYear(date.Year) ? 366 : 365;
            for (int i = 1; i < daysInYear; i++)
            {

            }
        }

        private Dictionary<int, string> monthsDictionary = new Dictionary<int, string>
        {
            {1, "январь"},
            {2, "февраль"},
            {3, "март"},
            {4, "апрель"},
            {5, "май"},
            {6, "июнь"},
            {7, "июль"},
            {8, "август"},
            {9, "сентябрь"},
            {10, "октябрь"},
            {11, "ноябрь"},
            {12, "декабрь"}
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
