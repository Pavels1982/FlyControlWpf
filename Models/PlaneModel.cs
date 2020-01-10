using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using static System.Net.Mime.MediaTypeNames;

namespace FlyControlWPF.Models
{
    public class PlaneModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ModelVisual3D AirplaneModel { get; set; } = new ModelVisual3D();
        public int Speed { get; set; } = 200;

        private double rotateAngle;
        public double RotateAngle
        {
            get
            {
                return rotateAngle;
            }

            set
            {
                if (value > 360)
                {
                    rotateAngle = 0;
                }
                else 
                {
                    rotateAngle = value;
                }

                RotatePlaneObject(rotateAngle);
            }
        
        }

        public ModelVisual3D PlaneObject { get; set; } = new ModelVisual3D();

        public string Name { get; set; }

        public BillboardTextGroupVisual3D Label { get; set; } 

        public PlaneModel()
        {
            AirplaneModel.Content = GetAirplaneModel();
            AirplaneModel.Children.Add(Label);

            Random rnd = new Random();
            rotateAngle = rnd.Next(0, 360);

            PlaneObject.Children.Add(AirplaneModel);
            PlaneObject.Children[0].Transform = new TranslateTransform3D(new Vector3D(0, rnd.Next(200,1500), 0));
           
            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotateAngle)));
            group.Children.Add(new TranslateTransform3D(new Vector3D(rnd.Next(1000, 1500), rnd.Next(1000, 1500), rnd.Next(300, 800))));
            PlaneObject.Transform = group;
            PlaneObject.SetName("Aircraft");
    
        }

        private void RotatePlaneObject(double rotateAngle)
        {
            Matrix3D matrix = PlaneObject.GetTransform();
            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotateAngle)));
            group.Children.Add(new TranslateTransform3D(new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ)));
            PlaneObject.Transform = group;

        }


        public Model3DGroup GetAirplaneModel()
        {

            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup MyModel = CurrentHelixObjReader.Read(GetExeDirectory() + "\\Resources\\Models\\plane.obj");

            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 0)));
            group.Children.Add(new ScaleTransform3D(new Vector3D(0.05, 0.05, 0.05)));
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            MyModel.Transform = group;
            this.Name = "RA" + this.GetHashCode().ToString();
            Label = CreateBillboard(this.Name);
            MyModel.SetName(this.Name);
            return MyModel;
        }

        private BillboardTextGroupVisual3D CreateBillboard(string text)
        {
            BillboardTextGroupVisual3D billboard = new BillboardTextGroupVisual3D()
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                Foreground = Brushes.Black,
                BorderThickness = new System.Windows.Thickness(1),
                FontSize = 11,
                Offset = new System.Windows.Vector(30, 30),
                PinBrush = Brushes.Gray,
                IsEnabled = true
            };
            List <BillboardTextItem> TextItems3 = new List<BillboardTextItem>();
            TextItems3.Add(new BillboardTextItem { Text = text, Position = new Point3D(0.5, 0, 0.5), DepthOffset = 0, WorldDepthOffset = 0.2 });

            billboard.Items = TextItems3;


            return billboard;
        }


        static private string GetExeDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = System.IO.Path.GetDirectoryName(path);
            return path;
        }


    }
}
