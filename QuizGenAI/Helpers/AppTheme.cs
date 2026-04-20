using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace QuizGenAI.Helpers
{
    public static class AppTheme
    {
        /// <summary>
        /// Set <see cref="Control.Tag"/> on a <see cref="Form"/> to this value so hosted light UIs
        /// (for example LiveCharts on Reports) are not recolored by <see cref="ApplyCognitaTheme"/>.
        /// </summary>
        public const string SkipCognitaThemeTag = "skip-cognita-theme";

        private static readonly ConditionalWeakTable<Control, Guna2Elipse> Elipses = new ConditionalWeakTable<Control, Guna2Elipse>();
        private static readonly Color BaseBackground = Color.FromArgb(5, 39, 29);
        private static readonly Color SurfaceBackground = Color.FromArgb(16, 58, 44);
        private static readonly Color SurfaceBorder = Color.FromArgb(44, 105, 82);
        private static readonly Color InputBackground = Color.FromArgb(10, 34, 26);
        private static readonly Color PrimaryText = Color.FromArgb(235, 243, 239);
        private static readonly Color SecondaryText = Color.FromArgb(184, 201, 193);
        private static readonly Color Accent = Color.FromArgb(212, 175, 55);

        public static void ApplyCognitaTheme(Form form)
        {
            if (form == null)
            {
                return;
            }

            form.BackColor = BaseBackground;
            form.ForeColor = PrimaryText;
            form.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);

            if (!(form.Tag is string) || !string.Equals((string)form.Tag, "theme-gradient", StringComparison.Ordinal))
            {
                form.Paint += Form_Paint;
                form.Resize += delegate { form.Invalidate(); };
                form.Tag = "theme-gradient";
            }

            ApplyToControlTree(form);
            HookControlAdded(form);
        }

        private static void HookControlAdded(Control parent)
        {
            parent.ControlAdded -= Parent_ControlAdded;
            parent.ControlAdded += Parent_ControlAdded;

            foreach (Control child in parent.Controls)
            {
                HookControlAdded(child);
            }
        }

        private static void Parent_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control == null)
            {
                return;
            }

            ApplyToControlTree(e.Control);
            HookControlAdded(e.Control);
        }

        private static void ApplyToControlTree(Control root)
        {
            if (ShouldSkipTheming(root))
            {
                return;
            }

            ApplyToControl(root);
            foreach (Control child in root.Controls)
            {
                ApplyToControlTree(child);
            }
        }

        private static bool ShouldSkipTheming(Control control)
        {
            var current = control;
            while (current != null)
            {
                if (string.Equals(current.Tag as string, SkipCognitaThemeTag, StringComparison.Ordinal))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void ApplyToControl(Control control)
        {
            var button = control as Button;
            if (button != null)
            {
                StyleButton(button);
                AttachElipse(button, 10);
                return;
            }

            var label = control as Label;
            if (label != null)
            {
                label.BackColor = Color.Transparent;
                label.ForeColor = label.Font.Bold ? PrimaryText : SecondaryText;
                return;
            }

            var linkLabel = control as LinkLabel;
            if (linkLabel != null)
            {
                linkLabel.LinkColor = PrimaryText;
                linkLabel.ActiveLinkColor = Accent;
                linkLabel.VisitedLinkColor = PrimaryText;
                linkLabel.BackColor = Color.Transparent;
                return;
            }

            var textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = InputBackground;
                textBox.ForeColor = PrimaryText;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                AttachElipse(textBox, 6);
                return;
            }

            var comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = InputBackground;
                comboBox.ForeColor = PrimaryText;
                comboBox.FlatStyle = FlatStyle.Flat;
                AttachElipse(comboBox, 6);
                return;
            }

            var numeric = control as NumericUpDown;
            if (numeric != null)
            {
                numeric.BackColor = InputBackground;
                numeric.ForeColor = PrimaryText;
                AttachElipse(numeric, 6);
                return;
            }

            var listBox = control as ListBox;
            if (listBox != null)
            {
                listBox.BackColor = InputBackground;
                listBox.ForeColor = PrimaryText;
                listBox.BorderStyle = BorderStyle.FixedSingle;
                AttachElipse(listBox, 6);
                return;
            }

            var panel = control as Panel;
            if (panel != null)
            {
                panel.BackColor = IsLight(panel.BackColor) ? SurfaceBackground : panel.BackColor;
                panel.ForeColor = PrimaryText;
                AttachElipse(panel, 12);
                return;
            }

            if (control is TableLayoutPanel || control is FlowLayoutPanel)
            {
                control.BackColor = Color.Transparent;
                control.ForeColor = PrimaryText;
                return;
            }

            if (control is Form)
            {
                return;
            }

            if (control.BackColor == SystemColors.Control || IsLight(control.BackColor))
            {
                control.BackColor = SurfaceBackground;
            }

            control.ForeColor = PrimaryText;
        }

        private static void AttachElipse(Control control, int radius)
        {
            if (control == null || radius <= 0 || control is Form)
            {
                return;
            }

            Guna2Elipse elipse;
            if (!Elipses.TryGetValue(control, out elipse))
            {
                elipse = new Guna2Elipse();
                Elipses.Add(control, elipse);
            }

            elipse.BorderRadius = radius;
            elipse.TargetControl = control;
        }

        private static void StyleButton(Button button)
        {
            var useAccent = IsPrimaryAction(button.Text) || IsLight(button.BackColor);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = useAccent ? 0 : 1;
            button.FlatAppearance.BorderColor = SurfaceBorder;
            button.BackColor = useAccent ? Accent : SurfaceBackground;
            button.ForeColor = useAccent ? Color.FromArgb(28, 34, 30) : PrimaryText;
        }

        private static bool IsPrimaryAction(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var normalized = text.Trim().ToLowerInvariant();
            return normalized.Contains("login")
                || normalized.Contains("save")
                || normalized.Contains("generate")
                || normalized.Contains("publish")
                || normalized.Contains("start")
                || normalized.Contains("submit")
                || normalized.Contains("next")
                || normalized.Contains("view");
        }

        private static bool IsLight(Color color)
        {
            return color.GetBrightness() > 0.62F;
        }

        private static void Form_Paint(object sender, PaintEventArgs e)
        {
            var form = sender as Form;
            if (form == null)
            {
                return;
            }

            var rect = form.ClientRectangle;
            using (var brush = new SolidBrush(BaseBackground))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            var glowColor = Color.FromArgb(80, 32, 102, 78);
            var glowRect = new Rectangle((int)(rect.Width * 0.42F), -60, (int)(rect.Width * 0.64F), (int)(rect.Height * 0.56F));
            using (var glowBrush = new System.Drawing.Drawing2D.LinearGradientBrush(glowRect, glowColor, Color.Transparent, 45F))
            {
                e.Graphics.FillEllipse(glowBrush, glowRect);
            }
        }
    }
}
