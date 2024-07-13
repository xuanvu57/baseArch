using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BaseArch.Infrastructure.Serilog.DestructingPolicies
{
    /// <summary>
    /// Sensitive data destructing policy
    /// </summary>
    public class SensitiveDataDestructuringPolicy : IDestructuringPolicy
    {
        /// <summary>
        /// <see cref="SensitiveDataOptions"/>
        /// </summary>
        private readonly SensitiveDataOptions _options;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="configuration"><see cref="Action(SensitiveDataOptions)"/></param>
        public SensitiveDataDestructuringPolicy(Action<SensitiveDataOptions> configuration)
        {
            _options = new();
            configuration(_options);
        }

        /// <inheritdoc/>
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
        {
            if (_options.Keywords.Length == 0)
            {
                result = new StructureValue([]);

                return false;
            }

            var props = value.GetType().GetTypeInfo().DeclaredProperties;
            var logEventProperties = new List<LogEventProperty>();

            foreach (var propertyInfo in props)
            {
                if (Array.Exists(_options.Keywords, x => x.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(_options.MaskValue)));
                }
                else
                {
                    try
                    {
                        logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyInfo.GetValue(value))));
                    }
                    catch { continue; }
                }
            }

            result = new StructureValue(logEventProperties);

            return true;
        }
    }
}
