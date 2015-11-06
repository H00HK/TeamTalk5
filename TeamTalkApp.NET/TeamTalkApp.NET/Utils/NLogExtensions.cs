using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;


namespace TeamTalkApp.NET.Utils
{
    public static class NLogExtensions
    {
        public static Exception LogAndThrow(this Logger logger, Exception exc)
        {
            logger.Error(exc.Message);
            return exc;
        }
    }
}
