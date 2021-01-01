using Golf_With_Friends_Cheats.Exceptions;
using System;
using System.Diagnostics;

namespace Golf_With_Friends_Cheats
{
    public class CurrentProcess : IDisposable
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
        private bool _disposed;

        public CurrentProcess(string name, string targetModule)
        {
            var process = Process.GetProcessesByName(name);
            if (process.Length < 1)
            {
                throw new ProcessNotFoundException($"Process \"{name}\" cannot be found.");
            }

            Process = process[0];
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
            if (CurrentModule == null)
            {
                throw new ModuleNotFoundException($"Cannot find module \"{name}\".");
            }
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
                    Process.Dispose();
                    CurrentModule.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
