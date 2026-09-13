namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>The CSharpFunctionalExtensions result shape accepted by a generated method.</summary>
internal enum ResultReceiverKind
{
  Result,
  ResultOfT,
  ResultOfByteArray,
  ResultOfStream,
  ResultOfString,
  ResultOfAsyncEnumerable,
  UnitResult,
}
