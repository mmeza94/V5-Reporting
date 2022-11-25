﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultNotificationWindow.cs" company="Tenaris">
//   Copyright (c) Tenaris S.A. All rights reserved.
// </copyright>
// <summary>
//  DefaultNotificationWindow class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure.InteractionRequests.DefaultWindows
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for NotificationChildWindow
    /// </summary>
    public partial class DefaultNotificationWindow : Window
    {
        public static readonly DependencyProperty NotificationTemplateProperty =
           DependencyProperty.Register(
               "NotificationTemplate",
               typeof(DataTemplate),
               typeof(DefaultNotificationWindow),
               new PropertyMetadata(null));

        /// <summary>
        /// Creates a new instance of <see cref="DefaultNotificationWindow"/>
        /// </summary>
        public DefaultNotificationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The <see cref="DataTemplate"/> to apply when displaying <see cref="Notification"/> data.
        /// </summary>
        public DataTemplate NotificationTemplate
        {
            get { return (DataTemplate)GetValue(NotificationTemplateProperty); }
            set { SetValue(NotificationTemplateProperty, value); }
        }
    }
}