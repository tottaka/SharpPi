using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class Drm
        {
			public struct drmModePlane
			{
				uint count_formats;
				uint formats;
				uint plane_id;

				uint crtc_id;
				uint fb_id;

				uint crtc_x, crtc_y;
				uint x, y;

				uint possible_crtcs;
				uint gamma_size;
			}

			public struct drmModeObjectProperties
			{
				uint count_props;
				uint props;
				uint prop_values;
			}

			public struct drmModePropertyRes
			{
				uint prop_id;
				uint flags;
				string name; //char name[DRM_PROP_NAME_LEN];
				int count_values;
				ulong values; /* store the blob lengths */
				int count_enums;
				drm_mode_property_enum enums;
				int count_blobs;
				uint blob_ids; /* store the blob IDs */
			}

			public struct drm_mode_property_enum
			{
				ulong value;
				string name; // char name[DRM_PROP_NAME_LEN];
			}

			public struct drmModeModeInfo
			{
				uint clock;
				ushort hdisplay, hsync_start, hsync_end, htotal, hskew;
				ushort vdisplay, vsync_start, vsync_end, vtotal, vscan;
				uint vrefresh;
				uint flags;
				uint type;
				string name;//  char name[DRM_DISPLAY_MODE_LEN];
			}

			public struct drmModeCrtc
			{
				uint crtc_id;
				uint buffer_id; // FB id to connect to 0 = disconnect
				uint x, y; // Position on the framebuffer
				uint width, height;
				int mode_valid;
				drmModeModeInfo mode;
				int gamma_size; // Number of gamma stops
			}

			public struct drmModeConnector
			{
				uint connector_id;
				uint encoder_id; /* Encoder currently connected to */
				uint connector_type;
				uint connector_type_id;
				drmModeConnection connection;
				uint mmWidth, mmHeight; /*  HxW in millimeters */
				drmModeSubPixel subpixel;

				int count_modes;
				drmModeModeInfo modes;

				int count_props;
				uint props; /* List of property ids */
				ulong prop_values; /* List of property values */

				int count_encoders;
				uint encoders; /* List of encoder ids */
			}

			public struct Plane
			{
				drmModePlane plane;
				drmModeObjectProperties props;
				drmModePropertyRes props_info;
			};

			public struct Crtc
			{
				drmModeCrtc crtc;
				drmModeObjectProperties props;
				drmModePropertyRes props_info;
			
			
			};

			public struct Connector
			{
				drmModeConnector connector;
				drmModeObjectProperties props;
				drmModePropertyRes props_info;
			};

			public delegate int DrmRunDelegate(GbmVars gbm, EglVars egl);

			public struct DrmVars
			{
				public int fd;

				/* only used for atomic: */
				public Plane plane;
				public Crtc crtc;
				public Connector connector;
				public int crtc_index;
				public int kms_in_fence_fd;
				public int kms_out_fence_fd;

				public drmModeModeInfo mode;
				public uint crtc_id;
				public uint connector_id;

				public DrmRunDelegate Run;
			}

			struct gbm_bo
			{
				int fd;
				uint width;
				uint height;
				uint stride;
				uint format;
			};

			public struct drm_fb
			{
				gbm_bo bo;
				uint fb_id;
			};
		}
    }
}
