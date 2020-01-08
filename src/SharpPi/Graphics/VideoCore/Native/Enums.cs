namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class VideoCore
        {
            public enum DISPLAY_INPUT_FORMAT_T
            {
                VCOS_DISPLAY_INPUT_FORMAT_INVALID = 0,
                VCOS_DISPLAY_INPUT_FORMAT_RGB888,
                VCOS_DISPLAY_INPUT_FORMAT_RGB565
            }

            public enum VC_IMAGE_TYPE_T
            {
                VC_IMAGE_MIN = 0, //bounds for error checking

                VC_IMAGE_RGB565 = 1,
                VC_IMAGE_1BPP,
                VC_IMAGE_YUV420,
                VC_IMAGE_48BPP,
                VC_IMAGE_RGB888,
                VC_IMAGE_8BPP,
                VC_IMAGE_4BPP,    // 4bpp palettised image
                VC_IMAGE_3D32,    /* A separated format of 16 colour/light shorts followed by 16 z values */
                VC_IMAGE_3D32B,   /* 16 colours followed by 16 z values */
                VC_IMAGE_3D32MAT, /* A separated format of 16 material/colour/light shorts followed by 16 z values */
                VC_IMAGE_RGB2X9,   /* 32 bit format containing 18 bits of 6.6.6 RGB, 9 bits per short */
                VC_IMAGE_RGB666,   /* 32-bit format holding 18 bits of 6.6.6 RGB */
                VC_IMAGE_PAL4_OBSOLETE,     // 4bpp palettised image with embedded palette
                VC_IMAGE_PAL8_OBSOLETE,     // 8bpp palettised image with embedded palette
                VC_IMAGE_RGBA32,   /* RGB888 with an alpha byte after each pixel */ /* xxx: isn't it BEFORE each pixel? */
                VC_IMAGE_YUV422,   /* a line of Y (32-byte padded), a line of U (16-byte padded), and a line of V (16-byte padded) */
                VC_IMAGE_RGBA565,  /* RGB565 with a transparent patch */
                VC_IMAGE_RGBA16,   /* Compressed (4444) version of RGBA32 */
                VC_IMAGE_YUV_UV,   /* VCIII codec format */
                VC_IMAGE_TF_RGBA32, /* VCIII T-format RGBA8888 */
                VC_IMAGE_TF_RGBX32,  /* VCIII T-format RGBx8888 */
                VC_IMAGE_TF_FLOAT, /* VCIII T-format float */
                VC_IMAGE_TF_RGBA16, /* VCIII T-format RGBA4444 */
                VC_IMAGE_TF_RGBA5551, /* VCIII T-format RGB5551 */
                VC_IMAGE_TF_RGB565, /* VCIII T-format RGB565 */
                VC_IMAGE_TF_YA88, /* VCIII T-format 8-bit luma and 8-bit alpha */
                VC_IMAGE_TF_BYTE, /* VCIII T-format 8 bit generic sample */
                VC_IMAGE_TF_PAL8, /* VCIII T-format 8-bit palette */
                VC_IMAGE_TF_PAL4, /* VCIII T-format 4-bit palette */
                VC_IMAGE_TF_ETC1, /* VCIII T-format Ericsson Texture Compressed */
                VC_IMAGE_BGR888,  /* RGB888 with R & B swapped */
                VC_IMAGE_BGR888_NP,  /* RGB888 with R & B swapped, but with no pitch, i.e. no padding after each row of pixels */
                VC_IMAGE_BAYER,  /* Bayer image, extra defines which variant is being used */
                VC_IMAGE_CODEC,  /* General wrapper for codec images e.g. JPEG from camera */
                VC_IMAGE_YUV_UV32,   /* VCIII codec format */
                VC_IMAGE_TF_Y8,   /* VCIII T-format 8-bit luma */
                VC_IMAGE_TF_A8,   /* VCIII T-format 8-bit alpha */
                VC_IMAGE_TF_SHORT,/* VCIII T-format 16-bit generic sample */
                VC_IMAGE_TF_1BPP, /* VCIII T-format 1bpp black/white */
                VC_IMAGE_OPENGL,
                VC_IMAGE_YUV444I, /* VCIII-B0 HVS YUV 4:4:4 interleaved samples */
                VC_IMAGE_YUV422PLANAR,  /* Y, U, & V planes separately (VC_IMAGE_YUV422 has them interleaved on a per line basis) */
                VC_IMAGE_ARGB8888,   /* 32bpp with 8bit alpha at MS byte, with R, G, B (LS byte) */
                VC_IMAGE_XRGB8888,   /* 32bpp with 8bit unused at MS byte, with R, G, B (LS byte) */

                VC_IMAGE_YUV422YUYV,  /* interleaved 8 bit samples of Y, U, Y, V */
                VC_IMAGE_YUV422YVYU,  /* interleaved 8 bit samples of Y, V, Y, U */
                VC_IMAGE_YUV422UYVY,  /* interleaved 8 bit samples of U, Y, V, Y */
                VC_IMAGE_YUV422VYUY,  /* interleaved 8 bit samples of V, Y, U, Y */

                VC_IMAGE_RGBX32,      /* 32bpp like RGBA32 but with unused alpha */
                VC_IMAGE_RGBX8888,    /* 32bpp, corresponding to RGBA with unused alpha */
                VC_IMAGE_BGRX8888,    /* 32bpp, corresponding to BGRA with unused alpha */

                VC_IMAGE_YUV420SP,    /* Y as a plane, then UV byte interleaved in plane with with same pitch, half height */

                VC_IMAGE_YUV444PLANAR,  /* Y, U, & V planes separately 4:4:4 */

                VC_IMAGE_TF_U8,   /* T-format 8-bit U - same as TF_Y8 buf from U plane */
                VC_IMAGE_TF_V8,   /* T-format 8-bit U - same as TF_Y8 buf from V plane */

                VC_IMAGE_MAX,     //bounds for error checking
                VC_IMAGE_FORCE_ENUM_16BIT = 0xffff,
            }

            public enum DISPMANX_TRANSFORM_T
            {
                /* Bottom 2 bits sets the orientation */
                DISPMANX_NO_ROTATE = 0,
                DISPMANX_ROTATE_90 = 1,
                DISPMANX_ROTATE_180 = 2,
                DISPMANX_ROTATE_270 = 3,

                DISPMANX_FLIP_HRIZ = 1 << 16,
                DISPMANX_FLIP_VERT = 1 << 17,

                /* invert left/right images */
                DISPMANX_STEREOSCOPIC_INVERT = 1 << 19,
                /* extra flags for controlling 3d duplication behaviour */
                DISPMANX_STEREOSCOPIC_NONE = 0 << 20,
                DISPMANX_STEREOSCOPIC_MONO = 1 << 20,
                DISPMANX_STEREOSCOPIC_SBS = 2 << 20,
                DISPMANX_STEREOSCOPIC_TB = 3 << 20,
                DISPMANX_STEREOSCOPIC_MASK = 15 << 20,

                /* extra flags for controlling snapshot behaviour */
                DISPMANX_SNAPSHOT_NO_YUV = 1 << 24,
                DISPMANX_SNAPSHOT_NO_RGB = 1 << 25,
                DISPMANX_SNAPSHOT_FILL = 1 << 26,
                DISPMANX_SNAPSHOT_SWAP_RED_BLUE = 1 << 27,
                DISPMANX_SNAPSHOT_PACK = 1 << 28
            }

            public enum DISPMANX_FLAGS_ALPHA_T
            {
                /* Bottom 2 bits sets the alpha mode */
                DISPMANX_FLAGS_ALPHA_FROM_SOURCE = 0,
                DISPMANX_FLAGS_ALPHA_FIXED_ALL_PIXELS = 1,
                DISPMANX_FLAGS_ALPHA_FIXED_NON_ZERO = 2,
                DISPMANX_FLAGS_ALPHA_FIXED_EXCEED_0X07 = 3,

                DISPMANX_FLAGS_ALPHA_PREMULT = 1 << 16,
                DISPMANX_FLAGS_ALPHA_MIX = 1 << 17
            }

            public enum DISPMANX_FLAGS_CLAMP_T
            {
                DISPMANX_FLAGS_CLAMP_NONE = 0,
                DISPMANX_FLAGS_CLAMP_LUMA_TRANSPARENT = 1,

                // If __VCCOREVER__ >= 0x04000000
                DISPMANX_FLAGS_CLAMP_TRANSPARENT = 2,
                DISPMANX_FLAGS_CLAMP_REPLACE = 3

                // If __VCCOREVER__ < 0x04000000
                // DISPMANX_FLAGS_CLAMP_CHROMA_TRANSPARENT = 2,
                // DISPMANX_FLAGS_CLAMP_TRANSPARENT = 3
            }

            public enum DISPMANX_FLAGS_KEYMASK_T
            {
                DISPMANX_FLAGS_KEYMASK_OVERRIDE = 1,
                DISPMANX_FLAGS_KEYMASK_SMOOTH = 1 << 1,
                DISPMANX_FLAGS_KEYMASK_CR_INV = 1 << 2,
                DISPMANX_FLAGS_KEYMASK_CB_INV = 1 << 3,
                DISPMANX_FLAGS_KEYMASK_YY_INV = 1 << 4
            }
        }
    }
}
