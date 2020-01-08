using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        /// <summary>
        /// Cisco OpenH264
        /// </summary>
        public abstract partial class H264
        {
            /*
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct ISVCEncoder
            {
                int Initialize(ISVCEncoder*, const SEncParamBase* pParam);
                int InitializeExt(ISVCEncoder*, const SEncParamExt* pParam);

                int GetDefaultParams(ISVCEncoder*, SEncParamExt* pParam);
                int Uninitialize(ISVCEncoder*);

                int EncodeFrame(ISVCEncoder*, const SSourcePicture* kpSrcPic, SFrameBSInfo* pBsInfo);
	            int EncodeParameterSets(ISVCEncoder*, SFrameBSInfo* pBsInfo);

	            int ForceIntraFrame(ISVCEncoder*, bool bIDR);

	            int SetOption(ISVCEncoder*, ENCODER_OPTION eOptionId, void* pOption);
	            int GetOption(ISVCEncoder*, ENCODER_OPTION eOptionId, void* pOption);
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct ISVCDecoder
            {
                int Initialize(ISVCDecoder*, const SDecodingParam* pParam);
                int Uninitialize(ISVCDecoder*);

	            DecodingState DecodeFrame(ISVCDecoder*, const unsigned char* pSrc, const int iSrcLen, unsigned char** ppDst, int* pStride, int* iWidth, int* iHeight);

                DecodingState DecodeFrameNoDelay(ISVCDecoder*, const unsigned char* pSrc, const int iSrcLen, unsigned char** ppDst, SBufferInfo* pDstInfo);

	            DecodingState DecodeFrame2(ISVCDecoder*, const unsigned char* pSrc, const int iSrcLen, unsigned char** ppDst, SBufferInfo* pDstInfo);

	            DecodingState DecodeParser(ISVCDecoder*, const unsigned char* pSrc, const int iSrcLen, SParserBsInfo* pDstInfo);

	            DecodingState DecodeFrameEx(ISVCDecoder*, const unsigned char* pSrc, const int iSrcLen, unsigned char* pDst, int iDstStride, int* iDstLen, int* iWidth, int* iHeight, int* iColorFormat);

                int SetOption(ISVCDecoder*, DecoderOption eOptionId, void* pOption);
	            int GetOption(ISVCDecoder*, DecoderOption eOptionId, void* pOption);
            }
            */
            /// <summary>
            /// Define a new struct to show the property of video bitstream.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct SVideoProperty
            {
                /// <summary>
                /// Size of the struct.
                /// </summary>
                public uint size;

                /// <summary>
                /// Video stream type (AVC/SVC)
                /// </summary>
                public VideoBitstreamType eVideoBsType;
            }

            /// <summary>
            /// SVC Decoding Parameters, reserved here and potential applicable in the future.
            /// SDecodingParam, *PDecodingParam;
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public unsafe struct SDecodingParam
            {
                /// <summary>
                /// File name of reconstructed frame used for PSNR calculation based debug.
                /// </summary>
                // this might be a string???
                public char* pFileNameRestructed;

                /// <summary>
                /// CPU load.
                /// </summary>
                public uint uiCpuLoad;

                /// <summary>
                /// Setting target dq layer id.
                /// </summary>
                public byte uiTargetDqLayer;

                /// <summary>
                /// Whether active error concealment feature in decoder.
                /// </summary>
                public ErrorConIDC eEcActiveIdc;

                /// <summary>
                /// Decoder for parse only, no reconstruction. When it is true, SPS/PPS size should not exceed SPS_PPS_BS_SIZE (128). Otherwise, it will return error info.
                /// </summary>
                public bool bParseOnly;

                /// <summary>
                /// Video stream property.
                /// </summary>
                public SVideoProperty sVideoProperty;
            }
            //SDecodingParam, *PDecodingParam;

            /// <summary>
            /// Structure for parse only output.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct SParserBsInfo
            {
                /// <summary>
                /// Total NAL number in current AU.
                /// </summary>
                int iNalNum;

                /// <summary>
                /// Each nal length.
                /// </summary>
                //[MarshalAs(UnmanagedType.)]
                int[] pNalLenInByte;  //< each nal length

                /// <summary>
                /// Output dest buffer for parsed bitstream.
                /// </summary>
                //[MarshalAs(UnmanagedType.)]
                byte[] pDstBuff;

                /// <summary>
                /// Required SPS width info.
                /// </summary>
                int iSpsWidthInPixel;

                /// <summary>
                /// Required SPS height info.
                /// </summary>
                int iSpsHeightInPixel;

                /// <summary>
                /// Input BS timestamp.
                /// </summary>
                ulong uiInBsTimeStamp;

                /// <summary>
                /// Output BS timestamp.
                /// </summary>
                ulong uiOutBsTimeStamp;
            }

            /// <summary>
            /// Buffer info
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct SBufferInfo
            {
                /// <summary>
                /// 0: one frame data is not ready; 1: one frame data is ready.
                /// </summary>
                public int iBufferStatus;

                /// <summary>
                /// input BS timestamp.
                /// </summary>
                public ulong uiInBsTimeStamp;

                /// <summary>
                /// Output YUV timestamp, when bufferstatus is 1.
                /// </summary>
                ulong uiOutYuvTimeStamp;

                /// <summary>
                /// Output buffer info.
                /// </summary>
                public UsrData Data;

                /*
                union
                {
                    SSysMEMBuffer sSystemBuffer; ///<  memory info for one picture
                } UsrData;                     ///<  output buffer info
                */
            }

            /// <summary>
            /// Structure for decoder memery
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct SSysMEMBuffer
            {
                /// <summary>
                /// Width of decoded pic for display.
                /// </summary>
                public int iWidth;

                /// <summary>
                /// Height of decoded pic for display.
                /// </summary>
                public int iHeight;

                /// <summary>
                /// Type is "EVideoFormatType"
                /// </summary>
                public int iFormat;

                /// <summary>
                /// Stride of 2 component.
                /// </summary>
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                public int[] iStride;
            }

            /// <summary>
            /// Output buffer info
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct UsrData
            {
                /// <summary>
                /// Memory info for one picture
                /// </summary>
                [FieldOffset(0)]
                public SSysMEMBuffer sSystemBuffer;
            }
        }
    }
}
