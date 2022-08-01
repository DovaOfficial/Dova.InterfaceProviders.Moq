using Dova.Common.InterfaceFactory;
using Moq;

namespace Dova.InterfaceProviders.Moq;

/// <summary>
/// Interface object provider which uses Moq as a base for creating interface-type objects.
/// It is a fully-working solution until Dova change how it handles interfaces.
/// 
/// In order to use this provider please setup it as so:
/// DovaInterfaceFactory.Provider = new MoqInterfaceObjectProvider();
///
/// Or you can just call static Setup method as:
/// MoqInterfaceObjectProvider.Setup();
/// </summary>
public class MoqInterfaceObjectProvider : IInterfaceObjectProvider
{
    public TInterface Get<TInterface>() where TInterface : class
    {
        var mock = new Mock<TInterface>(MockBehavior.Loose) {CallBase = true};
        var obj = mock.Object;
        return obj;
    }

    /// <summary>
    /// Setups current provider to be used by DovaInterfaceFactory.
    /// </summary>
    public static void Setup()
    {
        DovaInterfaceFactory.Provider = new MoqInterfaceObjectProvider();
    }
}