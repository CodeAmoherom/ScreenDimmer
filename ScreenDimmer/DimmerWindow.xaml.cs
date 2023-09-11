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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenDimmer
{
    /// <summary>
    /// Interaction logic for DimmerWindow.xaml
    /// </summary>
    public partial class DimmerWindow : Window
    {
        public DimmerWindow()
        {
            InitializeComponent();
        }
        public void ChangeDimmer(Color color)
        {
            Shade.Background = new SolidColorBrush(color);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        
    }
}
