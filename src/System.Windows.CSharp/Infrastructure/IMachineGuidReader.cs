using SFX.ROP.CSharp;
using SFX.System.Windows.CSharp.Model.Machine;
using SFX.Utils.Infrastructure;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Interface describing the capability of reading the Windows installation product ids.
    /// </summary>
    public interface IMachineGuidReader : IInitializable
    {
        /// <summary>
        /// Reads the Windows product id from registry
        /// </summary>
        /// <returns></returns>
        Result<MachineGuid> GetMachineGuid();
    }
}
