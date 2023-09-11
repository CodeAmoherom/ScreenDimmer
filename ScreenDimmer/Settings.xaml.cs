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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScreenDimmer
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : System.Windows.Window
    {
        private System.Windows.Forms.Screen SelectedScreen {  get; set; }
        private DimmerWindow SelectedDimmer {  get; set; }
        public Settings()
        {
            InitializeComponent();
            DetectScreens();
        }

        private void DetectScreens()
        {
            var screens = System.Windows.Forms.Screen.AllScreens;

            foreach ( var screen in screens )
            {
                Border screenButton = new Border();
                screenButton.Tag = screen;
                screenButton.BorderThickness = new Thickness(3);
                screenButton.BorderBrush = new SolidColorBrush(new Color() { A = 255, R = 0, G = 120, B = 212 });
                screenButton.CornerRadius = new CornerRadius(10);
                TextBlock textBlock = new TextBlock();
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb((byte)255, (byte)255, (byte)255, (byte)255));
                textBlock.Text = screen.DeviceName.Replace("\\", "");
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                screenButton.Child = textBlock;
                screenButton.Width = 100;
                screenButton.Height = 60;
                screenButton.Margin = new Thickness(5);
                screenButton.MouseEnter += ScreenButton_MouseEnter;
                screenButton.MouseLeave += ScreenButton_MouseLeave;
                screenButton.MouseDown += ScreenButton_MouseDown;
                

                ScreensPanel.Children.Add( screenButton );

            }

            if (screens.Length > 0)
            {
                foreach(var screen in screens)
                {
                    DimmerWindow screenDimmer = new DimmerWindow();
                    screenDimmer.ShowDialog();
                    screenDimmer.Left = screen.Bounds.Left;
                    screenDimmer.Top = screen.Bounds.Top;
                    screenDimmer.WindowState = WindowState.Minimized;
                    Instances.ScreenDimmers.Add( screenDimmer );
                }
            }
            else
            {
                MessageBox.Show("Cannot Detect your Screen!", "Error");
            }

            SelectedScreen = screens[0];

            (ScreensPanel.Children[0] as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)3, (byte)4, (byte)20));
        }

        private void ScreenButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Screen selectedScreen = (sender as Border).Tag as System.Windows.Forms.Screen;
            SelectScreen((sender as Border), selectedScreen);
        }

        private void ScreenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)0, (byte)0, (byte)0));
            
        }

        private void ScreenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)0, (byte)13, (byte)54));
        }

        private void Dimmer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int alpha = (int)e.NewValue;
            SelectedDimmer.ChangeDimmer(new Color() { A = (byte)alpha, R=0, G=0, B = 0 });
        }

        private void SelectScreen(Border screenButton, System.Windows.Forms.Screen screen)
        {
            List<Border> ScreenButtons = new List<Border>((IEnumerable<Border>)ScreensPanel.Children);

            foreach(Border border in ScreenButtons)
            {
                if (border != screenButton)
                {
                    (ScreensPanel.Children[ScreenButtons.IndexOf(border)] as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)0, (byte)0, (byte)0));
                }
                else
                {
                    (ScreensPanel.Children[ScreenButtons.IndexOf(border)] as Border).Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)3, (byte)4, (byte)20));
                }
            }
        }
        
    }
}
