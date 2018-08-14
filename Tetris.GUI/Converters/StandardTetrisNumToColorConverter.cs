using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Tetris.GUI.Converters
{
    public class StandardTetrisNumToColorConverter : IValueConverter
    {

        private SolidColorBrush[] colors = new SolidColorBrush[9];
        public StandardTetrisNumToColorConverter()
        {
            colors[0] = new SolidColorBrush(Colors.Black);
            colors[1] = new SolidColorBrush(Colors.Cyan);
            colors[2] = new SolidColorBrush(Colors.Yellow);
            colors[3] = new SolidColorBrush(Colors.Blue);
            colors[4] = new SolidColorBrush(Colors.Orange);
            colors[5] = new SolidColorBrush(Colors.LightGreen);
            colors[6] = new SolidColorBrush(Colors.Red);
            colors[7] = new SolidColorBrush(Colors.DarkMagenta);
            colors[8] = new SolidColorBrush(Colors.Gray);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush scb = null;
            if(targetType == typeof(Brush))
            {
                int num = (int)value;
                scb = colors[num];
            }
            return scb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
                SolidColorBrush brush = (SolidColorBrush)value;
                if(colors.Contains(brush))
                {
                    int index = -1;
                    for(int i = 0; i < colors.Count(); i++)
                    {
                        if(colors[i] == brush)
                        {
                            index = i;
                        }
                        if(index != -1)
                        {
                            return index;
                        }

                    }
                }
            return null;
        }
    }
}
