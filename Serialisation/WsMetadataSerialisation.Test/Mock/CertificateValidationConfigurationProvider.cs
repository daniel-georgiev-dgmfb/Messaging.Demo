using System;
using Kernel.Security.Configuration;
using Kernel.Security.Validation;

namespace WsMetadataSerialisation.Test.Mock
{
    internal class CertificateValidationConfigurationProvider : ICertificateValidationConfigurationProvider
    {
        public CertificateValidationConfiguration GetConfiguration(string federationPartyId)
        {
            return new CertificateValidationConfiguration
            {
                X509CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom
            };
        }
        public BackchannelConfiguration GeBackchannelConfiguration(string federationPartyId)
        {
            throw new NotImplementedException();
        }
        public BackchannelConfiguration GeBackchannelConfiguration(Uri partyUri)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}