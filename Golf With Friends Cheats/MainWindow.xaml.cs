using Golf_With_Friends_Cheats.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Golf_With_Friends_Cheats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _nubmerOfBytes = 8; // 64 bit application = 8 bytes
        const string proccessName = "Golf With Your Friends";
        const string moduleName = "GameAssembly.dll";
        private ActiveTimer _activeStrokeTimer;
        private Dictionary<string, int> _holes;

        public MainWindow()
        {
            InitializeComponent();
            LoadHoles();
            SelectHoleComboBox.ItemsSource = _holes;
        }
        
        private void LoadHoles()
        {
            var val = 0x20;
            _holes = new Dictionary<string, int>();
            for (int i = 1; i <= 18; i++)
            {
                _holes.Add($"Hole {i}", val);
                val += 4;
            }
        }

        private void SetStrokeValue(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StrokeValue.Text, out int result))
            {
                SetStrokeValueInMemory(result);
            }
        }

        private void SetStrokeValueInMemory(int value)
        {
            try
            {
                using (var process = new CurrentProcess(proccessName, moduleName))
                using (var memory = new Memory(process, _nubmerOfBytes))
                {
                    var offsets = new[] { 0x2d0, 0xb8, 0x478, 0x140, 0x80 };
                    var address = 0x02ABFD70;
                    memory.SetMemoryAddress(address, offsets);
                    memory.SetDataForMultiLevelPointers();
                    var bytes = BitConverter.GetBytes((long)value);
                    memory.WriteData(bytes);
                }
            }
            catch (Exception exception)
            {
                _activeStrokeTimer?.Stop();
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnCheckActiveStrokeValue(object sender, RoutedEventArgs e)
        {
            _activeStrokeTimer = new ActiveTimer(() => 
            {
                Dispatcher.Invoke(() =>
                {
                    if (int.TryParse(StrokeValue.Text, out int result))
                    {
                        SetStrokeValueInMemory(result);
                    }
                });                
            }, GetIntervalValue());
            _activeStrokeTimer.Start();
        }

        private void OnUncheckActiveStrokeValue(object sender, RoutedEventArgs e)
        {
            _activeStrokeTimer.Stop();
        }

        private int GetIntervalValue()
        {
            if (int.TryParse(IntervalValue.Text, out int result))
            {
                return result;
            }
            return 0;
        }

        private void SetHoleValue(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(HoleValue.Text, out int result))
            {
                SetHoleValue(result);
            }
        }

        private void SetHoleValue(int value)
        {
            try
            {
                if (SelectHoleComboBox.SelectedIndex < 0)
                {
                    throw new HoleNotSelecteException("No hole selected. Please select hole before setting value");
                }
                using (var process = new CurrentProcess(proccessName, moduleName))
                using (var memory = new Memory(process, _nubmerOfBytes))
                {
                    var offsets = new[] { 0xb8, 0x28, 0x78, 0x168, 0xa8, 0x88, 
                        _holes.ElementAt(SelectHoleComboBox.SelectedIndex).Value };
                    var address = 0x02AB7338;
                    memory.SetMemoryAddress(address, offsets);
                    memory.SetDataForMultiLevelPointers();
                    var bytes = BitConverter.GetBytes((long)value);
                    memory.WriteData(bytes);
                }
            }
            catch (Exception exception)
            {
                _activeStrokeTimer?.Stop();
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}