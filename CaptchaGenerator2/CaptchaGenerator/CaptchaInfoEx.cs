using System.Collections.Generic;
using System.Drawing;

namespace CaptchaGenerator
{
    public class CaptchaInfoEx : CaptchaInfo
    {
        public List<Bitmap> Blocks { get; }

        public int TryCount { get;  }

        public CaptchaInfoEx(string captcha, Bitmap image, List<Bitmap> blocks, int tryCount) : base(captcha, image)
        {
            Blocks = blocks;
            TryCount = tryCount;
        }
    }
}
