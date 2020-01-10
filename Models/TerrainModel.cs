using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace FlyControlWPF.Models
{
    public class TerrainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ModelVisual3D Model3D { get; set; } 
        public TerrainModel()
        {
            Model3D = CreateTerrainPlane();
        }

        public void SetImage(ImageSource image)
        {
            ImageBrush brush = new ImageBrush
            {
                ImageSource = image,
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
                ViewboxUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };

            brush.Viewport = new Rect(0, 0, brush.ImageSource.Width, brush.ImageSource.Height);
            DiffuseMaterial mat = new DiffuseMaterial(brush);
            (Model3D.Content as GeometryModel3D).Material = mat;
        }

        private ModelVisual3D CreateTerrainPlane()
        {
            var mb = new MeshBuilder(false, true);

            IList<Point3D> pnts = new List<Point3D>
            {
                new Point3D(0, 0, 0),
                new Point3D(3000, 0, 0),
                new Point3D(3000, 3000, 0),
                new Point3D(0, 3000, 0)
            };

            mb.AddPolygon(pnts);
            var mesh = mb.ToMesh(false);

            PointCollection pntCol = new PointCollection
            {
                new Point(0, 0),
                new Point(0, 3000),
                new Point(3000, 3000),
                new Point(3000, 0)
            };

            mesh.TextureCoordinates = pntCol;

            ImageBrush brush = new ImageBrush();
            DiffuseMaterial mat = new DiffuseMaterial(brush);
            GeometryModel3D geometryModel3D = new GeometryModel3D { Geometry = mesh, Material = mat };
           
            Model3D = new ModelVisual3D();
            Model3D.Content = geometryModel3D;

            return Model3D;
        }



    }
}
