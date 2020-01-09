using System;
using System.IO;
using System.Net;
using SharpPi.Graphics;
using SharpPi.Native;

namespace SharpPi
{
    /// <summary>
    /// The different types of Raspberry Pi board models.
    /// </summary>
    public enum ModelType : int
    {
        A = 0,
        B = 1,
        A_PLUS = 2,
        B_PLUS = 3,
        Pi2_B = 4,
        Pi3_B = 8,
        Pi3_B_PLUS = 0xd,
        Pi3_A_PLUS = 0xe,
        Pi_Zero = 9,
        Pi_ZeroW = 0xc,
        Pi4_B = 0x11,
        ALPHA = 5,
        CM = 6,
        CM2 = 7,
        CM3 = 0xa,
        CM3_PLUS = 0x10,
        CUSTOM = 0xb,
        FPGA = 0xf,
    };

    /// <summary>
    /// The chipset for the processor of this board.
    /// </summary>
    public enum ProcessorType : int
    {
        BCM2835 = 0,
        BCM2836 = 1,
        BCM2837 = 2,
        BCM2838 = 3,
    }

    /// <summary>
    /// Default display IDs.
    /// Note: if you overwrite with your own dispmanx_platform_init function, you should use IDs you provided during dispmanx_display_attach.
    /// </summary>
    public enum DisplayID : short { LCD = 0, AUX = 1, HDMI = 2 }

    public abstract class Device
    {
        /// <summary>
        /// Provides real-time data about the CPU on the Raspberry Pi.
        /// </summary>
        public static class CPU
        {
            /// <summary>
            /// The temperature of the CPU (in degrees centigrade)
            /// </summary>
            public static double Temperature => double.Parse(SubProcess.Run("/opt/vc/bin/vcgencmd", "measure_temp").Replace("temp=", string.Empty).Replace("'C", string.Empty).Trim());

            /// <summary>
            /// The current frequency of the CPU (in Hz)
            /// </summary>
            public static uint Frequency => uint.Parse(SubProcess.Run("cat", "/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq").Trim());// why not change to File.ReadAllText ????
        }

        /// <summary>
        /// The model of the Raspberry Pi board.
        /// </summary>
        public static ModelType Model => (ModelType)NativeMethods.VideoCore.bcm_host_get_model_type();

        /// <summary>
        /// Determines whether this board is the Raspberry Pi 4. 
        /// </summary>
        public static bool IsPi4 => NativeMethods.VideoCore.bcm_host_is_model_pi4() == 1;

        public enum KMS_Status { DISABLED = 0, FAKE = 1, FULL = 2 };

        /// <summary>
        /// The Kernal Mode Setting for the board.
        /// </summary>
        public static KMS_Status KMS => NativeMethods.VideoCore.bcm_host_is_fkms_active() == 1 ? KMS_Status.FAKE : NativeMethods.VideoCore.bcm_host_is_kms_active() == 1 ? KMS_Status.FULL : KMS_Status.DISABLED;

        /// <summary>
        /// The chipset of the processor.
        /// </summary>
        public static ProcessorType Chipset => (ProcessorType)NativeMethods.VideoCore.bcm_host_get_processor_id();

        /// <summary>
        /// iwlist
        /// </summary>
        /// <param name="_interface">The interface to scan for networks on.</param>
        /// <returns>A string that contains information about the wireless networks in area.</returns>
        public static string GetNetworkList(string _interface = "wlan0") => SubProcess.Pipe("iwlist", _interface + " scanning", "egrep", "\"Cell |Encryption|Quality|Last beacon|ESSID\"");

        /// <summary>
        /// ifconfig
        /// </summary>
        /// <param name="_interface">The interface to query</param>
        /// <returns>A string that contains information about the interface.</returns>
        public static string GetInterfaceStatus(string _interface = "wlan0") => SubProcess.Run("ifconfig", _interface);

        /// <summary>
        /// iwconfig
        /// </summary>
        /// <param name="_interface">The interface to query</param>
        /// <returns>A string that contains information about the status of the interface.</returns>
        public static string GetWifiStatus(string _interface = "wlan0") => SubProcess.Run("iwconfig", _interface);

        /// <summary>
        /// Parses the inet address from ifconfig.
        /// </summary>
        /// <param name="_interface">The interface to query the inet address from.</param>
        /// <returns><see cref="IPAddress"/> if successful, otherwise <see cref="IPAddress.None"/>.</returns>
        public static IPAddress GetInterfaceIPv4(string _interface = "wlan0")
        {
            string[] if_status = GetInterfaceStatus(_interface).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (if_status[1].Contains("inet") && if_status[1].Contains("netmask") && if_status[1].Contains("broadcast"))
            {
                int inet_index = if_status[1].IndexOf("inet");
                return IPAddress.Parse(if_status[1].Substring(inet_index + 5, if_status[1].IndexOf(' ', inet_index + 5) - (inet_index + 5)).Trim());
            }

            return IPAddress.None;
        }

        /// <summary>
        /// Set the hostname for this device.
        /// </summary>
        /// <param name="name">The hostname</param>
        public static void SetHostname(string name)
        {
            File.WriteAllText("/etc/hostname", name);
            File.WriteAllText("/etc/hosts", string.Format(Properties.Resources.hosts_template, name));
        }
    }
}
