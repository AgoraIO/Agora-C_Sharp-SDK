﻿using System;
using System.Runtime.InteropServices;
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
using AOT;
#endif

namespace agora.rtc
{
    internal static class AgoraRtcAudioEncodedFrameObserverNative
    {
        internal static IAgoraRtcAudioEncodedFrameObserver AudioEncodedFrameObserver = null;


        internal static EncodedAudioFrameInfo IrisEncodedAudioFrameInfo2EncodedAudioFrameInfo(ref IrisEncodedAudioFrameInfo from)
        {
            EncodedAudioFrameInfo to = new EncodedAudioFrameInfo();
            to.codec = from.codec;
            to.sampleRateHz = from.sampleRateHz;
            to.samplesPerChannel = from.samplesPerChannel;
            to.numberOfChannels = from.numberOfChannels;
            to.advancedSettings = new EncodedAudioFrameAdvancedSettings();
            to.advancedSettings.speech = from.advancedSettings.speech;
            to.advancedSettings.sendEvenIfEmpty = from.advancedSettings.sendEvenIfEmpty;
            return to;
        }


#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
        [MonoPInvokeCallback(typeof(Func_RecordAudioEncodedFrame_Native))]
#endif
        internal static void OnRecordAudioEncodedFrame(IntPtr frame_buffer, int length, IntPtr encoded_audio_frame_info)
        {
            if (AudioEncodedFrameObserver == null) return;

            byte[] frameBuffer = new byte[length];
            Marshal.Copy(frameBuffer, 0, frame_buffer, length);

            IrisEncodedAudioFrameInfo from = (IrisEncodedAudioFrameInfo)Marshal.PtrToStructure(encoded_audio_frame_info);
            EncodedAudioFrameInfo to = IrisEncodedAudioFrameInfo2EncodedAudioFrameInfo(ref from);

            AudioEncodedFrameObserver.OnRecordAudioEncodedFrame(frameBuffer, length, to);
        }

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
        [MonoPInvokeCallback(typeof(Func_RecordAudioEncodedFrame_Native))]
#endif
        internal static void OnPlaybackAudioEncodedFrame(IntPtr frame_buffer, int length, IntPtr encoded_audio_frame_info)
        {
            if (AudioEncodedFrameObserver == null) return;

            byte[] frameBuffer = new byte[length];
            Marshal.Copy(frameBuffer, 0, frame_buffer, length);

            IrisEncodedAudioFrameInfo from = (IrisEncodedAudioFrameInfo)Marshal.PtrToStructure(encoded_audio_frame_info);
            EncodedAudioFrameInfo to = IrisEncodedAudioFrameInfo2EncodedAudioFrameInfo(ref from);

            AudioEncodedFrameObserver.OnPlaybackAudioEncodedFrame(frameBuffer, length, to);
        }

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
        [MonoPInvokeCallback(typeof(Func_RecordAudioEncodedFrame_Native))]
#endif
        internal static void OnMixedAudioEncodedFrame(IntPtr frame_buffer, int length, IntPtr encoded_audio_frame_info)
        {
            if (AudioEncodedFrameObserver == null) return;

            byte[] frameBuffer = new byte[length];
            Marshal.Copy(frameBuffer, 0, frame_buffer, length);

            IrisEncodedAudioFrameInfo from = (IrisEncodedAudioFrameInfo)Marshal.PtrToStructure(encoded_audio_frame_info);
            EncodedAudioFrameInfo to = IrisEncodedAudioFrameInfo2EncodedAudioFrameInfo(ref from);

            AudioEncodedFrameObserver.OnMixedAudioEncodedFrame(frameBuffer, length, to);
        }
    }
}
