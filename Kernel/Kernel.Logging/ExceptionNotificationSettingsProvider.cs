using Kernel.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kernel.Logging.SettingsProviders
{
    internal class ExceptionNotificationSettingsProvider : SettingsProviderBase
    {
        public static object EmailMessageClone { get; internal set; }
        public bool IsInitialised { get; internal set; }

        public override IList<ValidationResult> ValidationResult => throw new NotImplementedException();

        public override bool IsValid => throw new NotImplementedException();

        public override Exception Error => throw new NotImplementedException();

        internal void WaitToInitialise()
        {
            throw new NotImplementedException();
        }
    }
}
