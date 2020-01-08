using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class GPIO
        {
            [DllImport(Library.GPIO, EntryPoint = "bcm2835_init")]
            public static extern bool bcm2835_init();

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_fsel")]
            public static extern void bcm2835_gpio_fsel(uint pin, bool mode_out);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_write")]
            public static extern void bcm2835_gpio_write(uint pin, bool value);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_lev")]
            public static extern bool bcm2835_gpio_lev(uint pin);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_set_pud")]
            public static extern void bcm2835_gpio_set_pud(uint pin, uint pud);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_set_multi")]
            public static extern void bcm2835_gpio_set_multi(uint mask);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_clr_multi")]
            public static extern void bcm2835_gpio_clr_multi(uint mask);

            [DllImport(Library.GPIO, EntryPoint = "bcm2835_gpio_write_multi")]
            public static extern void bcm2835_gpio_write_multi(uint mask, bool on);
        }
    }
}
