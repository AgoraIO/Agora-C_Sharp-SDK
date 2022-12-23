using System;

namespace Agora.Rtm
{
    public abstract class IRtmEventHandler
    {
        public virtual void OnMessageEvent(MessageEvent @event) { }

        public virtual void OnPresenceEvent(PresenceEvent @event) { }

        public virtual void OnLockEvent(LockEvent @event) { }

        public virtual void OnStorageEvent(StorageEvent @event) { }

        public virtual void OnJoinResult(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnLeaveResult(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnJoinTopicResult(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnLeaveTopicResult(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnTopicSubscribed(UInt64 requestId, string channelName, string userId, string topic,
                                              UserList succeedUsers, UserList failedUsers, RTM_CHANNEL_ERROR_CODE errorCode)
        { }


        public virtual void OnConnectionStateChange(string channelName, RTM_CONNECTION_STATE state, RTM_CONNECTION_CHANGE_REASON reason) { }

        public virtual void OnTokenPrivilegeWillExpire(string channelName) { }

        public virtual void OnSubscribeResult(UInt64 requestId, string channelName, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnPublishResult(UInt64 requestId, RTM_CHANNEL_ERROR_CODE errorCode) { }

        public virtual void OnLoginResult(RTM_LOGIN_ERROR_CODE errorCode) { }

        public virtual void OnSetChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnUpdateChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnRemoveChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnGetChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, IMetadata data,
                                                          OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void OnSetUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnUpdateUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnRemoveUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnGetUserMetadataResult(UInt64 requestId, string userId, IMetadata data, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnSubscribeUserMetadataResult(string userId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnSetLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                string lockName, OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void OnRemoveLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                string lockName, OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void OnReleaseLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                string lockName, OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void OnAcquireLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                string lockName, OPERATION_ERROR_CODE errorCode, string errorDetails)
        { }

        public virtual void OnRevokeLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                string lockName, OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void OnGetLocksResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType,
                                                LockDetail[] lockDetailList, UInt64 count, OPERATION_ERROR_CODE errorCode)
        { }

        public virtual void WhoNowResult(UInt64 requestId, UserState[] userStateList, UInt64 count, OPERATION_ERROR_CODE errorCode) { }

        public virtual void WhereNowResult(UInt64 requestId, ChannelInfo[] channels, UInt64 count, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnPresenceSetStateResult(UInt64 requestId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnPresenceRemoveStateResult(UInt64 requestId, OPERATION_ERROR_CODE errorCode) { }

        public virtual void OnPresenceGetStateResult(UInt64 requestId, UserState state, OPERATION_ERROR_CODE errorCode) { }
    }
}