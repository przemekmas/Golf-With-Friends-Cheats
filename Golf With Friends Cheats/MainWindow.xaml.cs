using System;
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

        public MainWindow()
        {
            InitializeComponent();
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
                var offsets = new[] { 0x2d0, 0xb8, 0x478, 0x140, 0x80 };
                var address = 0x02ABFD70;
                var process = new CurrentProcess(proccessName, moduleName);
                var memory = new Memory(process, _nubmerOfBytes);
                memory.SetMemoryAddress(address, offsets);
                memory.SetDataForMultiLevelPointers();
                var bytes = BitConverter.GetBytes((long)value);
                memory.WriteData(bytes);
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
    }
}