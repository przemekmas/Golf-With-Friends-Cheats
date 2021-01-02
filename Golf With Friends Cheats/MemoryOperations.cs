namespace Golf_With_Friends_Cheats
{
    public class MemoryOperations
    {
        public static string ProcessName => "Golf With Your Friends";

        public static string ModuleName => "GameAssembly.dll";

        public static int NumberOfBytes => 8;

        private static MemoryOperations _memoryOperations;

        public static MemoryOperations Instance 
        {
            get
            {
                if (_memoryOperations == null)
                {
                    _memoryOperations = new MemoryOperations();
                }
                return _memoryOperations;
            }
        }

        public void WriteBytesToMemory(int address, int[] offsets, byte[] bytes)
        {
            using (var process = new CurrentProcess(ProcessName, ModuleName))
            using (var memory = new Memory(process, NumberOfBytes))
            {
                memory.SetMemoryAddress(address, offsets);
                memory.SetDataForMultiLevelPointers();
                memory.WriteData(bytes);
            }
        }
    }
}
