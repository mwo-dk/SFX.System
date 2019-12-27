using SFX.ROP.CSharp;
using SFX.System.Model;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Interface describing the capability to read a unique machine key
    /// </summary>
    public interface IMachineKeyReader
    {
        /// <summary>
        /// Reads a uniqu machine id
        /// </summary>
        /// <returns>The machine key</returns>
        Result<MachineKey> GetMachineKey();
    }
}
