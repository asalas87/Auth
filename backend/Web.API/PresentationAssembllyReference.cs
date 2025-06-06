using Application;
using System.Reflection;

namespace Web.API
{
    public static class PresentationAssembllyReference
    {
        internal static readonly Assembly Asembly = typeof(PresentationAssembllyReference).Assembly;
    }
}
