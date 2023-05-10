﻿/// <summary>
/// [Send Message]Key Step：
/// 1. Create Engine and Initialize：（CreateAgoraRtcEngine、Initialize、[SetLogFile]、[InitEventHandler]）
/// 
/// 2. Join Channel：（[EnableAudio]、EnableVideo、JoinChannel）
/// 
/// 3. Send Message: sendStreamMessage
/// 
/// 4. Leave Channel：（LeaveChannel）
/// 
/// 5. Exit：（Dispose）
/// <summary>

using System;
using agora.rtc;

namespace CSharp_API_Example
{
    using System;
    using System.Runtime.InteropServices;

    public class RTT : IEngine
    {
        private string app_id_ = "";
        private string channel_id_ = "";
        private readonly string RTT_TAG = "[RTT] ";
        private readonly string agora_sdk_log_file_path_ = "agorasdk.log";
        private IAgoraRtcEngine rtc_engine_ = null;
        private IAgoraRtcEngineEventHandler event_handler_ = null;
        private IntPtr local_win_id_ = IntPtr.Zero;
        private IntPtr remote_win_id_ = IntPtr.Zero;
        private int data_stream_id_ = -1;
        public RTT(IntPtr localWindowId, IntPtr remoteWindowId)
        {
            local_win_id_ = localWindowId;
            remote_win_id_ = remoteWindowId;
        }

        internal override int Init(string appId, string channelId)
        {
            int ret = -1;
            app_id_ = appId;
            channel_id_ = channelId.Split(';').GetValue(0).ToString();

            if (null == rtc_engine_)
            {
                rtc_engine_ = AgoraRtcEngine.CreateAgoraRtcEngine();
            }
            event_handler_ = new RTTEventHandler(this);
            rtc_engine_.InitEventHandler(event_handler_);

            LogConfig log_config = new LogConfig(agora_sdk_log_file_path_);
            RtcEngineContext rtc_engine_ctx = new RtcEngineContext(app_id_, AREA_CODE.AREA_CODE_GLOB, log_config);
            ret = rtc_engine_.Initialize(rtc_engine_ctx);
            CSharpForm.dump_handler_(RTT_TAG + "Initialize", ret);                      
         
            DataStreamConfig config = new DataStreamConfig(false, false);
            data_stream_id_ = rtc_engine_.CreateDataStream(config);
            return ret;
        }

        internal override int UnInit()
        {
            int ret = -1;
            if (null != rtc_engine_)
            {
                ret = rtc_engine_.LeaveChannel();
                CSharpForm.dump_handler_(RTT_TAG + "LeaveChannel", ret);

                rtc_engine_.Dispose();
                rtc_engine_ = null;    
            }
            return ret;
        }

        internal override int JoinChannel()
        {
            int ret = -1;
            if (null != rtc_engine_)
            {
                ret = rtc_engine_.EnableAudio();
                CSharpForm.dump_handler_(RTT_TAG + "EnableAudio", ret);

                ret = rtc_engine_.EnableVideo();
                CSharpForm.dump_handler_(RTT_TAG + "EnableVideo", ret);

                ret = rtc_engine_.JoinChannel("", channel_id_, "info");
                CSharpForm.dump_handler_(RTT_TAG + "JoinChannel", ret);
            }
            return ret;
        }

        internal override int LeaveChannel()
        {
            int ret = -1;
            if (null != rtc_engine_)
            {
                ret = rtc_engine_.LeaveChannel();
                CSharpForm.dump_handler_(RTT_TAG + "LeaveChannel", ret);
            }
            return ret;
        }

        internal override string GetSDKVersion()
        {
            if (null == rtc_engine_)
                return "-" + (ERROR_CODE_TYPE.ERR_NOT_INITIALIZED).ToString();

            return rtc_engine_.GetVersion();
        }

        internal override IAgoraRtcEngine GetEngine()
        {
            return rtc_engine_;
        }

        internal string GetChannelId()
        {
            return channel_id_;
        }

        internal IntPtr GetLocalWinId()
        {
            return local_win_id_;
        }

        internal IntPtr GetRemoteWinId()
        {
            return remote_win_id_;
        }

        public override int SendStreamMessage(string str)
        {
            if (null == rtc_engine_)
                return 0;
            int ret = rtc_engine_.SendStreamMessage(data_stream_id_, System.Text.Encoding.Default.GetBytes(str));
            CSharpForm.dump_handler_(RTT_TAG + "SendStreamMessage", ret);
            return ret;
        }

        public  void onRecevMessage(string message)
        {
            CSharpForm.dump_handler_(RTT_TAG + "OnStreamMessage", 0);
           
        }
    }

    // override if need
    internal class RTTEventHandler : IAgoraRtcEngineEventHandler
    {
        private RTT rtt_inst_ = null;

        public RTTEventHandler(RTT _StreamMessage) {
            rtt_inst_ = _StreamMessage;
        }

        public override void OnWarning(int warn, string msg)
        {
            Console.WriteLine("=====>OnWarning {0} {1}", warn, msg);
        }

        public override void OnError(int error, string msg)
        {
            Console.WriteLine("=====>OnError {0} {1}", error, msg);
        }

        public override void OnJoinChannelSuccess(string channel, uint uid, int elapsed)
        {
            Console.WriteLine("----->OnJoinChannelSuccess channel={0} uid={1}", channel, uid);
            VideoCanvas vs = new VideoCanvas((ulong)rtt_inst_.GetLocalWinId(), RENDER_MODE_TYPE.RENDER_MODE_FIT, channel);
            int ret = rtt_inst_.GetEngine().SetupLocalVideo(vs);
            Console.WriteLine("----->SetupLocalVideo ret={0}", ret);
        }

        public override void OnRejoinChannelSuccess(string channel, uint uid, int elapsed)
        {
            Console.WriteLine("----->OnRejoinChannelSuccess");
        }

        public override void OnLeaveChannel(RtcStats stats)
        {
            Console.WriteLine("----->OnLeaveChannel duration={0}", stats.duration);
        }

        public override void OnUserJoined(uint uid, int elapsed)
        {
            Console.WriteLine("----->OnUserJoined uid={0}", uid);
            if (rtt_inst_.GetRemoteWinId() == IntPtr.Zero) return;
            var vc = new VideoCanvas((ulong)rtt_inst_.GetRemoteWinId(), RENDER_MODE_TYPE.RENDER_MODE_FIT, rtt_inst_.GetChannelId(), uid);
            int ret = rtt_inst_.GetEngine().SetupRemoteVideo(vc);
            Console.WriteLine("----->SetupRemoteVideo, ret={0}", ret);
        }

        public override void OnUserOffline(uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            Console.WriteLine("----->OnUserOffline reason={0}", reason);
        }

        public override void OnStreamMessage(uint uid, int streamId, byte[] data, uint length)
        {
            string str = System.Text.Encoding.Default.GetString(data);

            CSharpForm.dump_handler_("OnStreamMessage,recv message: " + str , 0);

            Console.WriteLine("----->OnStreamMessage uid={0},streamId={1},data={2},length={3}"
              , uid, streamId, str, length);
        }

        public override void OnStreamMessageError(uint uid, int streamId, int code, int missed, int cached)
        {
            Console.WriteLine("----->OnStreamMessageError uid={0},streamId={1},code={2},missed={3},cached={4}"
                , uid, streamId, code, missed, cached);
        }
    }
}
