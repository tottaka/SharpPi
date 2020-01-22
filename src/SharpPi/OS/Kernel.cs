using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace SharpPi.OS
{
    /// <summary>
    /// Interface for the Linux Kernel.
    /// </summary>
    public abstract class Kernel
    {
        public static string Commandline => File.ReadAllText("/boot/cmdline.txt");
    }
}
