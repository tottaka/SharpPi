﻿// <copyright file="DefaultPortCallbackHandler.cs" company="Techyian">
// Copyright (c) Ian Auty. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;

namespace MMALSharp.Callbacks
{
    /// <summary>
    /// A default callback handler for ports.
    /// </summary>
    public class DefaultPortCallbackHandler : PortCallbackHandler<IPort, IOutputCaptureHandler>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DefaultPortCallbackHandler"/>.
        /// </summary>
        /// <param name="port">The working <see cref="IPort"/>.</param>
        /// <param name="handler">The port capture handler.</param>
        public DefaultPortCallbackHandler(IPort port, IOutputCaptureHandler handler)
            : base(port, handler)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="DefaultPortCallbackHandler"/>.
        /// </summary>
        /// <param name="port">The working <see cref="IPort"/>.</param>
        /// <param name="handler">The port capture handler.</param>
        /// <param name="encodingType">The <see cref="MMALEncoding"/> type to restrict on.</param>
        public DefaultPortCallbackHandler(IPort port, IOutputCaptureHandler handler, MMALEncoding encodingType)
            : base(port, handler, encodingType)
        {
        }
    }
}
