namespace BaseArch.Domain.AssemblyLayer
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class LayerInfoAttribute(CleanArchitectureLayerTypes layerType) : Attribute
    {
        public CleanArchitectureLayerTypes LayerType { get; } = layerType;
    }
}
