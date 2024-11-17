namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class AuthenticatedAttribute : Attribute {} // Just a custom attribute