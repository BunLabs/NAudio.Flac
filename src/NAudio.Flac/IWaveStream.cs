﻿using System;
using NAudio.Wave;

namespace NAudio
{
    /// <summary>
    ///     Defines the base for all audio streams.
    /// </summary>
    public interface IWaveStream : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="IWaveStream"/> supports seeking.
        /// </summary>
        bool CanSeek { get; }

        /// <summary>
        ///     Gets the <see cref="WaveFormat" /> of the waveform-audio data.
        /// </summary>
       WaveFormat WaveFormat { get; }

        /// <summary>
        ///     Gets or sets the current position. The unit of this property depends on the implementation of this interface. Some
        ///     implementations may not support this property.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        ///     Gets the length of the waveform-audio data. The unit of this property depends on the implementation of this
        ///     interface. Some implementations may not support this property.
        /// </summary>
        long Length { get; }
    }
}