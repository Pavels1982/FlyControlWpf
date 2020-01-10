using FlyControlWPF.ViewModels;
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

namespace FlyControlWPF.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для ViewPortControl.xaml
    /// </summary>
    public partial class ViewPortControl : UserControl
    {
        public ViewPortControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Add((DataContext as ViewPortViewModel).ViewPortModel);
        }
    }
}
