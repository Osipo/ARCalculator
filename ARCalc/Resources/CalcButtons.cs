using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;

namespace ARCalc.Resources
{
    /// <summary>
    /// A part of interaction logic of MainWindow.
    /// Contains common logic for all buttons of MainWindow
    /// </summary>
    public partial class CalcButtons 
    {
        private readonly MainWindow _main;
        public CalcButtons()
        {
            InitializeComponent();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    _main = window as MainWindow;
                    break;
                }
            }
        }
        private void Button_Click(object sender, EventArgs args)
        {
            if (sender is Button b && b.Content is String txt)
            {
                _main.CalcManager.Manage(txt);
            }
        }

    }
}
