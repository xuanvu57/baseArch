using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BaseArch.Infrastructure.Serilog
{
    public class SensitiveDataDestructuringPolicy(IConfiguration configuration) : IDestructuringPolicy
    {
        private const string DEFAULT_MASK_VALUE = "**MASKED**";
        private const string SENSITIVE_KEYWORDS_SECTION = "Logging:SensitiveData:Keywords";
        private const string MASK_VALUE = "Logging:SensitiveData:MaskValue";

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
        {
            var sensitiveKeywords = configuration.GetSection(SENSITIVE_KEYWORDS_SECTION).Get<string[]>() ?? [];
            var maskValue = configuration.GetValue<string>(MASK_VALUE) ?? DEFAULT_MASK_VALUE;

            if (sensitiveKeywords.Length == 0)
            {
                result = new StructureValue([]);

                return false;
            }

            var props = value.GetType().GetTypeInfo().DeclaredProperties;
            var logEventProperties = new List<LogEventProperty>();

            foreach (var propertyInfo in props)
            {
                if (Array.Exists(sensitiveKeywords, x => x.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(maskValue)));
                }
                else
                {
                    logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyInfo.GetValue(value))));
                }
            }

            result = new StructureValue(logEventProperties);

            return true;
        }
    }
}
