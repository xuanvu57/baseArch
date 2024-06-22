using System.Diagnostics.CodeAnalysis;

namespace BaseArch.Domain.Entities.Interfaces
{
    public interface ISoftDeletable
    {
        /// <summary>
        /// Identify if the record is (soft) deleted
        /// </summary>
        [NotNull]
        bool IsDeleted { get; set; }
    }
}
