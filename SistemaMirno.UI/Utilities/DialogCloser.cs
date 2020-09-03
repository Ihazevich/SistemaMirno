// <copyright file="DialogCloser.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Windows;

namespace SistemaMirno.UI.Utilities
{
    /// <summary>
    /// A class representing an attached property to close windows using data binding.
    /// </summary>
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        /// <summary>
        /// Sets the Dialog Result of a specific window.
        /// </summary>
        /// <param name="target">The target window.</param>
        /// <param name="value">The value for the dialog result.</param>
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }
    }
}
