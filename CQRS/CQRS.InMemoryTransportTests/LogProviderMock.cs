﻿using System;
using System.Diagnostics;
using Kernel.Logging;

namespace CQRS.InMemoryTransportTests.MockData
{
    internal class LogProviderMock : ILogProvider
    {
        public ILogWriter LogWriter { set => throw new NotImplementedException(); }

        public string AddParameter(string name, object value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LogMessage(string m)
        {
            Debug.WriteLine(m);
        }

        public bool TryCommitException(object o, out Exception exception)
        {
            throw new NotImplementedException();
        }

        public bool TryLogException(Exception ex, out Exception result)
        {
            Debug.WriteLine(ex.Message);
            result = null;
            return true;
        }
    }
}