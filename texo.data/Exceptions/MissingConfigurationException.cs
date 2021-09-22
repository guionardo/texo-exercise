using System;

namespace texo.data.Exceptions
{
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string configurationName) : base(
            $"Missing configuration \"{configurationName}\"")
        {
        }
    }
}