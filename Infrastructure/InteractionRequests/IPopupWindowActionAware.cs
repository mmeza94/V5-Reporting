// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPopupWindowActionAware.cs" company="Tenaris">
//   Copyright (c) Tenaris S.A. All rights reserved.
// </copyright>
// <summary>
//  IPopupWindowActionAware class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure.InteractionRequests
{
    using System.Windows;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// PopupWindowActionAware interface
    /// </summary>
    public interface IPopupWindowActionAware
    {
        /// <summary>
        /// HostWindow
        /// </summary>
        Window HostWindow { get; set; }

        /// <summary>
        /// HostNotification
        /// </summary>
        Notification HostNotification { get; set; }
    }
}