using System.Diagnostics;

namespace SharpPi
{
    public abstract class SubProcess
    {
        public static string Run(string cmd, string args = "")
        {
            using (Process p = new Process { StartInfo = new ProcessStartInfo(cmd, args) { UseShellExecute = false, RedirectStandardOutput = true } })
            {
                if (p.Start())
                {
                    p.WaitForExit();
                    return p.StandardOutput.ReadToEnd();
                }

                return string.Empty;
            }
        }

        public static string Pipe(string cmd1, string args1, string cmd2, string args2)
        {
            using (Process p1 = new Process { StartInfo = new ProcessStartInfo(cmd1, args1) { UseShellExecute = false, RedirectStandardOutput = true } })
            {
                if (p1.Start())
                {
                    p1.WaitForExit();
                    using (Process p2 = new Process { StartInfo = new ProcessStartInfo(cmd2, args2) { UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true } })
                    {
                        if (p2.Start())
                        {
                            p2.StandardInput.WriteLine(p1.StandardOutput.ReadToEnd());
                            p2.StandardInput.Close();
                            p2.WaitForExit();
                            return p2.StandardOutput.ReadToEnd();
                        }
                    }
                }

                return string.Empty;
            }
        }
    }
}
