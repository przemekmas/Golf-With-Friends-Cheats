using System.Diagnostics;

namespace Golf_With_Friends_Cheats
{
    public class CurrentProcess
    {
        public Process Process { get; set; }
        public ProcessModule CurrentModule { get; set; }
        public long ModuleBaseAddress 
        { 
            get
            {
                return CurrentModule.BaseAddress.ToInt64();
            }
        }

        public CurrentProcess(string name, string targetModule)
        {
            Process = Process.GetProcessesByName(name)[0];
            SetTargetModuleByName(targetModule);
        }

        private void SetTargetModuleByName(string name)
        {
            foreach (var module in Process.Modules)
            {
                // Module where static pointer is located
                if (((ProcessModule)module).ModuleName.EndsWith(name))
                {
                    CurrentModule = (ProcessModule)module;
                    break;
                }
            }
        }
    }
}
