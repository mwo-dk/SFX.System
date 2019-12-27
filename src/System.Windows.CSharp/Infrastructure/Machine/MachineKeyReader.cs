//using SFX.System.Infrastructure;
//using SFX.System.Model;
//using System;

namespace SFX.System.Windows.CSharp.Infrastructure.Machine
{
    /*
    /// <summary>
    /// Implements <see cref="IMachineKeyReader"/>
    /// </summary>
    public sealed class MachineKeyReader : IMachineKeyReader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="machineGuidReader">The <see cref="IMachineGuidReader"/> utilized</param>
        public MachineKeyReader(IMachineGuidReader machineGuidReader) =>
            MachineGuidReader = machineGuidReader ?? throw new ArgumentNullException(nameof(machineGuidReader));

        internal IMachineGuidReader MachineGuidReader { get; }

        public (bool Success, string Error, MachineKey Result) GetMachineKey()
        {
            try
            {
                var (success, error, machineGuid) = MachineGuidReader.GetMachineGuid();
                if (!success)
                    return (false, error, default);
                success = Guid.TryParse(machineGuid.Value, out Guid guid);
                if (!success)
                    return (false, $"Unable to parse machine guid: {machineGuid.Value}", default);
                return (true, default, new MachineKey { Value = guid.ToByteArray() });
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }
    }*/
}
