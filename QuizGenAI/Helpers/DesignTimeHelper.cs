using System.ComponentModel;
using System.Windows.Forms;

namespace QuizGenAI.Helpers
{
    internal static class DesignTimeHelper
    {
        public static bool IsInDesignMode(Control control)
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime
                || (control != null && control.Site != null && control.Site.DesignMode);
        }
    }
}
