﻿// <copyright file="InputPortCallbackHandler.cs" company="Techyian">
// Copyright (c) Ian Auty. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;

namespace MMALSharp.Callbacks
{
    /// <summary>
    /// Represents an input port callback handler.
    /// </summary>
    /// <typeparam name="TPort">The input port type linked to this callback handler.</typeparam>
    /// <typeparam name="TCaptureHandler">The capture handler type linked to this callback handler.</typeparam>
    public abstract class InputPortCallbackHandler<TPort, TCaptureHandler> : IInputCallbackHandler
        where TPort : IInputPort
        where TCaptureHandler : IInputCaptureHandler
    {
        /// <summary>
        /// The encoding type to restrict on.
        /// </summary>
        public MMALEncoding EncodingType { get; }

        /// <summary>
        /// The working port.
        /// </summary>
        public TPort WorkingPort { get; }

        /// <summary>
        /// The active capture handler.
        /// </summary>
        public TCaptureHandler CaptureHandler { get; }

        /// <summary>
        /// Creates a new instance of <see cref="InputPortCallbackHandler{TPort,TCaptureHandler}"/>.
        /// </summary>
        /// <param name="port">The working port.</param>
        /// <param name="handler">The input port capture handler.</param>
        protected InputPortCallbackHandler(TPort port, TCaptureHandler handler)
        {
            this.WorkingPort = port;
            this.CaptureHandler = handler;
        }

        /// <summary>
        /// Creates a new instance of <see cref="InputPortCallbackHandler{TPort,TCaptureHandler}"/>.
        /// </summary>
        /// <param name="port">The working port.</param>
        /// <param name="handler">The input port capture handler.</param>
        /// <param name="encodingType">The <see cref="MMALEncoding"/> type to restrict on.</param>
        protected InputPortCallbackHandler(TPort port, TCaptureHandler handler, MMALEncoding encodingType)
            : this(port, handler)
        {
            this.EncodingType = encodingType;
        }

        /// <summary>
        /// Responsible for feeding data into the input port.
        /// </summary>
        /// <param name="buffer">The working buffer.</param>
        /// <returns>A <see cref="ProcessResult"/> based on the result of the operation.</returns>
        public virtual ProcessResult CallbackWithResult(IBuffer buffer)
        {
            if (MMALCameraConfig.Debug)
            {
                MMALLog.Logger.LogDebug($"In managed {this.WorkingPort.PortType.GetPortType()} callback");
            }

            if (this.EncodingType != null && this.WorkingPort.EncodingType != this.EncodingType)
            {
                throw new ArgumentException("Port Encoding Type not supported for this handler.");
            }

            MMALLog.Logger.LogInformation($"Feeding: {Helpers.ConvertBytesToMegabytes(buffer.AllocSize)}. Total processed: {this.CaptureHandler?.TotalProcessed()}.");

            return this.CaptureHandler?.Process(buffer.AllocSize);
        }
    }
}
