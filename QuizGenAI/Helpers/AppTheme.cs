using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
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
        private const string TitleFontFamilyName = "Host Grotesk";
        private const string BodyFontFamilyName = "Noto Sans";
        private static readonly PrivateFontCollection BundledFonts = new PrivateFontCollection();
        private static FontFamily _bundledTitleFamily;
        private static FontFamily _bundledBodyFamily;
        private static bool _fontsInitialized;

        public static void ApplyCognitaTheme(Form form)
        {
            if (form == null)
            {
                return;
            }

            EnsureBundledFontsLoaded();
            form.BackColor = BaseBackground;
            form.ForeColor = PrimaryText;
            form.Font = CreatePreferredFont(BodyFontFamilyName, "Segoe UI", 10F, FontStyle.Regular);

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
                label.Font = IsTitleLabel(label)
                    ? CreatePreferredFont(TitleFontFamilyName, label.Font.FontFamily.Name, label.Font.Size, label.Font.Style)
                    : CreatePreferredFont(BodyFontFamilyName, label.Font.FontFamily.Name, label.Font.Size, label.Font.Style);
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
                textBox.Font = CreatePreferredFont(BodyFontFamilyName, textBox.Font.FontFamily.Name, textBox.Font.Size, textBox.Font.Style);
                AttachElipse(textBox, 6);
                return;
            }

            var comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = InputBackground;
                comboBox.ForeColor = PrimaryText;
                comboBox.FlatStyle = FlatStyle.Flat;
                comboBox.Font = CreatePreferredFont(BodyFontFamilyName, comboBox.Font.FontFamily.Name, comboBox.Font.Size, comboBox.Font.Style);
                AttachElipse(comboBox, 6);
                return;
            }

            var numeric = control as NumericUpDown;
            if (numeric != null)
            {
                numeric.BackColor = InputBackground;
                numeric.ForeColor = PrimaryText;
                numeric.Font = CreatePreferredFont(BodyFontFamilyName, numeric.Font.FontFamily.Name, numeric.Font.Size, numeric.Font.Style);
                AttachElipse(numeric, 6);
                return;
            }

            var listBox = control as ListBox;
            if (listBox != null)
            {
                listBox.BackColor = InputBackground;
                listBox.ForeColor = PrimaryText;
                listBox.BorderStyle = BorderStyle.FixedSingle;
                listBox.Font = CreatePreferredFont(BodyFontFamilyName, listBox.Font.FontFamily.Name, listBox.Font.Size, listBox.Font.Style);
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
            button.Font = CreatePreferredFont(BodyFontFamilyName, button.Font.FontFamily.Name, button.Font.Size, button.Font.Style);
        }

        private static bool IsTitleLabel(Label label)
        {
            return label.Font.Bold || label.Font.Size >= 14F;
        }

        private static Font CreatePreferredFont(string preferredFamily, string fallbackFamily, float size, FontStyle style)
        {
            var bundledFamily = string.Equals(preferredFamily, TitleFontFamilyName, StringComparison.OrdinalIgnoreCase)
                ? _bundledTitleFamily
                : _bundledBodyFamily;

            if (bundledFamily != null)
            {
                try
                {
                    return new Font(bundledFamily, size, style, GraphicsUnit.Point, 0);
                }
                catch
                {
                    // Fall through to system-installed fonts.
                }
            }

            try
            {
                return new Font(preferredFamily, size, style, GraphicsUnit.Point, 0);
            }
            catch
            {
                try
                {
                    return new Font(fallbackFamily, size, style, GraphicsUnit.Point, 0);
                }
                catch
                {
                    return new Font("Segoe UI", size, style, GraphicsUnit.Point, 0);
                }
            }
        }

        private static void EnsureBundledFontsLoaded()
        {
            if (_fontsInitialized)
            {
                return;
            }

            _fontsInitialized = true;
            TryAddBundledFont(Path.Combine(Application.StartupPath, "Assets", "Fonts", "HostGrotesk-wght.ttf"));
            TryAddBundledFont(Path.Combine(Application.StartupPath, "Assets", "Fonts", "NotoSans-wdth-wght.ttf"));

            foreach (var family in BundledFonts.Families)
            {
                if (_bundledTitleFamily == null && family.Name.IndexOf("Host Grotesk", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _bundledTitleFamily = family;
                }

                if (_bundledBodyFamily == null && family.Name.IndexOf("Noto Sans", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _bundledBodyFamily = family;
                }
            }
        }

        private static void TryAddBundledFont(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    BundledFonts.AddFontFile(filePath);
                }
            }
            catch
            {
                // Keep fallback fonts if local asset loading fails.
            }
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
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            using (var brush = new SolidBrush(BaseBackground))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            var glowColor = Color.FromArgb(80, 32, 102, 78);
            var glowRect = new Rectangle((int)(rect.Width * 0.42F), -60, (int)(rect.Width * 0.64F), (int)(rect.Height * 0.56F));
            if (glowRect.Width <= 0 || glowRect.Height <= 0)
            {
                return;
            }

            using (var glowBrush = new System.Drawing.Drawing2D.LinearGradientBrush(glowRect, glowColor, Color.Transparent, 45F))
            {
                e.Graphics.FillEllipse(glowBrush, glowRect);
            }
        }
    }
}
