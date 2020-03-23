using SharpPi.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Graphics
{
    public abstract class Drm
    {

		/*WEAK uint64_t gbm_bo_get_modifier(struct gbm_bo *bo);
WEAK int gbm_bo_get_plane_count(struct gbm_bo *bo);
WEAK uint32_t gbm_bo_get_stride_for_plane(struct gbm_bo *bo, int plane);
		WEAK uint32_t gbm_bo_get_offset(struct gbm_bo *bo, int plane);
		*/

		public static bool Initialized { get; private set; }

        private static readonly object ThreadLock = new object();
        public static void Initialize()
        {
            lock (ThreadLock)
            {
                if (!Initialized)
                {
                    //NativeMethods.VideoCore.Initialize();
                    Initialized = true;
                }
            }
        }

        /// <summary>
        /// Shutdown the VideoCore driver. Call this after you are done using VideoCore/OpenGL.
        /// </summary>
        public static void Uninitialize()
        {
            lock (ThreadLock)
            {
                if (Initialized)
                {
                    //NativeMethods.VideoCore.Uninitialize();
                    Initialized = false;
                }
            }
        }

    }

	public class DrmContext
	{
		private static bool Initialized { get; set; }
		public static NativeMethods.DynamicLibrary Drm_Lib { get; private set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		private DrmContext()
		{
			if (!Initialized)
			{
				Drm_Lib = NativeMethods.DynamicLibrary.Load(NativeMethods.Library.DRM, NativeMethods.DynamicLibrary.FLAGS_RTLD_NOW);
				Initialized = true;
			}
		}

        public DrmContext(int something) : this()
        {
			/*
             int init_drm(const char *device, const char *mode_str, unsigned int vrefresh)
{
	drmModeRes *resources;
	drmModeConnector *connector = NULL;
	drmModeEncoder *encoder = NULL;
	int i, ret, area;

	drm->fd = open( device, O_RDWR );
	resources = drmModeGetResources( drm->fd );
	if( resources == NULL && errno == EOPNOTSUPP)
		printf("%s does not look like a modeset device\n", device);
	
	if (drm->fd < 0) {
		printf("could not open drm device\n");
		return -1;
	}

	if (!resources) {
		printf("drmModeGetResources failed: %s\n", strerror(errno));
		return -1;
	}

	// find a connected connector:
			for (i = 0; i < resources->count_connectors; i++)
			{
				connector = drmModeGetConnector(drm->fd, resources->connectors[i]);
				if (connector->connection == DRM_MODE_CONNECTED)
				{
					// it's connected, let's use this!
					break;
				}
				drmModeFreeConnector(connector);
				connector = NULL;
			}

			if (!connector)
			{
				// we could be fancy and listen for hotplug events and wait for a connector..
				printf("no connected connector!\n");
				return -1;
			}

			// find user requested mode:
			if (mode_str && *mode_str)
			{
				for (i = 0; i < connector->count_modes; i++)
				{
					drmModeModeInfo* current_mode = &connector->modes[i];

					if (strcmp(current_mode->name, mode_str) == 0)
					{
						if (vrefresh == 0 || current_mode->vrefresh == vrefresh)
						{
							drm->mode = current_mode;
							break;
						}
					}
				}
				if (!drm->mode)
					printf("requested mode not found, using default mode!\n");
			}

			// find preferred mode or the highest resolution mode:
			if (!drm->mode)
			{
				for (i = 0, area = 0; i < connector->count_modes; i++)
				{
					drmModeModeInfo* current_mode = &connector->modes[i];

					if (current_mode->type & DRM_MODE_TYPE_PREFERRED)
					{
						drm->mode = current_mode;
						break;
					}

					int current_area = current_mode->hdisplay * current_mode->vdisplay;
					if (current_area > area)
					{
						drm->mode = current_mode;
						area = current_area;
					}
				}
			}

			if (!drm->mode)
			{
				printf("could not find mode!\n");
				return -1;
			}

			// find encoder
			for (i = 0; i < resources->count_encoders; i++)
			{
				encoder = drmModeGetEncoder(drm->fd, resources->encoders[i]);
				if (encoder->encoder_id == connector->encoder_id)
					break;
				drmModeFreeEncoder(encoder);
				encoder = NULL;
			}

			if (encoder)
			{
				drm->crtc_id = encoder->crtc_id;
			}
			else
			{
				uint32_t crtc_id = find_crtc_for_connector(resources, connector);
				if (crtc_id == 0)
				{
					printf("no crtc found!\n");
					return -1;
				}

				drm->crtc_id = crtc_id;
			}

			for (i = 0; i < resources->count_crtcs; i++)
			{
				if (resources->crtcs[i] == drm->crtc_id)
				{
					drm->crtc_index = i;
					break;
				}
			}

			drmModeFreeResources(resources);

			drm->connector_id = connector->connector_id;

			return 0;
		}

            */
        }
	}
}
