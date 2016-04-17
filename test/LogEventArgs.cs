using System;

namespace test
{
    public class LogEventArgs : EventArgs
    {
        public long I { get; set; }
        public string Captcha { get; set; }
        public string Answer { get; set; }
        public int Errors { get; set; }
        public int Success { get; set; }
        public long Time { get; set; }

        public LogEventArgs(long i, string captcha, string answer, int errors, int success)
        {
            I = i;
            Captcha = captcha;
            Answer = answer;
            Errors = errors;
            Success = success;
        }
    }
}
