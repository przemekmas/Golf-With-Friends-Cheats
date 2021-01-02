using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Golf_With_Friends_Cheats.Controls
{
    [TemplatePart(Name = "PART_SelectItemComboBox", Type = typeof(ComboBox))]
    public class ChangeHoleValueControl : SetMemoryValueControl
    {
        private Dictionary<string, int> _holes;
        private ComboBox _selectItemComboBox;

        public override int Address => 0x02AB7338;

        public override int[] Offsets => new[] { 0xb8, 0x28, 0x78, 0x168, 0xa8, 0x88, 
            _holes.ElementAt(_selectItemComboBox.SelectedIndex).Value };

        public override void ExecuteAction(string value)
        {
            if (long.TryParse(value, out long result))
            {
                var bytes = BitConverter.GetBytes(result);
                MemoryOperations.Instance.WriteBytesToMemory(Address, Offsets, bytes);
            }
        }

        static ChangeHoleValueControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangeHoleValueControl), new FrameworkPropertyMetadata(typeof(ChangeHoleValueControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _selectItemComboBox = (ComboBox)Template.FindName("PART_SelectItemComboBox", this);
            LoadHoles();
            _selectItemComboBox.ItemsSource = _holes;
        }

        protected override void ControlLoaded(object sender, RoutedEventArgs e)
        {
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
    }
}
