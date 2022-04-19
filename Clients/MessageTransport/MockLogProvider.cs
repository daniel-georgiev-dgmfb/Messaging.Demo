using Kernel.Logging;
using System;

namespace Message.InMemory.Messaging.Client.Logging
{
	class MockLogProvider : ILogProvider
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
		}

		public bool TryCommitException(object o, out Exception exception)
		{
			exception = null;
			return true;
		}

		public bool TryLogException(Exception ex, out Exception result)
		{
			result = null;
			return true;
		}
	}
}
