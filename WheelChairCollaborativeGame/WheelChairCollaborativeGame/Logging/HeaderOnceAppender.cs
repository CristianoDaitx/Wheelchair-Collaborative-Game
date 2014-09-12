using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using log4net.Appender;
namespace WheelChairCollaborativeGame.Logging
{
    public class HeaderOnceAppender : RollingFileAppender
    {
        protected override void WriteHeader()
        {
            if (LockingModel.AcquireLock().Length == 0)
            {
                base.WriteHeader();
            }
        }
    }
}
