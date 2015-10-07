
//taken from: http://blog.ianlee.info/2011/10/netmf-extension-methods.html
namespace System.Runtime.CompilerServices
{
    // Required for NETMF to recognized extension methods
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}
