using System;
using System.Windows;

namespace Golf_With_Friends_Cheats.Controls
{
    public class EnableJetpackControl : SetMemoryValueControl
    {
        public override int Address => 0x027FF298;

        public override int[] Offsets => new int[] { 0x620, 0x640, 0x210, 0x2b8, 0x38 };

        public override void ExecuteAction(string value)
        {
            if (byte.TryParse(value, out byte result))
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
