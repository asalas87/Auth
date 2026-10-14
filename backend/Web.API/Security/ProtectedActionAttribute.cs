namespace Web.API.Security;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ProtectedActionAttribute : Attribute
{
    public string ActionName { get; }
    public ProtectedActionAttribute(string actionName) => ActionName = actionName;
}