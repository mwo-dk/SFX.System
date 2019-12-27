using SFX.ROP.CSharp;
using SFX.System.Windows.CSharp.Model.Product;
using SFX.Utils.Infrastructure;

namespace SFX.System.Windows.CSharp.Infrastructure.Product
{
    /// <summary>
    /// Interface describing the capability of reading the Windows installation product ids.
    /// </summary>
    public interface IProductIdReader : IInitializable
    {
        /// <summary>
        /// Reads the Windows product id from registry
        /// </summary>
        /// <returns></returns>
        Result<ProductId> GetProductId();

        /// <summary>
        /// Reads the Windows digital product id from registry
        /// </summary>
        /// <returns></returns>
        Result<DigitalProductId> GetDigitalProductId();
    }
}
