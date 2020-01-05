using System.Threading.Tasks;

namespace RFReborn.Routines
{
    /// <summary>
    /// Represents an object with running operations that can be killed
    /// </summary>
    public interface IKillable
    {
        /// <summary>
        /// Kill running operations
        /// </summary>
        Task Kill();
    }
}
