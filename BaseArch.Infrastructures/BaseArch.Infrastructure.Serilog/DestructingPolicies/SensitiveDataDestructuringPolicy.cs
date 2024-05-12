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
        private readonly SensitiveDataOptions options;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="configuration"><see cref="Action(SensitiveDataOptions)"/></param>
        public SensitiveDataDestructuringPolicy(Action<SensitiveDataOptions> configuration)
        {
            options = new();
            configuration(options);
        }

        /// <inheritdoc/>
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
        {
            if (options.Keywords.Length == 0)
            {
                result = new StructureValue([]);

                return false;
            }

            var props = value.GetType().GetTypeInfo().DeclaredProperties;
            var logEventProperties = new List<LogEventProperty>();

            foreach (var propertyInfo in props)
            {
                if (Array.Exists(options.Keywords, x => x.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(options.MaskValue)));
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
