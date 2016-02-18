using System.Diagnostics;

namespace restart
{
    class Program
    {
        static void Main(string[] args)
        {
            string procName = "";
            string exePath = "";
            for(int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-name"))
                {
                    i++;
                    procName = args[i];
                }
                else if (args[i].StartsWith("-path"))
                {
                    i++;
                    exePath = args[i];
                }
            }

            foreach (var p in Process.GetProcessesByName(procName))
            {
                if (p.ProcessName.Equals(procName))
                {
                    p.WaitForExit();
                    break;
                }
            }

            Process.Start(exePath);
        }
    }
}
