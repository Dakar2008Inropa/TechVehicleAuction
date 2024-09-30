using Avalonia;
using Avalonia.Controls;
using System;

namespace TechAuction.Utilities
{
    public static class FocusBehavior
    {
        public static readonly AttachedProperty<bool> IsFocusedProperty =
            AvaloniaProperty.RegisterAttached<Control, bool>(
                "IsFocused", typeof(FocusBehavior), defaultValue: false);

        public static bool GetIsFocused(Control element) =>
            element.GetValue(IsFocusedProperty);

        public static void SetIsFocused(Control element, bool value) =>
            element.SetValue(IsFocusedProperty, value);
        static FocusBehavior()
        {
            IsFocusedProperty.Changed.Subscribe(OnIsFocusedChanged);
        }

        private static void OnIsFocusedChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is Control control)
            {
                if (e.NewValue.Value)
                {
                    control.GotFocus += OnGotFocus;
                    control.AttachedToVisualTree += (_, __) => control.Focus();
                }
                else
                {
                    control.GotFocus -= OnGotFocus;
                }
            }
        }

        private static void OnGotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e)
        {
            if (sender is Control control)
            {
                SetIsFocused(control, true);
            }
        }
    }
}