using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FlyControlWPF.Models
{
    public class BoxModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Get or set визуальную модель.
        /// </summary>
        public BoxVisual3D VisualModel { get; protected set; } = new BoxVisual3D();

        public BillboardTextVisual3D Text { get; set; } = new BillboardTextVisual3D();

        /// <summary>
        /// Get or set цвет.
        /// </summary>
        public Color Color { get; set; }

        private string name;
        /// <summary>
        /// Get or set имя.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                VisualModel.Children.Remove(Text);
                Text.Text = value;
                Text.Foreground = Brushes.White;
                Text.DepthOffset = 1e-3;
                this.VisualModel.Children.Add(Text);
                this.VisualModel.SetName(this.Name);
            }
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Конструктор класса по умолчанию
        /// </summary>
        /// <param name="color"></param>
        public BoxModel(Color color)
        {
            this.Name = this.GetHashCode().ToString();
            this.Color = color;
            this.VisualModel.Transform = new ScaleTransform3D(new Vector3D(100, 100, 100));
            SetColor(this.Color);

        }

        /// <summary>
        /// Метод применение цвета на визульную модель.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(50, color.R, color.G, color.B);
            this.VisualModel.Fill = brush;
        }
    }
}
