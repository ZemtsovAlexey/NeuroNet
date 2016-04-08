using System.Drawing;

namespace CaptchaGenerator
{
    public class CaptchaInfo
    {
        public string Captcha { get; }

        public Bitmap Image { get; }

        public CaptchaInfo(string captcha, Bitmap image)
        {
            Captcha = captcha;
            Image = image;
        }
    }
}
