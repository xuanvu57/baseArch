namespace BaseArch.Infrastructure.gRPC.Attributes
{
    /// <summary>
    /// The signal to identify the gRPC service for mapping gRPC service automatically
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GrpcServiceAttribute : Attribute
    {
    }
}
