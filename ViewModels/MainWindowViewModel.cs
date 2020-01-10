using FlyControlWPF.Models;
using FlyControlWPF.Services;
using FlyControlWPF.Views;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace FlyControlWPF.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged

    {
        public ImageSource Image { get; set; }
        public string LocationName { get; set; }
        public GMapService GmapHelp { get; set; } = GMapService.GetInstance();
        public ObservableCollection<BoxModel> ListBox3D { get; set; } = new ObservableCollection<BoxModel>();
        public ObservableCollection<PlaneModel> ListPlane { get; set; } = new ObservableCollection<PlaneModel>();
        public PlaneModel SelectedPlane { get; set; }
        public BoxModel SelectedBox3D { get; set; }
        public ViewPortViewModel ViewPort { get; set; } = new ViewPortViewModel();
        private DispatcherTimer animateTimer { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };
        public ICommand SaveImage
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    try
                    {
                        ViewPort.Terrain.SetImage(GMapService.GetInstance().Gmap.ToImageSource());
                    }
                    catch{ }
                }
                );
            }
        }

        
        public ICommand AddAircraftCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    var newObj = new PlaneModel();
                    ListPlane.Add(newObj);
                    ViewPort.AddAircraft(newObj);
       
                });
            }
        }


        public ICommand AddBlueBoxCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    var newObj = new BoxModel(Colors.Blue);
                    ListBox3D.Add(newObj);
                    ViewPort.AddObject(newObj.VisualModel);

                });
            }
        }
        public ICommand AddRedBoxCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    var newObj = new BoxModel(Colors.Red);
                    ListBox3D.Add(newObj);
                    ViewPort.AddObject(newObj.VisualModel);
                });
            }
        }
        public ICommand DeleteObjectCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    ViewPort.ViewPortModel.Children.Remove(SelectedBox3D.VisualModel);
                    ListBox3D.Remove(SelectedBox3D);
                    ViewPort.CheckManipulatorVisible();

                });
            }
        }
        public ICommand SearchLocationCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    if (LocationName != null)
                    {
                        GMapService.GetInstance().GetLocation(LocationName);
                    }
                }
                );
            }
        }

        public ICommand DeleteAircraftCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    ViewPort.ViewPortModel.Children.Remove(SelectedPlane.PlaneObject);
                    ListPlane.Remove(SelectedPlane);
                   });
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            animateTimer.Tick += AnimateTimer_Tick;
            animateTimer.Start();
        }

        private void AnimateTimer_Tick(object sender, EventArgs e)
        {
            ListPlane.ToList().ForEach(o =>
            {
                o.RotateAngle+= 0.1f;

            });


        }
    }
}
