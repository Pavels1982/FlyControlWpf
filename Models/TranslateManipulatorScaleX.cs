﻿using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FlyControlWPF.Models
{
    public class TranslateManipulatorScaleX : TranslateManipulator, INotifyPropertyChanged
    {
        private bool isPan;
        private double lastPosition;
        public TranslateManipulator manipulator { get; set; }
        private BoxVisual3D childrenObject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TranslateManipulatorScaleX(BoxVisual3D boxVisual3D)
        {
            this.childrenObject = boxVisual3D;
            this.manipulator = new TranslateManipulator();
            manipulator.Color = Colors.Yellow;
            manipulator.Length = (boxVisual3D.Length / 2f) + 2f;
            manipulator.Direction = new Vector3D(1, 0, 0);
            manipulator.Diameter = 0.3;
            manipulator.MouseLeftButtonDown += Manipulator_MouseLeftButtonDown;
            manipulator.MouseLeftButtonUp += Manipulator_MouseLeftButtonUp;
            manipulator.MouseMove += Manipulator_MouseMove;
            manipulator.Bind(childrenObject);
        }

        private void Manipulator_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isPan)
            {
                Matrix3D matrix = childrenObject.GetTransform();
                if (matrix.OffsetX > lastPosition)
                {
                    this.childrenObject.Length += 0.1;
                    manipulator.Length += 0.05;
                }
                else
                {
                    this.childrenObject.Length -= 0.1;
                    manipulator.Length -= 0.05;
                }
                lastPosition = matrix.OffsetX;
            }
        }

        private void Manipulator_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPan = false;
        }

        private void Manipulator_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPan = true;
        }
    }
}
