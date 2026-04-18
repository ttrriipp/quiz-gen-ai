using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizGenAI.Helpers
{
    public static class NotificationHelper
    {
        public static void ShowSuccess(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(15, 118, 110), Color.FromArgb(236, 253, 245));
        }

        public static void ShowInfo(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(3, 105, 161), Color.FromArgb(239, 246, 255));
        }

        public static void ShowWarning(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(180, 83, 9), Color.FromArgb(255, 251, 235));
        }

        public static void ShowError(IWin32Window owner, string title, string message)
        {
            Show(owner, title, message, Color.FromArgb(185, 28, 28), Color.FromArgb(254, 242, 242));
        }

        private static void Show(IWin32Window owner, string title, string message, Color accentColor, Color backColor)
        {
            var toast = new ToastNotificationForm(title, message, accentColor, backColor);
            var ownerControl = owner as Control;

            if (ownerControl != null && ownerControl.IsHandleCreated && !ownerControl.IsDisposed)
            {
                ownerControl.BeginInvoke((Action)delegate
                {
                    if (ownerControl.FindForm() != null)
                    {
                        toast.Show(ownerControl.FindForm());
                        return;
                    }

                    toast.Show();
                });
                return;
            }

            toast.Show();
        }

        private sealed class ToastNotificationForm : Form
        {
            private readonly Timer _closeTimer;

            public ToastNotificationForm(string title, string message, Color accentColor, Color backColor)
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                TopMost = true;
                StartPosition = FormStartPosition.Manual;
                Width = 340;
                Height = 108;
                BackColor = backColor;

                var accent = new Panel
                {
                    Dock = DockStyle.Left,
                    Width = 6,
                    BackColor = accentColor
                };

                var body = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(14, 12, 14, 12)
                };

                body.Controls.Add(new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(51, 65, 85),
                    Text = message
                });

                body.Controls.Add(new Label
                {
                    Dock = DockStyle.Top,
                    Height = 26,
                    Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(15, 23, 42),
                    Text = title
                });

                Controls.Add(body);
                Controls.Add(accent);

                _closeTimer = new Timer { Interval = 2800 };
                _closeTimer.Tick += delegate
                {
                    _closeTimer.Stop();
                    Close();
                };

                Shown += ToastNotificationForm_Shown;
            }

            protected override bool ShowWithoutActivation
            {
                get { return true; }
            }

            private void ToastNotificationForm_Shown(object sender, EventArgs e)
            {
                var workingArea = Owner != null
                    ? Screen.FromControl(Owner).WorkingArea
                    : Screen.PrimaryScreen.WorkingArea;

                Left = workingArea.Right - Width - 22;
                Top = workingArea.Top + 22;
                _closeTimer.Start();
            }
        }
    }
}
