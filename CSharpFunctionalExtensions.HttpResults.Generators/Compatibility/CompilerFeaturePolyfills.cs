// Polyfills for language features that are not available on netstandard2.0:
// init-only setters (IsExternalInit) and required members.

namespace System.Runtime.CompilerServices
{
  /// <summary>Polyfill for init-only property setters.</summary>
  internal static class IsExternalInit { }

  /// <summary>Polyfill for the required members feature.</summary>
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  internal sealed class CompilerFeatureRequiredAttribute : Attribute
  {
    public CompilerFeatureRequiredAttribute(string featureName) => FeatureName = featureName;

    public string FeatureName { get; }
  }

  /// <summary>Polyfill for the required members feature.</summary>
  internal sealed class RequiredMemberAttribute : Attribute { }
}

namespace System.Diagnostics.CodeAnalysis
{
  /// <summary>Polyfill for constructors of types with required members.</summary>
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  internal sealed class SetsRequiredMembersAttribute : Attribute { }
}
