using FlyControlWPF.Services;
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
    /// Логика взаимодействия для GmapControl.xaml
    /// </summary>
    public partial class GmapControl : UserControl
    {
        public GmapControl()
        {
            InitializeComponent();
            MainGrid.Children.Add(GMapService.GetInstance().Gmap);
        }
    }
}
