using System;

namespace agora.rtc
{

    public abstract class IAgoraRtcEngine : IRtcEngine
    {
    }
    public abstract class IRtcEngine
    {
        public abstract int Initialize(RtcEngineContext context);

        public abstract void Dispose(bool sync = false);

        public abstract AgoraRtcEngineEventHandler GetAgoraRtcEngineEventHandler();

        public abstract void InitEventHandler(IAgoraRtcEngineEventHandler engineEventHandler);

        public abstract void RemoveEventHandler(IAgoraRtcEngineEventHandler engineEventHandler);

        public abstract void RegisterAudioFrameObserver(IAgoraRtcAudioFrameObserver audioFrameObserver);

        public abstract void UnRegisterAudioFrameObserver();

        public abstract void RegisterVideoFrameObserver(IAgoraRtcVideoFrameObserver videoFrameObserver);

        public abstract void UnRegisterVideoFrameObserver();

        public abstract void RegisterVideoEncodedImageReceiver(IAgoraRtcVideoEncodedImageReceiver videoEncodedImageReceiver);

        public abstract void UnRegisterVideoEncodedImageReceiver();

        public abstract IAgoraRtcAudioRecordingDeviceManager GetAgoraRtcAudioRecordingDeviceManager();

        public abstract IAgoraRtcAudioPlaybackDeviceManager GetAgoraRtcAudioPlaybackDeviceManager();

        public abstract IAgoraRtcVideoDeviceManager GetAgoraRtcVideoDeviceManager();

        public abstract IAgoraRtcMediaPlayer GetAgoraRtcMediaPlayer();

        public abstract IAgoraRtcCloudSpatialAudioEngine GetAgoraRtcCloudSpatialAudioEngine();

        public abstract IAgoraRtcSpatialAudioEngine GetAgoraRtcSpatialAudioEngine();

        public abstract string GetVersion();

        public abstract string GetErrorDescription(int code);

        public abstract int JoinChannel(string token, string channelId, string info = "", uint uid = 0);

        public abstract int JoinChannel(string token, string channelId, uint uid, ChannelMediaOptions options);

        public abstract int UpdateChannelMediaOptions(ChannelMediaOptions options);

        public abstract int LeaveChannel();

        public abstract int LeaveChannel(LeaveChannelOptions options);

        public abstract int RenewToken(string token);

        public abstract int SetChannelProfile(CHANNEL_PROFILE_TYPE profile);

        public abstract int SetClientRole(CLIENT_ROLE_TYPE role);

        public abstract int SetClientRole(CLIENT_ROLE_TYPE role, ref ClientRoleOptions options);

        public abstract int StartEchoTest();

        public abstract int StartEchoTest(int intervalInSeconds);

        public abstract int StopEchoTest();

        public abstract int EnableVideo();

        public abstract int DisableVideo();

        public abstract int StartPreview();

        public abstract int StartPreview(VIDEO_SOURCE_TYPE sourceType);

        public abstract int StopPreview();

        public abstract int StopPreview(VIDEO_SOURCE_TYPE sourceType);

        public abstract int StartLastmileProbeTest(LastmileProbeConfig config);

        public abstract int StopLastmileProbeTest();

        public abstract int SetVideoEncoderConfiguration(VideoEncoderConfiguration config);

        public abstract int SetBeautyEffectOptions(bool enabled, BeautyOptions options);

        public abstract int SetupRemoteVideo(VideoCanvas canvas);

        public abstract int SetupLocalVideo(VideoCanvas canvas);

        public abstract int EnableAudio();

        public abstract int DisableAudio();

        public abstract int SetAudioProfile(AUDIO_PROFILE_TYPE profile, AUDIO_SCENARIO_TYPE scenario);

        public abstract int SetAudioProfile(AUDIO_PROFILE_TYPE profile);

        public abstract int EnableLocalAudio(bool enabled);

        public abstract int MuteLocalAudioStream(bool mute);

        public abstract int MuteAllRemoteAudioStreams(bool mute);

        public abstract int SetDefaultMuteAllRemoteAudioStreams(bool mute);

        public abstract int MuteRemoteAudioStream(uint uid, bool mute);

        public abstract int MuteLocalVideoStream(bool mute);

        public abstract int EnableLocalVideo(bool enabled);

        public abstract int MuteAllRemoteVideoStreams(bool mute);

        public abstract int SetDefaultMuteAllRemoteVideoStreams(bool mute);

        public abstract int MuteRemoteVideoStream(uint uid, bool mute);

        public abstract int SetRemoteVideoStreamType(uint uid, VIDEO_STREAM_TYPE streamType);

        public abstract int SetRemoteDefaultVideoStreamType(VIDEO_STREAM_TYPE streamType);

        public abstract int EnableAudioVolumeIndication(int interval, int smooth, bool reportVad);

        public abstract int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality);

        public abstract int StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality);

        public abstract int StartAudioRecording(AudioRecordingConfiguration config);

        public abstract void RegisterAudioEncodedFrameObserver(AudioEncodedFrameObserverConfig config, IAgoraRtcAudioEncodedFrameObserver observer); //TODO

        public abstract void UnRegisterAudioEncodedFrameObserver();

        public abstract int StopAudioRecording();

        public abstract int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle);

        public abstract int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle, int startPos);

        public abstract int StopAudioMixing();

        public abstract int PauseAudioMixing();

        public abstract int ResumeAudioMixing();

        public abstract int AdjustAudioMixingVolume(int volume);

        public abstract int AdjustAudioMixingPublishVolume(int volume);

        public abstract int GetAudioMixingPublishVolume();

        public abstract int AdjustAudioMixingPlayoutVolume(int volume);

        public abstract int GetAudioMixingPlayoutVolume();

        public abstract int GetAudioMixingDuration();

        public abstract int GetAudioMixingCurrentPosition();

        public abstract int SetAudioMixingPosition(int pos /*in ms*/);

        public abstract int SetAudioMixingPitch(int pitch);

        public abstract int GetEffectsVolume();

        public abstract int SetEffectsVolume(int volume);

        public abstract int PreloadEffect(int soundId, string filePath, int startPos = 0);

        public abstract int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish = false, int startPos = 0);

        public abstract int PlayAllEffects(int loopCount, double pitch, double pan, int gain, bool publish = false);

        public abstract int GetVolumeOfEffect(int soundId);

        public abstract int SetVolumeOfEffect(int soundId, int volume);

        public abstract int PauseEffect(int soundId);

        public abstract int PauseAllEffects();

        public abstract int ResumeEffect(int soundId);

        public abstract int ResumeAllEffects();

        public abstract int StopEffect(int soundId);

        public abstract int StopAllEffects();

        public abstract int UnloadEffect(int soundId);

        public abstract int UnloadAllEffects();

        public abstract int EnableSoundPositionIndication(bool enabled);

        public abstract int SetRemoteVoicePosition(uint uid, double pan, double gain);

        public abstract int EnableSpatialAudio(bool enabled);

        public abstract int SetRemoteUserSpatialAudioParams(uint uid, SpatialAudioParams param);

        public abstract int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset);

        public abstract int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset);

        public abstract int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset);

        public abstract int SetAudioEffectParameters(AUDIO_EFFECT_PRESET preset, int param1, int param2);

        public abstract int SetVoiceBeautifierParameters(VOICE_BEAUTIFIER_PRESET preset, int param1, int param2);

        public abstract int SetVoiceConversionParameters(VOICE_CONVERSION_PRESET preset, int param1, int param2);

        public abstract int SetLocalVoicePitch(double pitch);

        public abstract int SetLocalVoiceEqualization(AUDIO_EQUALIZATION_BAND_FREQUENCY bandFrequency, int bandGain);

        public abstract int SetLocalVoiceReverb(AUDIO_REVERB_TYPE reverbKey, int value);

        //public abstract int SetLocalVoiceReverbPreset(AUDIO_REVERB_PRESET reverbPreset);

        //public abstract int SetLocalVoiceChanger(VOICE_CHANGER_PRESET voiceChanger);

        public abstract int SetLogFile(string filePath);

        public abstract int SetLogFilter(uint filter);

        public abstract int SetLogLevel(LOG_LEVEL level);

        public abstract int SetLogFileSize(uint fileSizeInKBytes);

        public abstract int SetLocalRenderMode(RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode);

        public abstract int SetRemoteRenderMode(uint uid, RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode);

        public abstract int SetLocalRenderMode(RENDER_MODE_TYPE renderMode);

        public abstract int SetLocalVideoMirrorMode(VIDEO_MIRROR_MODE_TYPE mirrorMode);

        public abstract int EnableDualStreamMode(bool enabled);

        public abstract int EnableDualStreamMode(VIDEO_SOURCE_TYPE sourceType, bool enabled);

        public abstract int EnableDualStreamMode(VIDEO_SOURCE_TYPE sourceType, bool enabled, SimulcastStreamConfig streamConfig);

        public abstract int SetExternalAudioSink(int sampleRate, int channels);

        public abstract int StartPrimaryCustomAudioTrack(AudioTrackConfig config);

        public abstract int StopPrimaryCustomAudioTrack();

        public abstract int StartSecondaryCustomAudioTrack(AudioTrackConfig config);

        public abstract int StopSecondaryCustomAudioTrack();

        public abstract int SetRecordingAudioFrameParameters(int sampleRate, int channel, RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall);

        public abstract int SetPlaybackAudioFrameParameters(int sampleRate, int channel, RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall);

        public abstract int SetMixedAudioFrameParameters(int sampleRate, int channel, int samplesPerCall);

        public abstract int SetPlaybackAudioFrameBeforeMixingParameters(int sampleRate, int channel);

        public abstract int EnableAudioSpectrumMonitor(int intervalInMS = 100);

        public abstract int DisableAudioSpectrumMonitor();

        public abstract void RegisterAudioSpectrumObserver(IAgoraRtcAudioSpectrumObserver observer);

        public abstract void UnregisterAudioSpectrumObserver(IAgoraRtcAudioSpectrumObserver observer);

        public abstract int AdjustRecordingSignalVolume(int volume);

        public abstract int MuteRecordingSignal(bool mute);

        public abstract int AdjustPlaybackSignalVolume(int volume);

        public abstract int AdjustUserPlaybackSignalVolume(uint uid, int volume);

        public abstract int EnableLoopbackRecording(bool enabled);

        public abstract int AdjustLoopbackRecordingVolume(int volume);

        public abstract int GetLoopbackRecordingVolume();

        public abstract int EnableInEarMonitoring(bool enabled, int includeAudioFilters);

        public abstract int SetInEarMonitoringVolume(int volume);

        public abstract int LoadExtensionProvider(string extension_lib_path);

        public abstract int SetExtensionProviderProperty(string provider, string key, string value);

        public abstract int EnableExtension(string provider, string extension, bool enable = true);

        public abstract int SetExtensionProperty(string provider, string extension, string key, string value);

        public abstract int GetExtensionProperty(string provider, string extension, string key, ref string value, int buf_len, MEDIA_SOURCE_TYPE type = MEDIA_SOURCE_TYPE.UNKNOWN_MEDIA_SOURCE);

        public abstract int SetCameraCapturerConfiguration(CameraCapturerConfiguration config);

        public abstract int SwitchCamera();

        public abstract bool IsCameraZoomSupported();

        public abstract bool IsCameraFaceDetectSupported();

        public abstract bool IsCameraTorchSupported();

        public abstract bool IsCameraFocusSupported();

        public abstract bool IsCameraAutoFocusFaceModeSupported();

        public abstract int SetCameraZoomFactor(float factor);

        public abstract int EnableFaceDetection(bool enabled);

        public abstract float GetCameraMaxZoomFactor();

        public abstract int SetCameraFocusPositionInPreview(float positionX, float positionY);

        public abstract int SetCameraTorchOn(bool isOn);

        public abstract int SetCameraAutoFocusFaceModeEnabled(bool enabled);

        public abstract bool IsCameraExposurePositionSupported();

        public abstract int SetCameraExposurePosition(float positionXinView, float positionYinView);

        public abstract bool IsCameraAutoExposureFaceModeSupported();

        public abstract int SetCameraAutoExposureFaceModeEnabled(bool enabled);

        public abstract int SetDefaultAudioRouteToSpeakerphone(bool defaultToSpeaker);

        public abstract int SetEnableSpeakerphone(bool speakerOn);

        public abstract bool IsSpeakerphoneEnabled();

        public abstract int StartScreenCaptureByDisplayId(uint displayId, Rectangle regionRect, ScreenCaptureParameters captureParams);

        public abstract int StartScreenCaptureByScreenRect(Rectangle screenRect, Rectangle regionRect, ScreenCaptureParameters captureParams);

        public abstract int StartScreenCapture(byte[] mediaProjectionPermissionResultData, ScreenCaptureParameters captureParams);

        public abstract int StartScreenCaptureByWindowId(UInt64 windowId, Rectangle regionRect, ScreenCaptureParameters captureParams);

        public abstract int SetScreenCaptureContentHint(VIDEO_CONTENT_HINT contentHint);

        public abstract int UpdateScreenCaptureRegion(Rectangle regionRect);

        public abstract int UpdateScreenCaptureParameters(ScreenCaptureParameters captureParams);

        public abstract int StopScreenCapture();

        public abstract string GetCallId();

        public abstract int Rate(string callId, int rating, string description);

        public abstract int Complain(string callId, string description);

        public abstract int AddPublishStreamUrl(string url, bool transcodingEnabled);

        public abstract int RemovePublishStreamUrl(string url);

        public abstract int SetLiveTranscoding(LiveTranscoding transcoding);

        public abstract int StartLocalVideoTranscoder(LocalTranscoderConfiguration config);

        public abstract int UpdateLocalTranscoderConfiguration(LocalTranscoderConfiguration config);

        public abstract int StopLocalVideoTranscoder();

        public abstract int StartPrimaryCameraCapture(CameraCapturerConfiguration config);

        public abstract int StartSecondaryCameraCapture(CameraCapturerConfiguration config);

        public abstract int StopPrimaryCameraCapture();

        public abstract int StopSecondaryCameraCapture();

        public abstract int SetCameraDeviceOrientation(VIDEO_SOURCE_TYPE type, VIDEO_ORIENTATION orientation);

        public abstract int SetScreenCaptureOrientation(VIDEO_SOURCE_TYPE type, VIDEO_ORIENTATION orientation);

        public abstract int StartPrimaryScreenCapture(ScreenCaptureConfiguration config);

        public abstract int StartSecondaryScreenCapture(ScreenCaptureConfiguration config);

        public abstract int StopPrimaryScreenCapture();

        public abstract int StopSecondaryScreenCapture();

        public abstract CONNECTION_STATE_TYPE GetConnectionState();

        public abstract int SetRemoteUserPriority(uint uid, PRIORITY_TYPE userPriority);

        //public abstract int RegisterPacketObserver(IPacketObserver observer);

        public abstract int SetEncryptionMode(string encryptionMode);

        public abstract int SetEncryptionSecret(string secret);

        public abstract int EnableEncryption(bool enabled, EncryptionConfig config);

        public abstract int CreateDataStream(ref int streamId, bool reliable, bool ordered);

        public abstract int CreateDataStream(ref int streamId, DataStreamConfig config);

        public abstract int SendStreamMessage(int streamId, byte[] data, uint length);

        public abstract int AddVideoWatermark(RtcImage watermark);

        public abstract int AddVideoWatermark(string watermarkUrl, WatermarkOptions options);

        public abstract int ClearVideoWatermark();

        public abstract int ClearVideoWatermarks();

        public abstract int AddInjectStreamUrl(string url, InjectStreamConfig config);

        public abstract int RemoveInjectStreamUrl(string url);

        public abstract int PauseAudio();

        public abstract int ResumeAudio();

        public abstract int EnableWebSdkInteroperability(bool enabled);

        public abstract int SendCustomReportMessage(string id, string category, string @event, string label, int value);

        public abstract void RegisterMediaMetadataObserver(IMetadataObserver observer, METADATA_TYPE type);

        public abstract void UnregisterMediaMetadataObserver(IMetadataObserver observer);

        public abstract int StartAudioFrameDump(string channel_id, uint user_id, string location, string uuid, string passwd, long duration_ms, bool auto_upload);

        public abstract int StopAudioFrameDump(string channel_id, uint user_id, string location);

        public abstract int RegisterLocalUserAccount(string appId, string userAccount);

        public abstract int JoinChannelWithUserAccount(string token, string channelId, string userAccount);

        public abstract int JoinChannelWithUserAccount(string token, string channelId, string userAccount, ChannelMediaOptions options);

        public abstract int JoinChannelWithUserAccountEx(string token, string channelId, string userAccount, ChannelMediaOptions options, IAgoraRtcEngineEventHandler eventHandler);

        public abstract int GetUserInfoByUserAccount(string userAccount, out UserInfo userInfo);

        public abstract int GetUserInfoByUid(uint uid, out UserInfo userInfo);

        public abstract int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration);

        public abstract int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration);

        public abstract int StopChannelMediaRelay();

        public abstract int SetDirectCdnStreamingAudioConfiguration(AUDIO_PROFILE_TYPE profile);

        public abstract int SetDirectCdnStreamingVideoConfiguration(VideoEncoderConfiguration config);

        public abstract int StartDirectCdnStreaming(string publishUrl, DirectCdnStreamingMediaOptions options);

        public abstract int StopDirectCdnStreaming();

        public abstract int UpdateDirectCdnStreamingMediaOptions(DirectCdnStreamingMediaOptions options);

        public abstract int PushDirectCdnStreamingCustomVideoFrame(ExternalVideoFrame frame);

        public abstract int JoinChannelEx(string token, RtcConnection connection, ChannelMediaOptions options);

        public abstract int LeaveChannelEx(RtcConnection connection);

        public abstract int UpdateChannelMediaOptionsEx(ChannelMediaOptions options, RtcConnection connection);

        public abstract int SetVideoEncoderConfigurationEx(VideoEncoderConfiguration config, RtcConnection connection);

        public abstract int SetupRemoteVideoEx(VideoCanvas canvas, RtcConnection connection);

        public abstract int MuteRemoteAudioStreamEx(uint uid, bool mute, RtcConnection connection);

        public abstract int MuteRemoteVideoStreamEx(uint uid, bool mute, RtcConnection connection);

        public abstract int SetRemoteVoicePositionEx(uint uid, double pan, double gain, RtcConnection connection);

        public abstract int SetRemoteUserSpatialAudioParamsEx(uint uid, SpatialAudioParams param, RtcConnection connection);

        public abstract int SetRemoteRenderModeEx(uint uid, RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode, RtcConnection connection);

        public abstract int EnableLoopbackRecordingEx(bool enabled, RtcConnection connection);

        public abstract CONNECTION_STATE_TYPE GetConnectionStateEx(RtcConnection connection);

        public abstract int EnableEncryptionEx(RtcConnection connection, bool enabled, EncryptionConfig config);

        public abstract int CreateDataStreamEx(bool reliable, bool ordered, RtcConnection connection);

        public abstract int CreateDataStreamEx(DataStreamConfig config, RtcConnection connection);

        public abstract int SendStreamMessageEx(int streamId, byte[] data, uint length, RtcConnection connection);

        public abstract int AddVideoWatermarkEx(string watermarkUrl, WatermarkOptions options, RtcConnection connection);

        public abstract int ClearVideoWatermarkEx(RtcConnection connection);

        public abstract int SendCustomReportMessageEx(string id, string category, string @event, string label, int value, RtcConnection connection);

        public abstract int PushAudioFrame(MEDIA_SOURCE_TYPE type, AudioFrame frame, bool wrap = false, int sourceId = 0);

        public abstract int PullAudioFrame(AudioFrame frame);

        public abstract int SetExternalVideoSource(bool enabled, bool useTexture, EXTERNAL_VIDEO_SOURCE_TYPE sourceType);

        public abstract int SetExternalAudioSource(bool enabled, int sampleRate, int channels, int sourceNumber, bool localPlayback = false, bool publish = true);

        public abstract int PushVideoFrame(ExternalVideoFrame frame);

        public abstract int PushVideoFrame(ExternalVideoFrame frame, RtcConnection connection);

        public abstract int PushEncodedVideoImage(byte[] imageBuffer, uint length, EncodedVideoFrameInfo videoEncodedFrameInfo);

        public abstract int PushEncodedVideoImage(byte[] imageBuffer, uint length, EncodedVideoFrameInfo videoEncodedFrameInfo, RtcConnection connection);

        //public abstract int GetCertificateVerifyResult(string credential_buf, int credential_len, string certificate_buf, int certificate_len);

        public abstract int SetAudioSessionOperationRestriction(AUDIO_SESSION_OPERATION_RESTRICTION restriction);

        public abstract int AdjustCustomAudioPublishVolume(int sourceId, int volume);

        public abstract int AdjustCustomAudioPlayoutVolume(int sourceId, int volume);

        public abstract int SetParameters(string parameters);

        public abstract int GetAudioDeviceInfo(out DeviceInfo deviceInfo);

        public abstract int EnableCustomAudioLocalPlayback(int sourceId, bool enabled);

        public abstract int EnableVirtualBackground(bool enabled, VirtualBackgroundSource backgroundSource);

        public abstract int SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS option);

        public abstract int SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS option);

        public abstract int PauseAllChannelMediaRelay();

        public abstract int ResumeAllChannelMediaRelay();

        public abstract int EnableEchoCancellationExternal(bool enabled, int audioSourceDelay);

        public abstract int TakeSnapshot(SnapShotConfig config);

        //public abstract int EnableContentInspect(bool enabled, ContentInspectConfig config);

        public abstract int SwitchChannel(string token, string channel);

        public abstract int SwitchChannel(string token, string channel, ChannelMediaOptions options);

        public abstract int StartRhythmPlayer(string sound1, string sound2, AgoraRhythmPlayerConfig config);

        public abstract int StopRhythmPlayer();

        public abstract int ConfigRhythmPlayer(AgoraRhythmPlayerConfig config);

        //public abstract int SetRemoteVideoSubscriptionOptions(uint uid, VideoSubscriptionOptions options);

        //public abstract int SetRemoteVideoSubscriptionOptionsEx(uint uid, VideoSubscriptionOptions options, RtcConnection connection);

        public abstract int SetDirectExternalAudioSource(bool enable, bool localPlayback);

        public abstract int PushDirectAudioFrame(AudioFrame frame);

        public abstract int SetCloudProxy(CLOUD_PROXY_TYPE proxyType);

        public abstract int SetLocalAccessPoint(LocalAccessPointConfiguration config);

        public abstract int EnableFishCorrection(bool enabled, FishCorrectionParams @params);

        public abstract int SetAdvancedAudioOptions(AdvancedAudioOptions options);

        public abstract int SetAVSyncSource(string channelId, uint uid);

        public abstract int StartRtmpStreamWithoutTranscoding(string url);

        public abstract int StartRtmpStreamWithTranscoding(string url, LiveTranscoding transcoding);

        public abstract int UpdateRtmpTranscoding(LiveTranscoding transcoding);

        public abstract int StopRtmpStream(string url);

        public abstract int GetUserInfoByUserAccountEx(string userAccount, out UserInfo userInfo, RtcConnection connection);

        public abstract int GetUserInfoByUidEx(uint uid, out UserInfo userInfo, RtcConnection connection);

        public abstract int EnableRemoteSuperResolution(uint userId, bool enable);

        public abstract int SetContentInspect(ContentInspectConfig config);

        public abstract int SetRemoteVideoStreamTypeEx(uint uid, VIDEO_STREAM_TYPE streamType, RtcConnection connection);

        public abstract int EnableAudioVolumeIndicationEx(int interval, int smooth, bool reportVad, RtcConnection connection);

        public abstract int SetVideoProfileEx(int width, int height, int frameRate, int bitrate);

        public abstract int EnableDualStreamModeEx(VIDEO_SOURCE_TYPE sourceType, bool enabled, SimulcastStreamConfig streamConfig, RtcConnection connection);

        public abstract int AddPublishStreamUrlEx(string url, bool transcodingEnabled, RtcConnection connection);

        public abstract int UploadLogFile(ref string requestId);

        //public abstract int GetScreenCaptureSources(SIZE thumbSize, SIZE iconSize, bool includeScreen);
    };

    internal static partial class ObsoleteMethodWarning
    {
    }
}