using Dova.Common;
using Dova.Common.InterfaceFactory;
using Moq;
using BindingFlags = System.Reflection.BindingFlags;

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
    public TInterface Get<TInterface>(IntPtr currentRefPtr) where TInterface : class, IJavaObject
    {
        var type = typeof(TInterface);

        if (type.IsClass && !type.IsAbstract)
        {
            return GetNewObject<TInterface>(currentRefPtr);
        }

        return GetMock<TInterface>(currentRefPtr);
    }

    private static TInterface GetNewObject<TInterface>(IntPtr currentRefPtr) where TInterface : class, IJavaObject
    {
        var type = typeof(TInterface);
        var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, new[] { typeof(IntPtr) });
        var instance = constructor.Invoke(new object?[] { currentRefPtr });

        return (TInterface)instance;
    }

    private static TInterface GetMock<TInterface>(IntPtr currentRefPtr) where TInterface : class, IJavaObject
    {
        var mock = new Mock<TInterface>(MockBehavior.Loose) { CallBase = true };

        mock.Setup(x => x.CurrentRefPtr).Returns(currentRefPtr);

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