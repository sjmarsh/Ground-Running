using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GroundRunning.GUI.Extensions
{
    /// <summary>
    /// Provides extensions to a PasswordBox that allow DataBinding 
    /// Sourced from http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html
    /// </summary>
    public class BindablePasswordBox
    {
        public static readonly DependencyProperty BoundPassword =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(BindablePasswordBox),
                                            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(BindablePasswordBox),
            new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(BindablePasswordBox),
                                                new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as PasswordBox;

            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            if (box != null) box.PasswordChanged -= HandlePasswordChanged;

            var newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                if (box != null) box.Password = newPassword;
            }

            if (box != null) box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {

            var box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            var wasBound = (bool)(e.OldValue);
            var needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;

            SetUpdatingPassword(box, true);
            if (box != null)
            {
                SetBoundPassword(box, box.Password);
                SetUpdatingPassword(box, false);
            }
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}
