using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tetris.GUI
{
    public interface ITetrisPage
    {
        void Page_KeyDown(object sender, KeyEventArgs e);
    }
}
