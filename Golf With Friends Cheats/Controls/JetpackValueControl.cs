using System;
using System.Windows;

namespace Golf_With_Friends_Cheats.Controls
{
    public class JetpackValueControl : SetMemoryValueControl
    {
        public override int Address => 0x027B4DC8;

        public override int[] Offsets => new[] { 0x440, 0x230, 0x230, 0x640, 0x210, 0x2b8, 0x68 };

        public override void ExecuteAction(string value)
        {
            if (float.TryParse(value, out float result))
            {
                var bytes = BitConverter.GetBytes(result);
                MemoryOperations.Instance.WriteBytesToMemory(Address, Offsets, bytes);
            }
        }

        protected override void ControlLoaded(object sender, RoutedEventArgs e)
        {
            DefaultValue = "100";
        }
    }
}
