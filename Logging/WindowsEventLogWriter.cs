using System;
using Kernel.Logging;

namespace Logging
{
    internal class WindowsEventLogWriter : AbstractLogWriter
    {
        public override void WriteExeption(object o)
        {
            if (o is Exception)
                LoggerManager.WriteExceptionToEventLog((Exception)o);
        }
    }
}