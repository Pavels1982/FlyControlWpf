using FlyControlWPF.Models;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace FlyControlWPF.ViewModels
{
    public class ViewPortViewModel : INotifyPropertyChanged
    {
        public HelixViewport3D ViewPortModel { get; set; }
        private DispatcherTimer timer { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };
        private int toolBoxIndex = -1;
        public int ToolBoxIndex
        {
            get
            {
                return toolBoxIndex;
            }

            set
            {
                toolBoxIndex = value;
                CheckManipulatorVisible();
            }
        }
        

        public Models.TerrainModel Terrain { get; set; }
       
        public ViewPortViewModel()
        {
            initialHelix3DViewPort();
            timer.Tick += Timer_Tick;
            timer.Start();
           

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ViewPortModel.Children.Where(o => o.GetName() == "Aircraft").ToList().ForEach(el =>
            {

                Matrix3D aircraftPosition = (el as ModelVisual3D).Children[0].GetTransform();


                ViewPortModel.Children.Where(o => o.GetType().Equals(typeof(BoxVisual3D))).ToList().ForEach(box =>
                {
                    Matrix3D boxPosition = box.GetTransform();
                    Rect3D boxBounds = (box as BoxVisual3D).Model.Bounds;

                    Point[] p = new Point[4];
                    p[0] = new Point(boxPosition.OffsetX - (boxBounds.SizeX * 100) / 2, boxPosition.OffsetY + (boxBounds.SizeY * 100) / 2);
                    p[1] = new Point(boxPosition.OffsetX - (boxBounds.SizeX * 100) / 2, boxPosition.OffsetY - (boxBounds.SizeY * 100) / 2);
                    p[2] = new Point(boxPosition.OffsetX + (boxBounds.SizeX * 100) / 2, boxPosition.OffsetY + (boxBounds.SizeY * 100) / 2);
                    p[3] = new Point(boxPosition.OffsetX + (boxBounds.SizeX * 100) / 2, boxPosition.OffsetY - (boxBounds.SizeY * 100) / 2);

                    bool xy = pnpoly(4, p.Select(point => (float)point.X).ToArray(), p.Select(point => (float)point.Y).ToArray(), (float)aircraftPosition.OffsetX, (float)aircraftPosition.OffsetY);

                    Point[] p2 = new Point[4];
                    p2[0] = new Point(boxPosition.OffsetX - (boxBounds.SizeX * 100) / 2, boxPosition.OffsetZ + (boxBounds.SizeZ * 100) / 2);
                    p2[1] = new Point(boxPosition.OffsetX - (boxBounds.SizeX * 100) / 2, boxPosition.OffsetZ - (boxBounds.SizeZ * 100) / 2);
                    p2[2] = new Point(boxPosition.OffsetX + (boxBounds.SizeX * 100) / 2, boxPosition.OffsetZ + (boxBounds.SizeZ * 100) / 2);
                    p2[3] = new Point(boxPosition.OffsetX + (boxBounds.SizeX * 100) / 2, boxPosition.OffsetZ - (boxBounds.SizeZ * 100) / 2);

                    bool xz = pnpoly(4, p2.Select(point => (float)point.X).ToArray(), p2.Select(point => (float)point.Y).ToArray(), (float)aircraftPosition.OffsetX, (float)aircraftPosition.OffsetZ);

                    if (xy && xz) Debug.WriteLine("Collision");

                });


            }
            );

        }

        private bool pnpoly(int npol, float[] xp, float[] yp, float x, float y)
        {
            bool c = false;
            for (int i = 0, j = npol - 1; i < npol; j = i++)
            {
                if ((((yp[i] <= y) && (y < yp[j])) || ((yp[j] <= y) && (y < yp[i]))) &&
                  (((yp[j] - yp[i]) != 0) && (x > ((xp[j] - xp[i]) * (y - yp[i]) / (yp[j] - yp[i]) + xp[i]))))
                    c = !c;
            }
            return c;
        }


        //private double angle;
        //private void AnimateTimer_Tick(object sender, EventArgs e)
        //{
        //    angle += 0.1;
        //    if (angle > 360) angle = 0;

        //    ViewPortModel.Children.Where(o => o.GetName() == "Aircraft").ToList().ForEach(el => 
        //    {
        //        Matrix3D matrix = el.GetTransform();

        //        // el.GetTransform().RotatePrepend(new Quaternion(1, 0, 0, 1));

        //        Transform3DGroup group = new Transform3DGroup();
        //        group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle)));
        //        group.Children.Add(new TranslateTransform3D(new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ)));
        //        el.Transform = group;



        //    });


        //}

        private void initialHelix3DViewPort()
        {
            ViewPortModel = new HelixViewport3D();
            ViewPortModel.ZoomExtentsWhenLoaded = true;
            var lights = new DefaultLights() ;
            ViewPortModel.Children.Add(lights);

            Terrain = new Models.TerrainModel();

            ViewPortModel.Children.Add(Terrain.Model3D);

        }
        public void CheckManipulatorVisible()
        {
            ClearManipulator();
            switch (ToolBoxIndex)
            {
                case 1:
                    ViewPortModel.Children.ToList().ForEach(e =>
                    {
                        if (e.GetType().Equals(typeof(BoxVisual3D)))
                        {
                            //CombinedManipulator manipulator = new CombinedManipulator();
                            //manipulator.Offset = new Vector3D(0, 0, (e as BoxVisual3D).Height + 1);
                            //manipulator.Bind(e as BoxVisual3D);
                            //ViewPortModel.Children.Add(manipulator);
                            TranslateManipulator manipulatorZ = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(0, 0, 1), Length = ((e as BoxVisual3D).Height / 2f) + 2f };
                            manipulatorZ.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorZ);

                            TranslateManipulator manipulatorZ2 = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(0, 0, -1), Length = ((e as BoxVisual3D).Height / 2f) + 2f };
                            manipulatorZ2.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorZ2);


                            TranslateManipulator manipulatorX = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(0, 1, 0), Length = ((e as BoxVisual3D).Width / 2f) + 2f };
                            manipulatorX.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorX);
                            TranslateManipulator manipulatorX2 = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(0, -1, 0), Length = ((e as BoxVisual3D).Width / 2f) + 2f };
                            manipulatorX2.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorX2);

                            TranslateManipulator manipulatorY = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(1, 0, 0), Length = ((e as BoxVisual3D).Length / 2f) + 2f };
                            manipulatorY.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorY);
                            TranslateManipulator manipulatorY2 = new TranslateManipulator() { Color = Colors.Blue, Diameter = 0.3, Direction = new Vector3D(-1, 0, 0), Length = ((e as BoxVisual3D).Length / 2f) + 2f };
                            manipulatorY2.Bind((e as BoxVisual3D));
                            ViewPortModel.Children.Add(manipulatorY2);
                        }

                    });
                    break;

                case 2:
                    ViewPortModel.Children.ToList().ForEach(e =>
                    {
                        if (e.GetType().Equals(typeof(BoxVisual3D)))
                        {

                            ViewPortModel.Children.Add(new TranslateManipulatorScaleZ(e as BoxVisual3D).manipulator);
                            ViewPortModel.Children.Add(new TranslateManipulatorScaleX(e as BoxVisual3D).manipulator);
                            ViewPortModel.Children.Add(new TranslateManipulatorScaleY(e as BoxVisual3D).manipulator);
                        }

                    });
                    break;
            }

        }
        private void ClearManipulator()
        {
            while (ViewPortModel.Children.Any(e =>
                   e.GetType().Equals(typeof(TranslateManipulator)) ))
            {
                ViewPortModel.Children.Remove(ViewPortModel.Children.Where(e => e.GetType().Equals(typeof(TranslateManipulator)) ).First() as Visual3D);
            }

        }

        public void AddObject(BoxVisual3D additionModel)
        {
            ViewPortModel.Children.Add(additionModel);
            CheckManipulatorVisible();
        }


        public void AddAircraft(PlaneModel additionModel)
        {
            ViewPortModel.Children.Insert(0, additionModel.PlaneObject);
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
