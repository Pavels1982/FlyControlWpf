using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FlyControlWPF.Services
{
    public class GMapService : INotifyPropertyChanged
    {
        #region Variables
        /// <summary>
        /// Имплементация паттерна Singlton. Представляет единичный экземпляр класса <see cref="GMapService"/>.
        /// </summary>
        private static GMapService instance;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        /// <summary>
        /// Get or set ссылку на объект GMapControl.
        /// </summary>
        public GMap.NET.WindowsPresentation.GMapControl Gmap { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод для получения объекта модели данных карты (реализация синглтона).
        /// </summary>
        /// <returns>Объект модели данных карты.</returns>
        public static GMapService GetInstance()
        {
            if (instance == null)
            {
                instance = new GMapService();
                initialGmap();
            }
            return instance;
        }

        public void GetLocation(string name)
        {
            instance.Gmap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            instance.Gmap.SetPositionByKeywords(name);
            instance.Gmap.Zoom = 16;
            instance.Gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
        }
        #endregion


        protected static void initialGmap()
        {
            instance.Gmap = new GMap.NET.WindowsPresentation.GMapControl();
            instance.Gmap.Loaded += Gmap_Loaded;
        }


        private static void Gmap_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            GMap.NET.GMaps.Instance.BoostCacheEngine = false;
            instance.Gmap.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            instance.Gmap.MinZoom = 5;
            instance.Gmap.MaxZoom = 16;
            instance.Gmap.Zoom = 5;
            instance.Gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            instance.Gmap.CanDragMap = true;
            instance.Gmap.DragButton = MouseButton.Left;
            instance.Gmap.CenterCrossPen = new Pen() { Brush = Brushes.Transparent };
        }

    }
}
