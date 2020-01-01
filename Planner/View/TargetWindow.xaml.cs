using Planner.ViewModel;
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
using System.Windows.Shapes;

namespace Planner.View
{
    /// <summary>
    /// Логика взаимодействия для TargetWindow.xaml
    /// </summary>
    public partial class TargetWindow : Window
    {
        private BrushConverter brushConverter = new BrushConverter();

        public TargetWindow()
        {
            InitializeComponent();

            FirstExclamationPoint_button.MouseEnter += new MouseEventHandler(FirstExclamationPointRed);
            FirstExclamationPoint_button.MouseLeave += new MouseEventHandler(FirstExclamationPointDefault);
            SecondExclamationPoint_button.MouseEnter += new MouseEventHandler(SecondExclamationPointRed);
            SecondExclamationPoint_button.MouseLeave += new MouseEventHandler(SecondExclamationPointDefault);
            ThirdExclamationPoint_button.MouseEnter += new MouseEventHandler(ThirdExclamationPointRed);
            ThirdExclamationPoint_button.MouseLeave += new MouseEventHandler(ThirdExclamationPointDefault);

        }

        private void FirstExclamationPointRed(object sender, MouseEventArgs e)
        {
            FirstExclamationPoint_button.Foreground = Brushes.Red;
        }

        private void FirstExclamationPointDefault(object sender, MouseEventArgs e)
        {
            var brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).LowImportantButtonColor);
            FirstExclamationPoint_button.Foreground = brush;
        }

        private void SecondExclamationPointRed(object sender, MouseEventArgs e)
        {
            FirstExclamationPoint_button.Foreground = Brushes.Red;
            SecondExclamationPoint_button.Foreground = Brushes.Red;
        }

        private void SecondExclamationPointDefault(object sender, MouseEventArgs e)
        {
            var brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).LowImportantButtonColor);
            FirstExclamationPoint_button.Foreground = brush;
            brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).MiddleImportantButtonColor);
            SecondExclamationPoint_button.Foreground = brush;
        }

        private void ThirdExclamationPointRed(object sender, MouseEventArgs e)
        {
            FirstExclamationPoint_button.Foreground = Brushes.Red;
            SecondExclamationPoint_button.Foreground = Brushes.Red;
            ThirdExclamationPoint_button.Foreground = Brushes.Red;
        }

        private void ThirdExclamationPointDefault(object sender, MouseEventArgs e)
        {
            var brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).LowImportantButtonColor);
            FirstExclamationPoint_button.Foreground = brush;
            brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).MiddleImportantButtonColor);
            SecondExclamationPoint_button.Foreground = brush;
            brush = (Brush)brushConverter.ConvertFromString(((MainViewModel)this.DataContext).HighImportantButtonColor);
            ThirdExclamationPoint_button.Foreground = brush;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainViewModel context = (MainViewModel)this.DataContext;
            context.SetExclamationPointNull();
        }
    }
}
