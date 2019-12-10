using Planner.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Planner.ViewModel;
using System.Threading;

namespace Planner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int menuClickedIndex;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("calendar.ico");
            ni.Visible = true;
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            this.MenuCell0.Background = Brushes.CornflowerBlue;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Owner = this;
            sw.ShowDialog();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            menuClickedIndex = int.Parse(((Button)e.Source).Uid);

            this.MenuCell0.Background = null;
            this.MenuCell1.Background = null;
            this.MenuCell2.Background = null;
            this.MenuCell3.Background = null;
            this.MenuCell4.Background = null;
            this.MenuCell5.Background = null;

            switch (menuClickedIndex)
            {
                case 0:
                    this.MenuCell0.Background = Brushes.CornflowerBlue;
                    this.PlusButton.Visibility = Visibility.Visible;
                    this.FilterGrid.Visibility = Visibility.Visible;
                    this.MonthFilterPanel.Visibility = Visibility.Hidden;
                    this.WeekFilterPanel.Visibility = Visibility.Hidden;
                    this.DayFilterPanel.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    this.MenuCell1.Background = Brushes.CornflowerBlue;
                    this.PlusButton.Visibility = Visibility.Visible;
                    this.FilterGrid.Visibility = Visibility.Visible;
                    this.MonthFilterPanel.Visibility = Visibility.Visible;
                    this.WeekFilterPanel.Visibility = Visibility.Hidden;
                    this.DayFilterPanel.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    this.MenuCell2.Background = Brushes.CornflowerBlue;
                    this.PlusButton.Visibility = Visibility.Visible;
                    this.FilterGrid.Visibility = Visibility.Visible;
                    this.MonthFilterPanel.Visibility = Visibility.Hidden;
                    this.WeekFilterPanel.Visibility = Visibility.Visible;
                    this.DayFilterPanel.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    this.MenuCell3.Background = Brushes.CornflowerBlue;
                    this.PlusButton.Visibility = Visibility.Visible;
                    this.FilterGrid.Visibility = Visibility.Visible;
                    this.MonthFilterPanel.Visibility = Visibility.Hidden;
                    this.WeekFilterPanel.Visibility = Visibility.Hidden;
                    this.DayFilterPanel.Visibility = Visibility.Visible;
                    break;
                case 4:
                    this.MenuCell4.Background = Brushes.IndianRed;
                    this.PlusButton.Visibility = Visibility.Hidden;
                    this.FilterGrid.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    this.MenuCell5.Background = Brushes.ForestGreen;
                    this.PlusButton.Visibility = Visibility.Hidden;
                    this.FilterGrid.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void PlusButton_click(object sender, RoutedEventArgs e)
        {
            //создаю новый объект цели для добавления
            ((MainViewModel)this.DataContext).SetNewSelectedTarget();
            ShowTargetWindow();
        }

        private void ShowTargetWindow()
        {
            TargetWindow TW = new TargetWindow();
            TW.Owner = this;
            TW.YearFilterPanel.Visibility = this.YearFilterPanel.Visibility;
            TW.MonthFilterPanel.Visibility = this.MonthFilterPanel.Visibility;
            TW.WeekFilterPanel.Visibility = this.WeekFilterPanel.Visibility;
            TW.DayFilterPanel.Visibility = this.DayFilterPanel.Visibility;
            TW.DataContext = this.DataContext;
            TW.ShowDialog();
        }
    }
}
