using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris.GUI
{
    /// <summary>
    /// Defines a color theme for the Tetris blocks
    /// </summary>
    public class Theme
    {
        /*
         * Key for blocks:
         * 0 - Background block/empty space
         * 1 - I piece
         * 2 - O piece
         * 3 - L piece
         * 4 - J piece
         * 5 - S piece
         * 6 - Z piece
         * 7 - T piece
         * 8 - Ghost piece
         * 9 - Piece area border blocks (the sides)
         */

        /// <summary>
        /// The brushes to use for filling the blocks
        /// </summary>
        public Brush[] FillBrushes { get; } = new Brush[10];
        /// <summary>
        /// The brushes to use for the borders of the blockes
        /// </summary>
        public Brush[] BorderBrushes { get; } = new Brush[10];
    }

    /// <summary>
    /// A container class containing builtin themes to use
    /// </summary>
    public static class Themes
    {
        public static Theme Classic { get; private set; } = new Theme();
        public static Theme Water { get; private set; } = new Theme();
        public static Theme XRay { get; private set; } = new Theme();
        public static Theme ClassicGradiant { get; private set; } = new Theme();
        public static Theme VladimClassic { get; private set; } = new Theme();
        public static Theme PlainText { get; private set; } = new Theme();

        static Themes() {
            Classic.FillBrushes[0] = new SolidColorBrush(Colors.Black);
            Classic.FillBrushes[1] = new SolidColorBrush(Colors.Cyan);
            Classic.FillBrushes[2] = new SolidColorBrush(Colors.Yellow);
            Classic.FillBrushes[3] = new SolidColorBrush(Colors.DodgerBlue);
            Classic.FillBrushes[4] = new SolidColorBrush(Colors.DarkOrange);
            Classic.FillBrushes[5] = new SolidColorBrush(Colors.LawnGreen);
            Classic.FillBrushes[6] = new SolidColorBrush(Colors.Red);
            Classic.FillBrushes[7] = new SolidColorBrush(Colors.MediumPurple);
            Classic.FillBrushes[8] = new SolidColorBrush(Colors.LightGray);
            Classic.FillBrushes[9] = new SolidColorBrush(Colors.Gray);
            Classic.BorderBrushes[0] = new SolidColorBrush(Colors.Black);
            Classic.BorderBrushes[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
            Classic.BorderBrushes[2] = new SolidColorBrush(Color.FromRgb(200, 200, 16));
            Classic.BorderBrushes[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            Classic.BorderBrushes[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            Classic.BorderBrushes[5] = new SolidColorBrush(Color.FromRgb(58, 197, 15));
            Classic.BorderBrushes[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            Classic.BorderBrushes[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            Classic.BorderBrushes[8] = new SolidColorBrush(Colors.Gray);
            Classic.BorderBrushes[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));

            Water.FillBrushes[0] = new SolidColorBrush(Colors.Black);
            Water.FillBrushes[1] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-cyan.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[2] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-yellow.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[3] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-blue.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[4] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-orange.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[5] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-green.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[6] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-red.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[7] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-purple.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[8] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-grey.jpg", UriKind.RelativeOrAbsolute)));
            Water.FillBrushes[9] = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Tetris.GUI;component/AltTextures/water-dgrey.jpg", UriKind.RelativeOrAbsolute)));
            Water.BorderBrushes[0] = new SolidColorBrush(Colors.Black);
            Water.BorderBrushes[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
            Water.BorderBrushes[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
            Water.BorderBrushes[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            Water.BorderBrushes[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            Water.BorderBrushes[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
            Water.BorderBrushes[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            Water.BorderBrushes[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            Water.BorderBrushes[8] = new SolidColorBrush(Colors.Gray);
            Water.BorderBrushes[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));

            XRay.FillBrushes[0] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[1] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[2] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[3] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[4] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[5] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[6] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[7] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[8] = new SolidColorBrush(Colors.Black);
            XRay.FillBrushes[9] = new SolidColorBrush(Colors.Black);
            XRay.BorderBrushes[0] = new SolidColorBrush(Colors.Black);
            XRay.BorderBrushes[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
            XRay.BorderBrushes[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
            XRay.BorderBrushes[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            XRay.BorderBrushes[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            XRay.BorderBrushes[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
            XRay.BorderBrushes[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            XRay.BorderBrushes[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            XRay.BorderBrushes[8] = new SolidColorBrush(Colors.Gray);
            XRay.BorderBrushes[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));

            LinearGradientBrush[] brushes = new LinearGradientBrush[10];
            ClassicGradiant.FillBrushes[0] = new SolidColorBrush(Colors.Black);
            brushes[1] = new LinearGradientBrush();
            brushes[1].GradientStops.Add(new GradientStop(Colors.Cyan, 0.0));
            brushes[1].GradientStops.Add(new GradientStop(Colors.DarkCyan, 1.0));
            ClassicGradiant.FillBrushes[1] = brushes[1];
            brushes[2] = new LinearGradientBrush();
            brushes[2].GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            brushes[2].GradientStops.Add(new GradientStop(Colors.DarkGoldenrod, 1.0));
            ClassicGradiant.FillBrushes[2] = brushes[2];
            brushes[3] = new LinearGradientBrush();
            brushes[3].GradientStops.Add(new GradientStop(Colors.DodgerBlue, 0.0));
            brushes[3].GradientStops.Add(new GradientStop(Colors.Blue, 1.0));
            ClassicGradiant.FillBrushes[3] = brushes[3];
            brushes[4] = new LinearGradientBrush();
            brushes[4].GradientStops.Add(new GradientStop(Colors.Orange, 0.0));
            brushes[4].GradientStops.Add(new GradientStop(Colors.OrangeRed, 1.0));
            ClassicGradiant.FillBrushes[4] = brushes[4];
            brushes[5] = new LinearGradientBrush();
            brushes[5].GradientStops.Add(new GradientStop(Colors.LawnGreen, 0.0));
            brushes[5].GradientStops.Add(new GradientStop(Colors.Olive, 1.0));
            ClassicGradiant.FillBrushes[5] = brushes[5];
            brushes[6] = new LinearGradientBrush();
            brushes[6].GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            brushes[6].GradientStops.Add(new GradientStop(Colors.Maroon, 1.0));
            ClassicGradiant.FillBrushes[6] = brushes[6];
            brushes[7] = new LinearGradientBrush();
            brushes[7].GradientStops.Add(new GradientStop(Colors.MediumPurple, 0.0));
            brushes[7].GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
            ClassicGradiant.FillBrushes[7] = brushes[7];
            brushes[8] = new LinearGradientBrush();
            brushes[8].GradientStops.Add(new GradientStop(Colors.LightGray, 0.0));
            brushes[8].GradientStops.Add(new GradientStop(Colors.GhostWhite, 1.0));
            ClassicGradiant.FillBrushes[8] = brushes[8];
            brushes[9] = new LinearGradientBrush();
            brushes[9].GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            brushes[9].GradientStops.Add(new GradientStop(Colors.DarkGray, 1.0));
            ClassicGradiant.FillBrushes[9] = brushes[9];
            ClassicGradiant.BorderBrushes[0] = new SolidColorBrush(Colors.Black);
            ClassicGradiant.BorderBrushes[1] = new SolidColorBrush(Color.FromRgb(16, 200, 200));
            ClassicGradiant.BorderBrushes[2] = new SolidColorBrush(Color.FromRgb(150, 150, 6));
            ClassicGradiant.BorderBrushes[3] = new SolidColorBrush(Color.FromRgb(26, 48, 202));
            ClassicGradiant.BorderBrushes[4] = new SolidColorBrush(Color.FromRgb(230, 93, 46));
            ClassicGradiant.BorderBrushes[5] = new SolidColorBrush(Color.FromRgb(28, 167, 5));
            ClassicGradiant.BorderBrushes[6] = new SolidColorBrush(Color.FromRgb(200, 16, 16));
            ClassicGradiant.BorderBrushes[7] = new SolidColorBrush(Color.FromRgb(92, 37, 92));
            ClassicGradiant.BorderBrushes[8] = new SolidColorBrush(Colors.Gray);
            ClassicGradiant.BorderBrushes[9] = new SolidColorBrush(Color.FromRgb(67, 52, 52));

            VladimClassic.FillBrushes[0] = Brushes.Black;
            VladimClassic.FillBrushes[1] = Brushes.Maroon;
            VladimClassic.FillBrushes[2] = Brushes.Navy;
            VladimClassic.FillBrushes[3] = Brushes.Purple;
            VladimClassic.FillBrushes[4] = Brushes.Silver;
            VladimClassic.FillBrushes[5] = Brushes.DarkGreen;
            VladimClassic.FillBrushes[6] = Brushes.Teal;
            VladimClassic.FillBrushes[7] = Brushes.Brown;
            VladimClassic.FillBrushes[8] = Brushes.GhostWhite;
            VladimClassic.FillBrushes[9] = Brushes.BlueViolet;
            VladimClassic.BorderBrushes[0] = Brushes.Black;
            VladimClassic.BorderBrushes[1] = Brushes.Maroon;
            VladimClassic.BorderBrushes[2] = Brushes.Navy;
            VladimClassic.BorderBrushes[3] = Brushes.Purple;
            VladimClassic.BorderBrushes[4] = Brushes.Silver;
            VladimClassic.BorderBrushes[5] = Brushes.DarkGreen;
            VladimClassic.BorderBrushes[6] = Brushes.Teal;
            VladimClassic.BorderBrushes[7] = Brushes.Brown;
            VladimClassic.BorderBrushes[8] = Brushes.GhostWhite;
            VladimClassic.BorderBrushes[9] = Brushes.BlueViolet;

            PlainText.FillBrushes[0] = Brushes.Black;
            PlainText.FillBrushes[1] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "I", Background = Brushes.Black } };
            PlainText.FillBrushes[2] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "O", Background = Brushes.Black } };
            PlainText.FillBrushes[3] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "L", Background = Brushes.Black } };
            PlainText.FillBrushes[4] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "J", Background = Brushes.Black } };
            PlainText.FillBrushes[5] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "S", Background = Brushes.Black } };
            PlainText.FillBrushes[6] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "Z", Background = Brushes.Black } };
            PlainText.FillBrushes[7] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "T", Background = Brushes.Black } };
            PlainText.FillBrushes[8] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "G", Background = Brushes.Black } };
            PlainText.FillBrushes[9] = new VisualBrush() { Visual = new Label() { FontSize = 16, Foreground = new SolidColorBrush(Color.FromRgb(0,255,0)), Content = "B", Background = Brushes.Black } };
            PlainText.BorderBrushes[0] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[1] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[2] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[3] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[4] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[5] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[6] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[7] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[8] = new SolidColorBrush(Color.FromArgb(64, 0, 255, 0));
            PlainText.BorderBrushes[9] = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));


        }
    }
}
