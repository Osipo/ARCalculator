using ARCalc.Resources;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CalcManager _manager;
        public CalcManager CalcManager { get { return _manager; } }
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += KeyBoard_Click;
            _manager = new CalcManager(this);
            
        }
        private void KeyBoard_Click(object sender, KeyEventArgs args)
        {
            

            Key i = args.Key;
            char valid_ch = i switch
            {
                Key.D0 when args.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) => ')',
                Key.D0 => '0',
                Key.D1 => '1',
                Key.D2 => '2',
                Key.D3 => '3',
                Key.D4 => '4',
                Key.D5 => '5',
                Key.D6 => '6',
                Key.D7 => '7',
                Key.D8 when args.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) => '*',
                Key.D8 => '8',
                Key.D9 when args.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) => '(',
                Key.D9 => '9',
                Key.NumPad0 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '0',
                Key.NumPad1 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '1',
                Key.NumPad2 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '2',
                Key.NumPad3 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '3',
                Key.NumPad4 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '4',
                Key.NumPad5 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '5',
                Key.NumPad6 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '6',
                Key.NumPad7 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '7',
                Key.NumPad8 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '8',
                Key.NumPad9 when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '9',
                Key.Multiply => '*',
                Key.Add => '+',
                Key.Subtract => '-',
                Key.Divide => '/',
                Key.OemQuestion => '/',
                Key.OemComma => '.',
                Key.Delete when args.KeyboardDevice.IsKeyToggled(Key.NumLock) => '.',
                Key.OemPeriod => '.',
                Key.OemMinus => '-',
                Key.OemPlus when args.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) => '=',
                Key.Enter => '=',
                Key.OemPlus => '+',
                Key.C => 'C',
                Key.Space => ' ',
                Key.Back => 'B',
                _ => '!'
            };
            if (valid_ch != '!')
            {
                Console.WriteLine($"Key pressed: {valid_ch}");
                string input = new string(valid_ch, 1);
                _manager.Manage(input);
            }
        }

        /*
        private void INP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int pos,  sel;
            if (sender is TextBox tb)
            {
                pos = tb.SelectionStart;
                sel = tb.SelectionLength;
                _manager?.Manage(tb.Text); // MUST ALSO PROCESS POSITION
                e.Handled = true;
            }
        }*/
    }
}
