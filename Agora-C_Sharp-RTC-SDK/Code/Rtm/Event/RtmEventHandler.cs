using System;

namespace Agora.Rtm
{
    public delegate void OnMessageEventHandler(MessageEvent @event);

    public delegate void OnPresenceEventHandler(PresenceEvent @event);

    public delegate void OnLockEventHandler(LockEvent @event);

    public delegate void OnStorageEventHandler(StorageEvent @event);

    public delegate void OnJoinResultHandler(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnLeaveResultHandler(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnJoinTopicResultHandler(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnLeaveTopicResultHandler(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnTopicSubscribedHandler(UInt64 requestId, string channelName, string userId, string topic, UserList succeedUsers, UserList failedUsers, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnConnectionStateChangeHandler(string channelName, RTM_CONNECTION_STATE state, RTM_CONNECTION_CHANGE_REASON reason);

    public delegate void OnTokenPrivilegeWillExpireHandler(string channelName);

    public delegate void OnSubscribeResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnPublishResultHandler(UInt64 requestId, RTM_CHANNEL_ERROR_CODE errorCode);

    public delegate void OnLoginResultHandler(RTM_LOGIN_ERROR_CODE errorCode);

    public delegate void OnSetChannelMetadataResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode);

    public delegate void OnUpdateChannelMetadataResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode);

    public delegate void OnRemoveChannelMetadataResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode);

    public delegate void OnGetChannelMetadataResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, RtmMetadata data, OPERATION_ERROR_CODE errorCode);

    public delegate void OnSetUserMetadataResultHandler(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnUpdateUserMetadataResultHandler(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnRemoveUserMetadataResultHandler(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnGetUserMetadataResultHandler(UInt64 requestId, string userId, RtmMetadata data, OPERATION_ERROR_CODE errorCode);

    public delegate void OnSubscribeUserMetadataResultHandler(string userId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnSetLockResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode);

    public delegate void OnRemoveLockResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode);

    public delegate void OnReleaseLockResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode);

    public delegate void OnAcquireLockResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode, string errorDetails);

    public delegate void OnRevokeLockResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode);

    public delegate void OnGetLocksResultHandler(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, LockDetail[] lockDetailList, UInt64 count, OPERATION_ERROR_CODE errorCode);

    public delegate void WhoNowResultHandler(UInt64 requestId, UserState[] userStateList, UInt64 count, OPERATION_ERROR_CODE errorCode);

    public delegate void WhereNowResultHandler(UInt64 requestId, ChannelInfo[] channels, UInt64 count, OPERATION_ERROR_CODE errorCode);

    public delegate void OnPresenceSetStateResultHandler(UInt64 requestId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnPresenceRemoveStateResultHandler(UInt64 requestId, OPERATION_ERROR_CODE errorCode);

    public delegate void OnPresenceGetStateResultHandler(UInt64 requestId, UserState state, OPERATION_ERROR_CODE errorCode);


    public class RtmEventHandler : IRtmEventHandler
    {

        public event OnMessageEventHandler EventOnMessageEvent;

        public event OnPresenceEventHandler EventOnPresenceEvent;

        public event OnLockEventHandler EventOnLockEvent;

        public event OnStorageEventHandler EventOnStorageEvent;

        public event OnJoinResultHandler EventOnJoinResult;

        public event OnLeaveResultHandler EventOnLeaveResult;

        public event OnJoinTopicResultHandler EventOnJoinTopicResult;

        public event OnLeaveTopicResultHandler EventOnLeaveTopicResult;

        public event OnTopicSubscribedHandler EventOnTopicSubscribed;

        public event OnConnectionStateChangeHandler EventOnConnectionStateChange;

        public event OnTokenPrivilegeWillExpireHandler EventOnTokenPrivilegeWillExpire;

        public event OnSubscribeResultHandler EventOnSubscribeResult;

        public event OnPublishResultHandler EventOnPublishResult;

        public event OnLoginResultHandler EventOnLoginResult;

        public event OnSetChannelMetadataResultHandler EventOnSetChannelMetadataResult;

        public event OnUpdateChannelMetadataResultHandler EventOnUpdateChannelMetadataResult;

        public event OnRemoveChannelMetadataResultHandler EventOnRemoveChannelMetadataResult;

        public event OnGetChannelMetadataResultHandler EventOnGetChannelMetadataResult;

        public event OnSetUserMetadataResultHandler EventOnSetUserMetadataResult;

        public event OnUpdateUserMetadataResultHandler EventOnUpdateUserMetadataResult;

        public event OnRemoveUserMetadataResultHandler EventOnRemoveUserMetadataResult;

        public event OnGetUserMetadataResultHandler EventOnGetUserMetadataResult;

        public event OnSubscribeUserMetadataResultHandler EventOnSubscribeUserMetadataResult;

        public event OnSetLockResultHandler EventOnSetLockResult;

        public event OnRemoveLockResultHandler EventOnRemoveLockResult;

        public event OnReleaseLockResultHandler EventOnReleaseLockResult;

        public event OnAcquireLockResultHandler EventOnAcquireLockResult;

        public event OnRevokeLockResultHandler EventOnRevokeLockResult;

        public event OnGetLocksResultHandler EventOnGetLocksResult;

        public event WhoNowResultHandler EventWhoNowResult;

        public event WhereNowResultHandler EventWhereNowResult;

        public event OnPresenceSetStateResultHandler EventOnPresenceSetStateResult;

        public event OnPresenceRemoveStateResultHandler EventOnPresenceRemoveStateResult;

        public event OnPresenceGetStateResultHandler EventOnPresenceGetStateResult;

        private static RtmEventHandler eventInstance = null;

        public static RtmEventHandler GetInstance()
        {
            return eventInstance ?? (eventInstance = new RtmEventHandler());
        }


        public override void OnMessageEvent(MessageEvent @event)
        {
            if (EventOnMessageEvent == null) return;
            EventOnMessageEvent.Invoke(@event);
        }

        public override void OnPresenceEvent(PresenceEvent @event)
        {
            if (EventOnPresenceEvent == null) return;
            EventOnPresenceEvent.Invoke(@event);
        }

        public override void OnLockEvent(LockEvent @event)
        {
            if (EventOnLockEvent == null) return;
            EventOnLockEvent.Invoke(@event);
        }

        public override void OnStorageEvent(StorageEvent @event)
        {
            if (EventOnStorageEvent == null) return;
            EventOnStorageEvent.Invoke(@event);
        }

        public override void OnJoinResult(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnJoinResult == null) return;
            EventOnJoinResult.Invoke(requestId, channelName, userId, errorCode);
        }

        public override void OnLeaveResult(UInt64 requestId, string channelName, string userId, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnLeaveResult == null) return;
            EventOnLeaveResult.Invoke(requestId, channelName, userId, errorCode);
        }

        public override void OnJoinTopicResult(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnJoinTopicResult == null) return;
            EventOnJoinTopicResult.Invoke(requestId, channelName, userId, topic, meta, errorCode);
        }

        public override void OnLeaveTopicResult(UInt64 requestId, string channelName, string userId, string topic, string meta, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnLeaveTopicResult == null) return;
            EventOnLeaveTopicResult.Invoke(requestId, channelName, userId, topic, meta, errorCode);
        }

        public override void OnTopicSubscribed(UInt64 requestId, string channelName, string userId, string topic, UserList succeedUsers, UserList failedUsers, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnTopicSubscribed == null) return;
            EventOnTopicSubscribed.Invoke(requestId, channelName, userId, topic, succeedUsers, failedUsers, errorCode);
        }

        public override void OnConnectionStateChange(string channelName, RTM_CONNECTION_STATE state, RTM_CONNECTION_CHANGE_REASON reason)
        {
            if (EventOnConnectionStateChange == null) return;
            EventOnConnectionStateChange.Invoke(channelName, state, reason);
        }

        public override void OnTokenPrivilegeWillExpire(string channelName)
        {
            if (EventOnTokenPrivilegeWillExpire == null) return;
            EventOnTokenPrivilegeWillExpire.Invoke(channelName);
        }

        public override void OnSubscribeResult(UInt64 requestId, string channelName, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnSubscribeResult == null) return;
            EventOnSubscribeResult.Invoke(requestId, channelName, errorCode);
        }

        public override void OnPublishResult(UInt64 requestId, RTM_CHANNEL_ERROR_CODE errorCode)
        {
            if (EventOnPublishResult == null) return;
            EventOnPublishResult.Invoke(requestId, errorCode);
        }

        public override void OnLoginResult(RTM_LOGIN_ERROR_CODE errorCode)
        {
            if (EventOnLoginResult == null) return;
            EventOnLoginResult.Invoke(errorCode);
        }

        public override void OnSetChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnSetChannelMetadataResult == null) return;
            EventOnSetChannelMetadataResult.Invoke(requestId, channelName, channelType, errorCode);
        }

        public override void OnUpdateChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnUpdateChannelMetadataResult == null) return;
            EventOnUpdateChannelMetadataResult.Invoke(requestId, channelName, channelType, errorCode);
        }

        public override void OnRemoveChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnRemoveChannelMetadataResult == null) return;
            EventOnRemoveChannelMetadataResult.Invoke(requestId, channelName, channelType, errorCode);
        }

        public override void OnGetChannelMetadataResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, RtmMetadata data, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnGetChannelMetadataResult == null) return;
            EventOnGetChannelMetadataResult.Invoke(requestId, channelName, channelType, data, errorCode);
        }

        public override void OnSetUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnSetUserMetadataResult == null) return;
            EventOnSetUserMetadataResult.Invoke(requestId, userId, errorCode);
        }

        public override void OnUpdateUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnUpdateUserMetadataResult == null) return;
            EventOnUpdateUserMetadataResult.Invoke(requestId, userId, errorCode);
        }

        public override void OnRemoveUserMetadataResult(UInt64 requestId, string userId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnRemoveUserMetadataResult == null) return;
            EventOnRemoveUserMetadataResult.Invoke(requestId, userId, errorCode);
        }

        public override void OnGetUserMetadataResult(UInt64 requestId, string userId, RtmMetadata data, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnGetUserMetadataResult == null) return;
            EventOnGetUserMetadataResult.Invoke(requestId, userId, data, errorCode);
        }

        public override void OnSubscribeUserMetadataResult(string userId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnSubscribeUserMetadataResult == null) return;
            EventOnSubscribeUserMetadataResult.Invoke(userId, errorCode);
        }

        public override void OnSetLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnSetLockResult == null) return;
            EventOnSetLockResult.Invoke(requestId, channelName, channelType, lockName, errorCode);
        }

        public override void OnRemoveLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnRemoveLockResult == null) return;
            EventOnRemoveLockResult.Invoke(requestId, channelName, channelType, lockName, errorCode);
        }

        public override void OnReleaseLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnReleaseLockResult == null) return;
            EventOnReleaseLockResult.Invoke(requestId, channelName, channelType, lockName, errorCode);
        }

        public override void OnAcquireLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode, string errorDetails)
        {
            if (EventOnAcquireLockResult == null) return;
            EventOnAcquireLockResult.Invoke(requestId, channelName, channelType, lockName, errorCode, errorDetails);
        }

        public override void OnRevokeLockResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, string lockName, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnRevokeLockResult == null) return;
            EventOnRevokeLockResult.Invoke(requestId, channelName, channelType, lockName, errorCode);
        }

        public override void OnGetLocksResult(UInt64 requestId, string channelName, RTM_CHANNEL_TYPE channelType, LockDetail[] lockDetailList, UInt64 count, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnGetLocksResult == null) return;
            EventOnGetLocksResult.Invoke(requestId, channelName, channelType, lockDetailList, count, errorCode);
        }

        public override void WhoNowResult(UInt64 requestId, UserState[] userStateList, UInt64 count, OPERATION_ERROR_CODE errorCode)
        {
            if (EventWhoNowResult == null) return;
            EventWhoNowResult.Invoke(requestId, userStateList, count, errorCode);
        }

        public override void WhereNowResult(UInt64 requestId, ChannelInfo[] channels, UInt64 count, OPERATION_ERROR_CODE errorCode)
        {
            if (EventWhereNowResult == null) return;
            EventWhereNowResult.Invoke(requestId, channels, count, errorCode);
        }

        public override void OnPresenceSetStateResult(UInt64 requestId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnPresenceSetStateResult == null) return;
            EventOnPresenceSetStateResult.Invoke(requestId, errorCode);
        }

        public override void OnPresenceRemoveStateResult(UInt64 requestId, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnPresenceRemoveStateResult == null) return;
            EventOnPresenceRemoveStateResult.Invoke(requestId, errorCode);
        }

        public override void OnPresenceGetStateResult(UInt64 requestId, UserState state, OPERATION_ERROR_CODE errorCode)
        {
            if (EventOnPresenceGetStateResult == null) return;
            EventOnPresenceGetStateResult.Invoke(requestId, state, errorCode);
        }

    }

}