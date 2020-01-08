namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        /// <summary>
        /// Decoding status
        /// </summary>
        public enum DecodingState
        {
            // Errors derived from bitstream parsing
            /// <summary>
            /// No error
            /// </summary>
            dsErrorFree = 0x00,

            /// <summary>
            /// Need more throughput to generate a frame output.
            /// </summary>
            dsFramePending = 0x01,

            /// <summary>
            /// Layer lost at reference frame with temporal id 0.
            /// </summary>
            dsRefLost = 0x02,

            /// <summary>
            /// Bitstream error (maybe broken internal frame?)
            /// </summary>
            dsBitstreamError = 0x04,

            /// <summary>
            /// Dependented layer is ever lost.
            /// </summary>
            dsDepLayerLost = 0x08,

            /// <summary>
            /// No parameter set NALs involved.
            /// </summary>
            dsNoParamSets = 0x10,

            /// <summary>
            /// Current data error concealed specified.
            /// </summary>
            dsDataErrorConcealed = 0x20,

            // Errors derived from logic level
            /// <summary>
            /// Invalid argument specified.
            /// </summary>
            dsInvalidArgument = 0x1000,

            /// <summary>
            /// Initializing operation is expected.
            /// </summary>
            dsInitialOptExpected = 0x2000,

            /// <summary>
            /// Out of memory due to new request.
            /// </summary>
            dsOutOfMemory = 0x4000,

            // ANY OTHERS?
            /// <summary>
            /// Actual picture size exceeds size of dst pBuffer feed in decoder, resize required.
            /// </summary>
            dsDstBufNeedExpan = 0x8000
        }

        /// <summary>
        /// Option types introduced in decoder application
        /// </summary>
        public enum DecoderOption
        {
            /// <summary>
            /// End of stream flag.
            /// </summary>
            DECODER_OPTION_END_OF_STREAM = 1,

            /// <summary>
            /// Feedback whether or not the current AU for application layer has VCL NAL.
            /// </summary>
            DECODER_OPTION_VCL_NAL,

            /// <summary>
            /// Feedback temporal id for application layer.
            /// </summary>
            DECODER_OPTION_TEMPORAL_ID,

            /// <summary>
            /// Feedback current decoded frame number.
            /// </summary>
            DECODER_OPTION_FRAME_NUM,

            /// <summary>
            /// Feedback current frame belongs to which IDR period.
            /// </summary>
            DECODER_OPTION_IDR_PIC_ID,

            /// <summary>
            /// Feedback whether current frame mark is an LTR.
            /// </summary>
            DECODER_OPTION_LTR_MARKING_FLAG,

            /// <summary>
            /// Feedback frame num marked by current Frame.
            /// </summary>
            DECODER_OPTION_LTR_MARKED_FRAME_NUM,

            /// <summary>
            /// Indicate decoder error concealment method.
            /// </summary>
            DECODER_OPTION_ERROR_CON_IDC,

            /// <summary>
            /// Not Implemented
            /// </summary>
            DECODER_OPTION_TRACE_LEVEL,

            /// <summary>
            /// A void (*)(void* context, int level, const char* message) function which receives log messages.
            /// </summary>
            DECODER_OPTION_TRACE_CALLBACK,

            /// <summary>
            /// Context info of trace callback.
            /// </summary>
            DECODER_OPTION_TRACE_CALLBACK_CONTEXT,

            /// <summary>
            /// Feedback decoder statistics.
            /// </summary>
            DECODER_OPTION_GET_STATISTICS,

            /// <summary>
            /// Feedback decoder Sample Aspect Ratio info in Vui.
            /// </summary>
            DECODER_OPTION_GET_SAR_INFO,

            /// <summary>
            /// Get current AU profile info (only used in GetOption.)
            /// </summary>
            DECODER_OPTION_PROFILE,

            /// <summary>
            /// Get current AU level info (only used in GetOption.)
            /// </summary>
            DECODER_OPTION_LEVEL,

            /// <summary>
            /// Set log output interval.
            /// </summary>
            DECODER_OPTION_STATISTICS_LOG_INTERVAL
        }

        /// <summary>
        /// Enumerate the type of video bitstream which is provided to decoder.
        /// </summary>
        public enum VideoBitstreamType
        {
            VIDEO_BITSTREAM_AVC = 0,
            VIDEO_BITSTREAM_SVC = 1,
            VIDEO_BITSTREAM_DEFAULT = VIDEO_BITSTREAM_SVC
        }

        /// <summary>
        /// Enumerate the type of error concealment methods
        /// </summary>
        public enum ErrorConIDC
        {
            ERROR_CON_DISABLE = 0,
            ERROR_CON_FRAME_COPY,
            ERROR_CON_SLICE_COPY,
            ERROR_CON_FRAME_COPY_CROSS_IDR,
            ERROR_CON_SLICE_COPY_CROSS_IDR,
            ERROR_CON_SLICE_COPY_CROSS_IDR_FREEZE_RES_CHANGE,
            ERROR_CON_SLICE_MV_COPY_CROSS_IDR,
            ERROR_CON_SLICE_MV_COPY_CROSS_IDR_FREEZE_RES_CHANGE
        }
    }
}
