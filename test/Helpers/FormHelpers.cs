using System.Linq;
using System.Windows.Forms;

namespace test.Helpers
{
    public static class FormHelpers
    {
        public static RadioButton GetCheckedRadioButton(this Control control)
        {
            return control.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
        }
    }
}
