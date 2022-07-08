﻿using System;

namespace Agora.Rtc
{
    ///
    /// <summary>
    /// The callback for custom media resource files.
    /// </summary>
    ///
    public abstract class IMediaPlayerCustomDataProvider
    {
        ///
        /// <summary>
        /// Occurs when the SDK seeks the media resource data.
        /// When you call the OpenWithCustomSource method to open a custom media resource, the SDK triggers this callback to request to seek to a specified location in the media resource.
        /// </summary>
        ///
        /// <param name="offset"> An input parameter. The offset of the target position relative to the starting point, in bytes. The value can be positive or negative.</param>
        ///
        /// <param name="whence"> An input parameter. The starting point. You can set it as one of the following values:
        ///  0: The starting point is the head of the data, and the actual data offset after seeking is offset.
        ///  1: The starting point is the current position, and the actual data offset after seeking is the current position plus offset.
        ///  2: The starting point is the end of the data, and the actual data offset after seeking is the whole data length plus offset.
        ///  65536: Do not perform position seeking and return the file size. Agora recommends that you use this parameter value when playing pure audio files such as MP3 and WAV.</param>
        ///
        /// <param name="playerId"> The ID of the media player.</param>
        ///
        /// <returns>
        /// When whence is 65536, the media file size is returned.
        /// When whence is 0, 1, or 2, the actual data offset after the seeking is returned.
        /// -1: Seeking failed.
        /// </returns>
        ///
        public virtual Int64 OnSeek(Int64 offset, int whence, int playerId)
        {
            return 0;
        }

        ///
        /// <summary>
        /// Occurs when the SDK reads the media resource data.
        /// When you call the OpenWithCustomSource method to open a custom media resource, the SDK triggers this callback, requesting you to transfer the media resource data into the buffer.
        /// </summary>
        ///
        /// <param name="bufferPtr"> An input parameter. Data buffer (bytes). Write the bufferSize data reported by the SDK into this parameter.</param>
        ///
        /// <param name="bufferSize"> The length of the data buffer (bytes).</param>
        ///
        /// <param name="playerId"> The ID of the media player.</param>
        ///
        /// <returns>
        /// If the data is read successfully, pass in the length of the data (bytes) you actually read in the return value.
        /// If reading the data fails, pass in 0 in the return value.
        /// </returns>
        ///
        public virtual int OnReadData(IntPtr bufferPtr, int bufferSize, int playerId)
        {
            return 0;
        }
    }
}