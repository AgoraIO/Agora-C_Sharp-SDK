﻿using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
using AOT;
#endif

namespace Agora.Rtc
{
    //get from alloc, need to free
    using IrisEventHandlerMarshal = IntPtr;
    //get from C++, no need to free
    using IrisEventHandlerHandle = IntPtr;

    using IrisApiEnginePtr = IntPtr;


    internal class MediaPlayerImpl
    {
        private bool _disposed = false;

        private IrisApiEnginePtr _irisApiEngine;

        private IrisCApiParam _apiParam;

        private EventHandlerHandle _mediaPlayerEventHandlerHandle = new EventHandlerHandle();

        //audioFrameObserver
        private Dictionary<int, EventHandlerHandle> _mediaPlayerAudioFrameObserverHandles = new Dictionary<int, EventHandlerHandle>();

        //openWithCustomSource
        private Dictionary<int, EventHandlerHandle> _mediaPlayerCustomProviderHandles = new Dictionary<int, EventHandlerHandle>();

        //openWithMediaSource
        private Dictionary<int, EventHandlerHandle> _mediaPlayerMediaProviderHandles = new Dictionary<int, EventHandlerHandle>();

        //AudioSpectrumObserver
        private Dictionary<int, EventHandlerHandle> _mediaPlayerAudioSpectrumObserverHandles = new Dictionary<int, EventHandlerHandle>();

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID
        private AgoraCallbackObject _callbackObject;
        private static readonly string identifier = "AgoraMediaPlayer";
#endif

        private List<T> GetDicKeys<T, D>(Dictionary<T, D> dic)
        {
            List<T> list = new List<T>();
            foreach (var e in dic)
            {
                list.Add(e.Key);
            }

            return list;
        }


        internal MediaPlayerImpl(IrisApiEnginePtr irisApiEngine)
        {
            _apiParam = new IrisCApiParam();
            _irisApiEngine = irisApiEngine;
        }

        ~MediaPlayerImpl()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                ReleaseEventHandler();


                var keys = GetDicKeys<int, EventHandlerHandle>(_mediaPlayerAudioFrameObserverHandles);
                foreach (var playerId in keys)
                {
                    this.UnSetIrisAudioFrameObserver(playerId);
                }

                keys = GetDicKeys<int, EventHandlerHandle>(_mediaPlayerCustomProviderHandles);
                foreach (var playerId in keys)
                {
                    this.UnSetMediaPlayerOpenWithCustomSource(playerId);
                }

                keys = GetDicKeys<int, EventHandlerHandle>(this._mediaPlayerMediaProviderHandles);
                foreach (var playerId in keys)
                {
                    this.UnsetMediaPlayerOpenWithMediaSource(playerId);
                }

                keys = GetDicKeys<int, EventHandlerHandle>(this._mediaPlayerAudioSpectrumObserverHandles);
                foreach (var playerId in keys)
                {
                    this.UnSetIrisAudioSpectrumObserver(playerId);
                }

            }

            _irisApiEngine = IntPtr.Zero;
            _apiParam = new IrisCApiParam();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void CreateEventHandler()
        {
            if (_mediaPlayerEventHandlerHandle.handle != IntPtr.Zero) return;

            AgoraUtil.AllocEventHandlerHandle(ref _mediaPlayerEventHandlerHandle, MediaPlayerSourceObserverNative.OnEvent);
            IntPtr[] arrayPtr = new IntPtr[] { _mediaPlayerEventHandlerHandle.handle };
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_REGISTERPLAYERSOURCEOBSERVER,
                "{}", 2,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_REGISTERPLAYERSOURCEOBSERVER failed: " + nRet);
            }


#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID
            _callbackObject = new AgoraCallbackObject(identifier);
            MediaPlayerSourceObserverNative.CallbackObject = _callbackObject;
#endif

        }

        private void ReleaseEventHandler()
        {
            if (_mediaPlayerEventHandlerHandle.handle == IntPtr.Zero) return;

            MediaPlayerSourceObserverNative.ClearSourceObserver();

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID 
            MediaPlayerSourceObserverNative.CallbackObject = null;
            if (_callbackObject != null) _callbackObject.Release();
            _callbackObject = null;
#endif

            IntPtr[] arrayPtr = new IntPtr[] { _mediaPlayerEventHandlerHandle.handle };
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_UNREGISTERPLAYERSOURCEOBSERVER,
                "{}", 2,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_UNREGISTERPLAYERSOURCEOBSERVER failed: " + nRet);
            }

            AgoraUtil.FreeEventHandlerHandle(ref _mediaPlayerEventHandlerHandle);

        }

        private void SetIrisAudioFrameObserver(int playerId)
        {
            if (_mediaPlayerAudioFrameObserverHandles.ContainsKey(playerId) == true) return;

            var mediaPlayerAudioFrameObserverHandle = new EventHandlerHandle();
            AgoraUtil.AllocEventHandlerHandle(ref mediaPlayerAudioFrameObserverHandle, MediaPlayerAudioFrameObserverNative.OnEvent);
            IntPtr[] arrayPtr = new IntPtr[] { mediaPlayerAudioFrameObserverHandle.handle };
            var param = new { playerId };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_REGISTERAUDIOFRAMEOBSERVER,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam); ;

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_REGISTERAUDIOFRAMEOBSERVER failed: " + nRet);
            }

            _mediaPlayerAudioFrameObserverHandles.Add(playerId, mediaPlayerAudioFrameObserverHandle);
        }

        private void SetIrisAudioFrameObserverWithMode(int playerId, RAW_AUDIO_FRAME_OP_MODE_TYPE mode)
        {
            if (_mediaPlayerAudioFrameObserverHandles.ContainsKey(playerId) == true) return;

            var mediaPlayerAudioFrameObserverHandle = new EventHandlerHandle();
            AgoraUtil.AllocEventHandlerHandle(ref mediaPlayerAudioFrameObserverHandle, MediaPlayerAudioFrameObserverNative.OnEvent);

            var param = new { playerId, mode };
            var json = AgoraJson.ToJson(param);
            IntPtr[] arrayPtr = new IntPtr[] { mediaPlayerAudioFrameObserverHandle.handle };
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_REGISTERAUDIOFRAMEOBSERVER,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_REGISTERAUDIOFRAMEOBSERVER failed: " + nRet);
            }
            _mediaPlayerAudioFrameObserverHandles.Add(playerId, mediaPlayerAudioFrameObserverHandle);
        }

        private void UnSetIrisAudioFrameObserver(int playerId)
        {
            if (_mediaPlayerAudioFrameObserverHandles.ContainsKey(playerId) == false) return;

            var mediaPlayerAudioFrameObserverHandle = _mediaPlayerAudioFrameObserverHandles[playerId];
            IntPtr[] arrayPtr = new IntPtr[] { mediaPlayerAudioFrameObserverHandle.handle };
            var param = new { playerId };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_UNREGISTERAUDIOFRAMEOBSERVER,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_UNREGISTERAUDIOFRAMEOBSERVER failed: " + nRet);
            }

            AgoraUtil.FreeEventHandlerHandle(ref mediaPlayerAudioFrameObserverHandle);
            _mediaPlayerAudioFrameObserverHandles.Remove(playerId);
        }

        private int SetMediaPlayerOpenWithCustomSource(int playerId, Int64 startPos)
        {
            if (_mediaPlayerCustomProviderHandles.ContainsKey(playerId))
                return -(int)ERROR_CODE_TYPE.ERR_ALREADY_IN_USE;

            EventHandlerHandle eventHandlerHandle = new EventHandlerHandle();
            AgoraUtil.AllocEventHandlerHandle(ref eventHandlerHandle, MediaPlayerCustomDataProviderNative.OnEvent);

            var param = new { playerId, startPos };
            var json = AgoraJson.ToJson(param);
            IntPtr[] arrayPtr = new IntPtr[] { eventHandlerHandle.handle };

            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_OPENWITHCUSTOMSOURCE,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_OPENWITHCUSTOMSOURCE failed: " + nRet);
            }

            this._mediaPlayerCustomProviderHandles.Add(playerId, eventHandlerHandle);

            return 0;
        }


        private int UnSetMediaPlayerOpenWithCustomSource(int playerId)
        {
            if (_mediaPlayerCustomProviderHandles.ContainsKey(playerId) == false)
                return 0;

            var eventHandlerHandle = _mediaPlayerCustomProviderHandles[playerId];
            var param = new { playerId };
            var json = AgoraJson.ToJson(param);
            IntPtr[] arrayPtr = new IntPtr[] { eventHandlerHandle.handle };

            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_UNOPENWITHCUSTOMSOURCE,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            AgoraUtil.FreeEventHandlerHandle(ref eventHandlerHandle);
            this._mediaPlayerCustomProviderHandles.Remove(playerId);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_UNOPENWITHCUSTOMSOURCE failed: " + nRet);
            }

            return nRet;
        }

        private int SetMediaPlayerOpenWithMediaSource(int playerId, MediaSource source, bool hadProvider)
        {


            IntPtr[] arrayPtr = new IntPtr[1] { IntPtr.Zero };
            if (hadProvider)
            {
                EventHandlerHandle eventHandlerHandle = new EventHandlerHandle();
                AgoraUtil.AllocEventHandlerHandle(ref eventHandlerHandle, MediaPlayerCustomDataProviderNative.OnEvent);
                arrayPtr[0] = eventHandlerHandle.handle;
                this._mediaPlayerMediaProviderHandles.Add(playerId, eventHandlerHandle);
            }
            var param = new { playerId, source };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_OPENWITHMEDIASOURCE,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_OPENWITHMEDIASOURCE failed: " + nRet);
            }

            return nRet;
        }

        private int UnsetMediaPlayerOpenWithMediaSource(int playerId)
        {
            if (_mediaPlayerMediaProviderHandles.ContainsKey(playerId) == false)
                return 0;

            var eventHandlerHandle = _mediaPlayerMediaProviderHandles[playerId];
            IntPtr[] arrayPtr = new IntPtr[1] { eventHandlerHandle.handle };

            var param = new { playerId };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_UNOPENWITHMEDIASOURCE,
            json, (uint)json.Length,
            Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
            ref _apiParam);

            AgoraUtil.FreeEventHandlerHandle(ref eventHandlerHandle);
            this._mediaPlayerMediaProviderHandles.Remove(playerId);


            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_UNOPENWITHMEDIASOURCE failed: " + nRet);
            }

            return nRet;
        }

        private void SetIrisAudioSpectrumObserver(int playerId, int intervalInMS)
        {
            if (_mediaPlayerAudioSpectrumObserverHandles.ContainsKey(playerId) == true) return;

            var mediaPlayerAudioSpectrumObserverHandle = new EventHandlerHandle();
            AgoraUtil.AllocEventHandlerHandle(ref mediaPlayerAudioSpectrumObserverHandle, MediaPlayerAudioSpectrumObserverNative.OnEvent);
            IntPtr[] arrayPtr = new IntPtr[] { mediaPlayerAudioSpectrumObserverHandle.handle };
            var param = new {playerId, intervalInMS };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_REGISTERMEDIAPLAYERAUDIOSPECTRUMOBSERVER,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_REGISTERMEDIAPLAYERAUDIOSPECTRUMOBSERVER failed: " + nRet);
            }
            _mediaPlayerAudioSpectrumObserverHandles.Add(playerId, mediaPlayerAudioSpectrumObserverHandle);
        }

        private void UnSetIrisAudioSpectrumObserver(int playerId)
        {
            if (_mediaPlayerAudioSpectrumObserverHandles.ContainsKey(playerId) == false) return;

            var _mediaPlayerAudioSpectrumObserverHandle = _mediaPlayerAudioSpectrumObserverHandles[playerId];
            IntPtr[] arrayPtr = new IntPtr[] { _mediaPlayerAudioSpectrumObserverHandle.handle };
            var param = new { playerId };
            var json = AgoraJson.ToJson(param);
            var nRet = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine, AgoraApiType.FUNC_MEDIAPLAYER_UNREGISTERMEDIAPLAYERAUDIOSPECTRUMOBSERVER,
                json, (uint)json.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(arrayPtr, 0), 1,
                ref _apiParam);

            if (nRet != 0)
            {
                AgoraLog.LogError("FUNC_MEDIAPLAYER_UNREGISTERMEDIAPLAYERAUDIOSPECTRUMOBSERVER failed: " + nRet);
            }

            AgoraUtil.FreeEventHandlerHandle(ref _mediaPlayerAudioSpectrumObserverHandle);
            _mediaPlayerAudioSpectrumObserverHandles.Remove(playerId);
        }

        public void InitEventHandler(int playerId, IMediaPlayerSourceObserver engineEventHandler)
        {
            CreateEventHandler();
            if (engineEventHandler == null)
            {
                MediaPlayerSourceObserverNative.RemoveSourceObserver(playerId);
            }
            else
            {
                MediaPlayerSourceObserverNative.AddSourceObserver(playerId, engineEventHandler);
            }
        }

        public void RegisterAudioFrameObserver(int playerId, IMediaPlayerAudioFrameObserver observer)
        {
            UnSetIrisAudioFrameObserver(playerId);
            MediaPlayerAudioFrameObserverNative.RemoveAudioFrameObserver(playerId);

            SetIrisAudioFrameObserver(playerId);
            MediaPlayerAudioFrameObserverNative.AddAudioFrameObserver(playerId, observer);
        }

        public void RegisterAudioFrameObserver(int playerId, IMediaPlayerAudioFrameObserver observer, RAW_AUDIO_FRAME_OP_MODE_TYPE mode)
        {
            UnSetIrisAudioFrameObserver(playerId);
            MediaPlayerAudioFrameObserverNative.RemoveAudioFrameObserver(playerId);

            SetIrisAudioFrameObserverWithMode(playerId, mode);
            MediaPlayerAudioFrameObserverNative.AddAudioFrameObserver(playerId, observer);
        }

        public void UnregisterAudioFrameObserver(int playerId)
        {
            UnSetIrisAudioFrameObserver(playerId);
            MediaPlayerAudioFrameObserverNative.RemoveAudioFrameObserver(playerId);
        }

        public void RegisterMediaPlayerAudioSpectrumObserver(int playerId, IAudioSpectrumObserver observer, int intervalInMS)
        {
            UnSetIrisAudioSpectrumObserver(playerId);
            MediaPlayerAudioSpectrumObserverNative.RemoveAudioSpectrumObserver(playerId);

            SetIrisAudioSpectrumObserver(playerId, intervalInMS);
            MediaPlayerAudioSpectrumObserverNative.AddAudioSpectrumObserver(playerId, observer);
        }

        public void UnregisterMediaPlayerAudioSpectrumObserver(int playerId)
        {
            UnSetIrisAudioSpectrumObserver(playerId);
            MediaPlayerAudioSpectrumObserverNative.RemoveAudioSpectrumObserver(playerId);
        }

        public int CreateMediaPlayer()
        {
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_RTCENGINE_CREATEMEDIAPLAYER,
                "", 0, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int DestroyMediaPlayer(int playerId)
        {
            var param = new
            {
                playerId
            };
            string jsonParam = AgoraJson.ToJson(param);
            return AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_RTCENGINE_DESTROYMEDIAPLAYER,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
        }

        public int Open(int playerId, string url, Int64 startPos)
        {
            var param = new
            {
                playerId,
                url,
                startPos
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_OPEN,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int OpenWithCustomSource(int playerId, Int64 startPos, IMediaPlayerCustomDataProvider provider)
        {
            if (provider == null)
            {
                AgoraLog.LogError("provide can not set as null");
                return -(int)ERROR_CODE_TYPE.ERR_INVALID_ARGUMENT;
            }

            UnsetMediaPlayerOpenWithMediaSource(playerId);
            UnSetMediaPlayerOpenWithCustomSource(playerId);

            SetMediaPlayerOpenWithCustomSource(playerId, startPos);

            MediaPlayerCustomDataProviderNative.RemoveCustomDataProvider(playerId);
            MediaPlayerCustomDataProviderNative.AddCustomDataProvider(playerId, provider);

            return 0;
        }

        public int OpenWithMediaSource(int playerId, MediaSource source)
        {
            UnsetMediaPlayerOpenWithMediaSource(playerId);
            UnSetMediaPlayerOpenWithCustomSource(playerId);

            SetMediaPlayerOpenWithMediaSource(playerId, source, source.provider != null);

            var provider = source.provider;
            if (provider != null)
            {
                MediaPlayerCustomDataProviderNative.RemoveCustomDataProvider(playerId);
                MediaPlayerCustomDataProviderNative.AddCustomDataProvider(playerId, provider);
            }
            else
            {
                MediaPlayerCustomDataProviderNative.RemoveCustomDataProvider(playerId);
            }

            return 0;
        }

        public int SetSoundPositionParams(float pan, float gain)
        {
            var param = new
            {
                pan,
                gain,
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETSOUNDPOSITIONPARAMS,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }


        public int Play(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_PLAY,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int Pause(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_PAUSE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int Stop(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_STOP,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int Resume(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_RESUME,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int Seek(int playerId, Int64 newPos)
        {
            var param = new
            {
                playerId,
                newPos
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SEEK,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetDuration(int playerId, ref Int64 duration)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETDURATION,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            duration = (Int64)AgoraJson.GetData<Int64>(_apiParam.Result, "duration");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetPlayPosition(int playerId, ref Int64 pos)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETPLAYPOSITION,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            pos = (Int64)AgoraJson.GetData<Int64>(_apiParam.Result, "pos");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetStreamCount(int playerId, ref Int64 count)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETSTREAMCOUNT,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            count = (Int64)AgoraJson.GetData<Int64>(_apiParam.Result, "count");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetStreamInfo(int playerId, Int64 index, ref PlayerStreamInfo info)
        {
            var param = new
            {
                playerId,
                index
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETSTREAMINFO,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);

            info = ret != 0 ? new PlayerStreamInfo() : AgoraJson.JsonToStruct<PlayerStreamInfo>(_apiParam.Result, "info");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetLoopCount(int playerId, int loopCount)
        {
            var param = new
            {
                playerId,
                loopCount
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETLOOPCOUNT,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetPlaybackSpeed(int playerId, int speed)
        {
            var param = new
            {
                playerId,
                speed
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETPLAYBACKSPEED,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SelectAudioTrack(int playerId, int index)
        {
            var param = new
            {
                playerId,
                index
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SELECTAUDIOTRACK,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetPlayerOption(int playerId, string key, int value)
        {
            var param = new
            {
                playerId,
                key,
                value
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETPLAYEROPTION,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetPlayerOption(int playerId, string key, string value)
        {
            var param = new
            {
                playerId,
                key,
                value
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETPLAYEROPTION2,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int TakeScreenshot(int playerId, string filename)
        {
            var param = new
            {
                playerId,
                filename
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_TAKESCREENSHOT,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SelectInternalSubtitle(int playerId, int index)
        {
            var param = new
            {
                playerId,
                index
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SELECTINTERNALSUBTITLE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetExternalSubtitle(int playerId, string url)
        {
            var param = new
            {
                playerId,
                url
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETEXTERNALSUBTITLE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public MEDIA_PLAYER_STATE GetState(int playerId)
        {
            //TODO CHECK
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETSTATE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return (MEDIA_PLAYER_STATE)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int Mute(int playerId, bool muted)
        {
            var param = new
            {
                playerId,
                muted
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_MUTE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetMute(int playerId, ref bool muted)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETMUTE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            muted = (bool)AgoraJson.GetData<bool>(_apiParam.Result, "muted");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int AdjustPlayoutVolume(int playerId, int volume)
        {
            var param = new
            {
                playerId,
                volume
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_ADJUSTPLAYOUTVOLUME,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetPlayoutVolume(int playerId, ref int volume)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETPLAYOUTVOLUME,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            volume = (int)AgoraJson.GetData<int>(_apiParam.Result, "volume");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int AdjustPublishSignalVolume(int playerId, int volume)
        {
            var param = new
            {
                playerId,
                volume
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_ADJUSTPUBLISHSIGNALVOLUME,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetPublishSignalVolume(int playerId, ref int volume)
        {
            var param = new
            {
                playerId,
                volume
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETPUBLISHSIGNALVOLUME,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            volume = (int)AgoraJson.GetData<int>(_apiParam.Result, "volume");
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetView(int playerId)
        {
            var param = new
            {
                playerId
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETVIEW,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetRenderMode(int playerId, RENDER_MODE_TYPE renderMode)
        {
            var param = new
            {
                playerId,
                renderMode
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETRENDERMODE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetAudioDualMonoMode(int playerId, AUDIO_DUAL_MONO_MODE mode)
        {
            var param = new
            {
                playerId,
                mode
            };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETAUDIODUALMONOMODE,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public string GetPlayerSdkVersion(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETPLAYERSDKVERSION,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret.ToString() : (string)AgoraJson.GetData<string>(_apiParam.Result, "result");
        }

        public string GetPlaySrc(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETPLAYSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret.ToString() : (string)AgoraJson.GetData<string>(_apiParam.Result, "result");
        }

        public int SetAudioPitch(int playerId, int pitch)
        {
            var param = new { playerId, pitch };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETAUDIOPITCH,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SetSpatialAudioParams(int playerId, SpatialAudioParams spatial_audio_params)
        {
            var param = new { playerId, spatial_audio_params };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SETSPATIALAUDIOPARAMS,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int OpenWithAgoraCDNSrc(int playerId, string src, Int64 startPos)
        {
            var param = new { playerId, src, startPos };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_OPENWITHAGORACDNSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetAgoraCDNLineCount(int playerId)
        {
            var param = new { playerId };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETAGORACDNLINECOUNT,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SwitchAgoraCDNLineByIndex(int playerId, int index)
        {
            var param = new { playerId, index };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SWITCHAGORACDNLINEBYINDEX,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int GetCurrentAgoraCDNIndex(int playerId)
        {
            var param = new { playerId };
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_GETCURRENTAGORACDNINDEX,
                "", 0, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int EnableAutoSwitchAgoraCDN(int playerId, bool enable)
        {
            var param = new { playerId, enable };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_ENABLEAUTOSWITCHAGORACDN,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int RenewAgoraCDNSrcToken(int playerId, string token, Int64 ts)
        {
            var param = new { playerId, token, ts };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_RENEWAGORACDNSRCTOKEN,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SwitchAgoraCDNSrc(int playerId, string src, bool syncPts = false)
        {
            var param = new { playerId, src, syncPts };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SWITCHAGORACDNSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int SwitchSrc(int playerId, string src, bool syncPts = true)
        {
            var param = new { playerId, src, syncPts };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_SWITCHSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int PreloadSrc(int playerId, string src, Int64 startPos)
        {
            var param = new { playerId, src, startPos };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_PRELOADSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int PlayPreloadedSrc(int playerId, string src)
        {
            var param = new { playerId, src };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_PLAYPRELOADEDSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }

        public int UnloadSrc(int playerId, string src)
        {
            var param = new { playerId, src };
            string jsonParam = AgoraJson.ToJson(param);
            var ret = AgoraRtcNative.CallIrisApiWithArgs(_irisApiEngine,
                AgoraApiType.FUNC_MEDIAPLAYER_UNLOADSRC,
                jsonParam, (UInt32)jsonParam.Length, IntPtr.Zero, 0, ref _apiParam);
            return ret != 0 ? ret : (int)AgoraJson.GetData<int>(_apiParam.Result, "result");
        }
    }
}