using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;
using Kernel.Configuration;
using Kernel.Configuration.Notification;
using Kernel.Extensions;
using Kernel.Logging;
using Kernel.Notification;
using Kernel.Reflection;

namespace SmtpNotification
{
    public class SmtpDestinationTargetProvider : SettingsProviderBase, ISmptServer
    {
        private static Exception _error;

        private static bool _isValid;

        private static string _userName;

        private static IList<ValidationResult> _validationResult;

        private static SecureString _password;

        private static string _path;

        private static int _port;

        private static bool _isSSLEnabled;

        private static bool _useDefaultNetworkCredentials;

        private readonly ILogProvider _logProvider;
        public NetworkCredential NetworkCredential
        {
            get
            {
                return _useDefaultNetworkCredentials ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(_userName, _password);
            }
        }

        public override bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        public override Exception Error
        {
            get { return _error; }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public bool SSL
        {
            get
            {
                return _isSSLEnabled;
            }
        }

        public override IList<ValidationResult> ValidationResult
        {
            get
            {
                return _validationResult;
            }
        }

        static SmtpDestinationTargetProvider()
        {
            InternalInit();
        }
        public SmtpDestinationTargetProvider(ILogProvider logProvider)
        {
            this._logProvider = logProvider;
        }

        private static void InternalInit()
        {
            try
            {
                if (_validationResult == null)
                    _validationResult = new List<ValidationResult>();

                var section = ConfigurationSectionBase.GetSection<SmptProviderSettingsSection>("smptProviderSettingsSection");

                var settings = section.SmptSettings;

                if (settings.Contains("Path"))
                {
                    var settingValue = settings["Path"].Value;

                    if (string.IsNullOrWhiteSpace(settingValue))
                    {
                        LoggerManager.WriteWarningToEventLog("Path cannot be empty");

                        _validationResult.Add(new ValidationResult("Path cannot be empty"));
                    }
                    else
                        _path = settingValue;
                }
                else
                    _validationResult.Add(new ValidationResult("Path does not exist in configuration section"));

                if (settings.Contains("Port"))
                {
                    var settingValue = settings["Port"].Value;

                    int portNumeric;

                    if (!ReflectionHelper.TryParseOrDefault<int>(settingValue, out portNumeric))
                    {
                        _validationResult.Add(new ValidationResult(string.Format("Port setting is not numerical. Value is {0}", settingValue)));

                        LoggerManager.WriteWarningToEventLog(string.Format("Port setting is not numerical. Value is {0}", settingValue));
                    }
                    else
                        _port = portNumeric;
                }

                if (settings.Contains("IsSSLEnabled"))
                {
                    var settingValue = settings["IsSSLEnabled"].Value;

                    if (!ReflectionHelper.TryParseOrDefault<bool>(settingValue, out _isSSLEnabled))
                    {
                        _validationResult.Add(new ValidationResult(string.Format("Port setting is not numerical. Value is {0}", settingValue)));

                        LoggerManager.WriteWarningToEventLog(string.Format("Port setting is not numerical. Value is {0}", settingValue));
                    }
                }

                if (settings.Contains("UserName"))
                {
                    var settingValue = settings["UserName"].Value;

                    if (string.IsNullOrWhiteSpace(settingValue))
                    {
                        LoggerManager.WriteWarningToEventLog("UserName cannot be empty");

                        _validationResult.Add(new ValidationResult("UserName cannot be empty"));
                    }
                    else
                        if (!EmailValidator.ValidateEmail(settingValue))
                    {
                        LoggerManager.WriteWarningToEventLog(string.Format("UserName is not a valid e-mail format. Value is {0}", settingValue));

                        _validationResult.Add(new ValidationResult(string.Format("UserName is not a valid e-mail format. Value is {0}", settingValue)));
                    }
                    else
                        _userName = settingValue;
                }
                else
                    _validationResult.Add(new ValidationResult("UserName does not exist in configuration section"));
#if DEBUG
                _password = StringExtensions.ToSecureString("Password1");
#else
                if (settings.Contains("Password"))
                {
                    var encryptedPassword = settings["Password"].Value;

                    var secureString = Kernel.Cryptography.DPAPIEncryption.DecryptString(encryptedPassword);

                    _password = secureString;
                }
                else
                    _validationResult.Add(new ValidationResult("Password does not exist in configuration section"));
#endif

            }
            catch (Exception ex)
            {
                _error = ex;

                LoggerManager.WriteExceptionToEventLog(ex);
            }
            finally
            {
                _isValid = _error == null && _validationResult.Count == 0;
            }
        }
    }
}