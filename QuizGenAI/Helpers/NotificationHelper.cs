using System;
using System.Drawing;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace QuizGenAI.Helpers
{
    public static class NotificationHelper
    {
        public static void ShowSuccess(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(15, 118, 110), Color.FromArgb(236, 253, 245), SystemIcons.Information.ToBitmap());
        }

        public static void ShowInfo(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(3, 105, 161), Color.FromArgb(239, 246, 255), SystemIcons.Information.ToBitmap());
        }

        public static void ShowWarning(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(180, 83, 9), Color.FromArgb(255, 251, 235), SystemIcons.Warning.ToBitmap());
        }

        public static void ShowError(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(185, 28, 28), Color.FromArgb(254, 242, 242), SystemIcons.Error.ToBitmap());
        }

        private static void Show(IWin32Window owner, string title, string message, Color accentColor, Color backColor, Image icon)
        {
            var ownerControl = owner as Control;

            if (ownerControl != null && ownerControl.IsHandleCreated && !ownerControl.IsDisposed)
            {
                ownerControl.BeginInvoke((Action)delegate
                {
                    ShowCore(title, message, accentColor, backColor, icon);
                });
                return;
            }

            ShowCore(title, message, accentColor, backColor, icon);
        }

        private static void ShowCore(string title, string message, Color accentColor, Color backColor, Image icon)
        {
            var popup = new PopupNotifier
            {
                TitleText = title,
                ContentText = message,
                BodyColor = backColor,
                TitleColor = Color.FromArgb(15, 23, 42),
                ContentColor = Color.FromArgb(51, 65, 85),
                BorderColor = accentColor,
                Image = icon,
                ImageSize = new Size(24, 24),
                Delay = 2800,
                AnimationDuration = 180,
                AnimationInterval = 8,
                ShowOptionsButton = false,
                Scroll = true,
                Size = new Size(360, 110)
            };

            popup.Popup();
        }
    }
}
