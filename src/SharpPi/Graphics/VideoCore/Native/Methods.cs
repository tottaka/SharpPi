using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    using DISPMANX_DISPLAY_HANDLE_T = System.UInt32;
    using DISPMANX_ELEMENT_HANDLE_T = System.UInt32;
    using DISPMANX_PROTECTION_T = System.UInt32;
    using DISPMANX_RESOURCE_HANDLE_T = System.UInt32;
    using DISPMANX_UPDATE_HANDLE_T = System.UInt32;

    public abstract partial class NativeMethods
    {
        public abstract partial class VideoCore
        {
            /// <summary>
            /// The processor ID.
            /// </summary>
            [DllImport(Library.VideoCore)]
            public static extern int bcm_host_get_processor_id();

            /// <summary>
            /// Returns 1 if kms is active (dtoverlay=v3d-kms-vc4)
            /// </summary>
            [DllImport(Library.VideoCore)]
            public static extern int bcm_host_is_kms_active();

            /// <summary>
            /// Returns 1 if fkms is active (dtoverlay=v3d-fkms-vc4)
            /// </summary>
            [DllImport(Library.VideoCore)]
            public static extern int bcm_host_is_fkms_active();

            /// <summary>
            /// Returns 1 if model is Pi4
            /// </summary>
            [DllImport(Library.VideoCore)]
            public static extern int bcm_host_is_model_pi4();

            /// <summary>
            /// Gets the model of the Raspberry Pi board being used.
            /// </summary>
            [DllImport(Library.VideoCore)]
            public static extern int bcm_host_get_model_type();

            [DllImport(Library.VideoCore, EntryPoint = "bcm_host_init")]
            public static extern void Initialize();

            [DllImport(Library.VideoCore, EntryPoint = "bcm_host_deinit")]
            public static extern void Uninitialize();

            [DllImport(Library.VideoCore, EntryPoint = "graphics_get_display_size")]
            public static extern int GetDisplaySize([MarshalAs(UnmanagedType.I2)]DisplayID display, out int width, out int height);

            //[DllImport(host_lib, EntryPoint = "vc_dispman_init")]
            //public extern static int vc_dispman_init();

            //[DllImport(host_lib, EntryPoint = "vc_dispmanx_stop")]
            //public extern static void vc_dispmanx_stop();


            //[DllImport(Library, EntryPoint = "vc_dispmanx_rect_set")]
            //public extern static int vc_dispmanx_rect_set(VC_RECT_T *rect, UInt32 x_offset, UInt32 y_offset, UInt32 width, UInt32 height);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_create")]
            //public extern static DISPMANX_RESOURCE_HANDLE_T vc_dispmanx_resource_create(VC_IMAGE_TYPE_T type, UInt32 width, UInt32 height, UInt32 *native_image_handle );


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_write_data")]
            //public extern static int vc_dispmanx_resource_write_data(DISPMANX_RESOURCE_HANDLE_T res, VC_IMAGE_TYPE_T src_type, int src_pitch, void * src_address, ref VC_RECT_T rect);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_write_data_handle")]
            //public extern static int vc_dispmanx_resource_write_data_handle(DISPMANX_RESOURCE_HANDLE_T res, VC_IMAGE_TYPE_T src_type, int src_pitch, VCHI_MEM_HANDLE_T handle, UInt32 offset, ref VC_RECT_T rect );


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_read_data")]
            //public extern static int vc_dispmanx_resource_read_data(DISPMANX_RESOURCE_HANDLE_T handle, ref VC_RECT_T p_rect, IntPtr dst_address, UInt32 dst_pitch);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_delete")]
            //public extern static int vc_dispmanx_resource_delete(DISPMANX_RESOURCE_HANDLE_T res);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_display_open")]
            public static extern DISPMANX_DISPLAY_HANDLE_T vc_dispmanx_display_open(UInt32 device);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_open_mode")]
            //public extern static DISPMANX_DISPLAY_HANDLE_T vc_dispmanx_display_open_mode(UInt32 device, UInt32 mode);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_open_offscreen")]
            //public extern static DISPMANX_DISPLAY_HANDLE_T vc_dispmanx_display_open_offscreen(DISPMANX_RESOURCE_HANDLE_T dest, DISPMANX_TRANSFORM_T orientation);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_reconfigure")]
            //public extern static int vc_dispmanx_display_reconfigure(DISPMANX_DISPLAY_HANDLE_T display, UInt32 mode);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_set_destination")]
            //public extern static int vc_dispmanx_display_set_destination(DISPMANX_DISPLAY_HANDLE_T display, DISPMANX_RESOURCE_HANDLE_T dest);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_set_background")]
            //public extern static int vc_dispmanx_display_set_background(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_DISPLAY_HANDLE_T display, Byte red, Byte green, Byte blue );


            //[DllImport(Library, EntryPoint = "vc_dispmanx_display_get_info")]
            //public extern static int vc_dispmanx_display_get_info(DISPMANX_DISPLAY_HANDLE_T display, DISPMANX_MODEINFO_T * pinfo);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_display_close")]
            public static extern int vc_dispmanx_display_close(DISPMANX_DISPLAY_HANDLE_T display);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_update_start")]
            public static extern DISPMANX_UPDATE_HANDLE_T vc_dispmanx_update_start(Int32 priority);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_element_add")]
            public static extern DISPMANX_ELEMENT_HANDLE_T vc_dispmanx_element_add(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_DISPLAY_HANDLE_T display, Int32 layer, ref VC_RECT_T dest_rect, DISPMANX_RESOURCE_HANDLE_T src, ref VC_RECT_T src_rect, DISPMANX_PROTECTION_T protection, IntPtr alpha, IntPtr clamp, DISPMANX_TRANSFORM_T transform);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_element_change_source")]
            //public extern static int vc_dispmanx_element_change_source(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element, DISPMANX_RESOURCE_HANDLE_T src);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_element_change_layer")]
            //public extern static int vc_dispmanx_element_change_layer(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element, Int32 layer);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_element_modified")]
            //public extern static int vc_dispmanx_element_modified(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element, ref VC_RECT_T rect);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_element_remove")]
            public static extern int vc_dispmanx_element_remove(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_update_submit")]
            //public extern static int vc_dispmanx_update_submit(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_CALLBACK_FUNC_T cb_func, void *cb_arg);

            [DllImport(Library.VideoCore, EntryPoint = "vc_dispmanx_update_submit_sync")]
            public static extern int vc_dispmanx_update_submit_sync(DISPMANX_UPDATE_HANDLE_T update);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_query_image_formats")]
            //public extern static int vc_dispmanx_query_image_formats(UInt32 *supported_formats);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_element_change_attributes")]
            //public extern static int vc_dispmanx_element_change_attributes(DISPMANX_UPDATE_HANDLE_T update, DISPMANX_ELEMENT_HANDLE_T element, UInt32 change_flags, Int32 layer, Byte opacity,  ref VC_RECT_T dest_rect, ref VC_RECT_T src_rect, DISPMANX_RESOURCE_HANDLE_T mask, DISPMANX_TRANSFORM_T transform);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_get_image_handle")]
            //public extern static UInt32 vc_dispmanx_resource_get_image_handle(DISPMANX_RESOURCE_HANDLE_T res);

            // [SuppressUnmanagedCodeSecurity()]
            // [DllImport(Library, EntryPoint = "vc_vchi_dispmanx_init")]
            // public extern static void vc_vchi_dispmanx_init(VCHI_INSTANCE_T initialise_instance, VCHI_CONNECTION_T **connections, UInt32 num_connections );


            //[DllImport(Library, EntryPoint = "vc_dispmanx_snapshot")]
            //public extern static int vc_dispmanx_snapshot( DISPMANX_DISPLAY_HANDLE_T display, DISPMANX_RESOURCE_HANDLE_T snapshot_resource, DISPMANX_TRANSFORM_T transform);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_resource_set_palette")]
            //public extern static int vc_dispmanx_resource_set_palette(DISPMANX_RESOURCE_HANDLE_T handle, IntPtr src_address, int offset, int size);


            //[DllImport(Library, EntryPoint = "vc_dispmanx_vsync_callback")]
            //public extern static int vc_dispmanx_vsync_callback(DISPMANX_DISPLAY_HANDLE_T display, DISPMANX_CALLBACK_FUNC_T cb_func, void *cb_arg);
        }
    }
}
