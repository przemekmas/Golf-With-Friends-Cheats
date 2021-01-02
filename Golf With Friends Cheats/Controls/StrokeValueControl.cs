using System;
using System.Windows;

namespace Golf_With_Friends_Cheats.Controls
{
    public class StrokeValueControl : SetMemoryValueControl
    {
        public override int Address => 0x02ABFD70;

        public override int[] Offsets => new int[] { 0x2d0, 0xb8, 0x478, 0x140, 0x80 };

        public override void ExecuteAction(string value)
        {
            if (long.TryParse(value, out long result))
            {
                var bytes = BitConverter.GetBytes(result);
                MemoryOperations.Instance.WriteBytesToMemory(Address, Offsets, bytes);
            }
        }

        protected override void ControlLoaded(object sender, RoutedEventArgs e)
        {
            DefaultValue = "1";
        }
    }
}
