using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenDimmer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Screen SelectedScreen { get; set; }
        private DimmerWindow SelectedDimmer { get; set; }
        private int SelectedIndex { get; set; } = 0;
        private List<Border> ScreenButtons { get; set; } = new List<Border>();
        public MainWindow()
        {
            InitializeComponent();
            DetectScreens();
            Instances.Instance = this;
            this.Topmost = true;
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            Instances.Instance.ShowDialog();
        }

        private void DetectScreens()
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            int screenCount = 0;
            foreach (var screen in screens)
            {
                Border screenButton = new Border();
                screenButton.Tag = screens.ToList().IndexOf(screen);
                screenButton.BorderThickness = new Thickness(3);
                screenButton.BorderBrush = 
                    new SolidColorBrush(new Color() { A = 255, R = 0, G = 120, B = 212 });
                screenButton.CornerRadius = new CornerRadius(10);
                TextBlock textBlock = new TextBlock();
                textBlock.Foreground = 
                    new SolidColorBrush(Color.FromArgb((byte)255, (byte)255, (byte)255, (byte)255));
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


                ScreensPanel.Children.Add(screenButton);
                ScreenButtons.Add(screenButton);

                DimmerWindow screenDimmer = new DimmerWindow();
                screenDimmer.Show();
                screenDimmer.Left = screens[screenCount].WorkingArea.Left;
                screenDimmer.Top = screens[screenCount].WorkingArea.Top;
                screenDimmer.WindowState = WindowState.Normal;
                screenDimmer.WindowState = WindowState.Maximized;
                
                Instances.ScreenDimmers.Add(screenDimmer);
                screenCount++;
            }

            SelectedScreen = screens[0];

            ScreenButtons[0].Background =
                new SolidColorBrush(Color.FromArgb((byte)255, (byte)83, (byte)45, (byte)110));

            this.Topmost = true;
        }

        private void ScreenButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Screen selectedScreen = 
                (sender as Border).Tag as System.Windows.Forms.Screen;
            SelectScreen((sender as Border), selectedScreen);
        }

        private void ScreenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Border).BorderBrush = 
                new SolidColorBrush(Color.FromArgb((byte)255, (byte)0, (byte)0, (byte)0));

        }

        private void ScreenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Border).BorderBrush = 
                new SolidColorBrush(Color.FromArgb((byte)255, (byte)24, (byte)188, (byte)255));
        }

        private void Dimmer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int alpha = (int)e.NewValue;
            Instances.ScreenDimmers[SelectedIndex].ChangeDimmer(new Color() { A = (byte)alpha, R = 0, G = 0, B = 0 });
        }

        private void SelectScreen(Border screenButton, System.Windows.Forms.Screen screen)
        {
            foreach (Border border in ScreenButtons)
            {
                border.Background = 
                        new SolidColorBrush(Color.FromArgb((byte)255, (byte)0, (byte)0, (byte)0));
            }
            

            screenButton.Background =
                        new SolidColorBrush(Color.FromArgb((byte)255, (byte)83, (byte)45, (byte)110));
            SelectedIndex = (int)screenButton.Tag;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
