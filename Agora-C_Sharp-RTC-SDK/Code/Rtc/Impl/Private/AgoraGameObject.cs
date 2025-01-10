﻿#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_ANDROID || UNITY_VISIONOS || UNITY_OPENHARMONY
using UnityEngine;
using System.Collections;

namespace Agora.Rtc
{
    public class AgoraGameObject : MonoBehaviour
    {
        void OnApplicationQuit()
        {
#if UNITY_IOS || UNITY_ANDROID
            IRtcEngine rtcEngine = RtcEngine.Get();

            if (rtcEngine != null)
            {
                rtcEngine.Dispose();
            }
#endif
        }
    }
}

#endif