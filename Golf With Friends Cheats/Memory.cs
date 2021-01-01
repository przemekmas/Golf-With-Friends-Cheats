using System;
using System.Runtime.InteropServices;

namespace Golf_With_Friends_Cheats
{
    public class Memory : IDisposable
    {
        public object CurrentValue { get; set; }
        public long CurrentAddress { get; set; }
        private int _address;
        private int[] _offsets;
        private readonly int _numberOfBytes;
        private readonly CurrentProcess _process;
        private bool _disposed;

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize,
          out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr dwSize,
            out UIntPtr lpNumberOfBytesWritten);

        public Memory(CurrentProcess process, int numberOfBytes)
        {
            _numberOfBytes = numberOfBytes;
            _process = process;
        }

        public void SetMemoryAddress(int address, int[] offsets)
        {
            _address = address;
            _offsets = offsets;
        }

        public void SetDataForMultiLevelPointers()
        {
            var value = ReadValueFromMemory(_process.Process.Handle, 
                new IntPtr(_process.ModuleBaseAddress + _address), _numberOfBytes, out IntPtr bytesRead);

            long currentMemoryLocation = 0;

            foreach (var offset in _offsets)
            {
                var pointer = new IntPtr(value + offset);
                currentMemoryLocation = pointer.ToInt64();
                value = ReadValueFromMemory(_process.Process.Handle, pointer, _numberOfBytes,
                    out bytesRead);
            }
            CurrentValue = value;
            CurrentAddress = currentMemoryLocation;
        }

        public void WriteData(byte[] bytes)
        {
            WriteProcessMemory(_process.Process.Handle, new IntPtr(CurrentAddress), bytes, new UIntPtr((ulong)_numberOfBytes),
                out UIntPtr uIntPtr);
        }

        private long ReadValueFromMemory(IntPtr processHandle, IntPtr moduleAddress,
           int size, out IntPtr bytesRead)
        {
            var buffer = new byte[size];

            ReadProcessMemory(processHandle, moduleAddress, buffer, size, out bytesRead);

            // int 64 is used to store the value of the number of strokes as this is a 64 bit application
            var value = BitConverter.ToInt64(buffer, 0);

            return value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _process.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
