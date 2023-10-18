﻿using System;
using System.Runtime.InteropServices;

namespace Agora.Rtc
{
    ///
    /// <summary>
    /// The mode for receiving data.
    /// </summary>
    ///
    public enum OBSERVER_MODE
    {
        ///
        /// <summary>
        /// Raw data mode, which means the SDK sends you raw data.
        /// </summary>
        ///
        RAW_DATA,

        INTPTR
    }

    public struct VideoFrameBufferConfig
    {
        public VIDEO_SOURCE_TYPE type;

        public uint id;

        public string key;
    }

    internal class AudioFrameInternal
    {
        internal AudioFrameInternal(AudioFrame audioFrame)
        {
            #region terra AudioFrameInternal_Assignment

            this.type = audioFrame.type;
            this.samplesPerChannel = audioFrame.samplesPerChannel;
            this.bytesPerSample = audioFrame.bytesPerSample;
            this.channels = audioFrame.channels;
            this.samplesPerSec = audioFrame.samplesPerSec;
            this.buffer = audioFrame.buffer;
            this.renderTimeMs = audioFrame.renderTimeMs;
            this.avsync_type = audioFrame.avsync_type;
            this.presentationMs = audioFrame.presentationMs;
            this.audioTrackNumber = audioFrame.audioTrackNumber;
            #endregion terra AudioFrameInternal_Assignment
        }

        #region terra AudioFrameInternal_Member_List

        public AUDIO_FRAME_TYPE type;
        public int samplesPerChannel;
        public BYTES_PER_SAMPLE bytesPerSample;
        public int channels;
        public int samplesPerSec;
        public IntPtr buffer;
        public long renderTimeMs;
        public int avsync_type;
        public long presentationMs;
        public int audioTrackNumber;
        #endregion terra AudioFrameInternal_Member_List
    }

    internal class ExternalVideoFrameInternal
    {

        internal ExternalVideoFrameInternal(ExternalVideoFrame frame)
        {
            #region terra ExternalVideoFrameInternal_Assignment

            this.type = frame.type;
            this.format = frame.format;

            this.stride = frame.stride;
            this.height = frame.height;
            this.cropLeft = frame.cropLeft;
            this.cropTop = frame.cropTop;
            this.cropRight = frame.cropRight;
            this.cropBottom = frame.cropBottom;
            this.rotation = frame.rotation;
            this.timestamp = frame.timestamp;

            this.eglType = frame.eglType;
            this.textureId = frame.textureId;
            this.matrix = frame.matrix;

            this.metadata_size = frame.metadata_size;

            this.d3d11_texture_2d = frame.d3d11_texture_2d;
            this.texture_slice_index = frame.texture_slice_index;
            #endregion terra ExternalVideoFrameInternal_Assignment
        }

        #region terra ExternalVideoFrameInternal_Member_List

        public VIDEO_BUFFER_TYPE type;
        public VIDEO_PIXEL_FORMAT format;

        public int stride;
        public int height;
        public int cropLeft;
        public int cropTop;
        public int cropRight;
        public int cropBottom;
        public int rotation;
        public long timestamp;

        public EGL_CONTEXT_TYPE eglType;
        public int textureId;
        public float[] matrix;

        public int metadata_size;

        public IntPtr d3d11_texture_2d;
        public int texture_slice_index;
        #endregion terra ExternalVideoFrameInternal_Member_List
    }

    ///
    /// <summary>
    /// Video frame formats.
    /// </summary>
    ///
    public enum VIDEO_OBSERVER_FRAME_TYPE
    {
        ///
        /// @ignore
        ///
        FRAME_TYPE_DEFAULT = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_DEFAULT,

        ///
        /// @ignore
        ///
        FRAME_TYPE_YUV420 = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_I420,

        ///
        /// @ignore
        ///
        FRAME_TYPE_BGRA = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_BGRA,

        ///
        /// @ignore
        ///
        FRAME_TYPE_NV21 = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_NV21,

        ///
        /// <summary>
        /// 2: The format of the video frame is RGBA.
        /// </summary>
        ///
        FRAME_TYPE_RGBA = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA,

        ///
        /// @ignore
        ///
        FRAME_TYPE_NV12 = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_NV12,

        ///
        /// @ignore
        ///
        FRAME_TYPE_TEXTURE_2D = VIDEO_PIXEL_FORMAT.VIDEO_TEXTURE_2D,

        ///
        /// @ignore
        ///
        FRAME_TYPE_TEXTURE_OES = VIDEO_PIXEL_FORMAT.VIDEO_TEXTURE_OES,

        ///
        /// @ignore
        ///
        FRAME_TYPE_CVPIXEL_NV12 = VIDEO_PIXEL_FORMAT.VIDEO_CVPIXEL_NV12,

        ///
        /// @ignore
        ///
        FRAME_TYPE_CVPIXEL_I420 = VIDEO_PIXEL_FORMAT.VIDEO_CVPIXEL_I420,

        ///
        /// @ignore
        ///
        FRAME_TYPE_CVPIXEL_BGRA = VIDEO_PIXEL_FORMAT.VIDEO_CVPIXEL_BGRA,

        ///
        /// @ignore
        ///
        FRAME_TYPE_YUV422 = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_I422,

        FRAME_TYPE_TEXTURE_ID3D11TEXTURE2D = VIDEO_PIXEL_FORMAT.VIDEO_TEXTURE_ID3D11TEXTURE2D,
    }

    internal class IrisAudioSpectrumData
    {
        public IrisAudioSpectrumData()
        {
            audioSpectrumData = 0;
            dataLength = 0;
        }

        public ulong audioSpectrumData;
        public int dataLength;

        public void GenerateAudioSpectrumData(ref AudioSpectrumData audioSpectrum)
        {
            audioSpectrum.audioSpectrumData = new float[this.dataLength];
            Marshal.Copy((IntPtr)this.audioSpectrumData, audioSpectrum.audioSpectrumData, 0, dataLength);
            audioSpectrum.dataLength = this.dataLength;
        }
    }

    internal class IrisUserAudioSpectrumInfo
    {
        public IrisUserAudioSpectrumInfo()
        {
            uid = 0;
            spectrumData = null;
        }

        public uint uid;
        public IrisAudioSpectrumData spectrumData;

        public void GenerateUserAudioSpectrumInfo(ref UserAudioSpectrumInfo info)
        {
            info.uid = this.uid;
            info.spectrumData = new AudioSpectrumData();
            this.spectrumData.GenerateAudioSpectrumData(ref info.spectrumData);
        }
    }

    internal class IrisUserAudioSpectrumInfoS
    {
        public IrisUserAudioSpectrumInfoS()
        {
            userAccount = "";
            spectrumData = null;
        }

        public string userAccount;
        public IrisAudioSpectrumData spectrumData;

        public void GenerateUserAudioSpectrumInfo(ref UserAudioSpectrumInfoS info)
        {
            info.userAccount = this.userAccount;
            info.spectrumData = new AudioSpectrumData();
            this.spectrumData.GenerateAudioSpectrumData(ref info.spectrumData);
        }
    }

}