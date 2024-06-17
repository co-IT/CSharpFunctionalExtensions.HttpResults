namespace CSharpFunctionalExtensions.HttpResults.Generators;

internal interface IGenerateMethods
{
    string Generate(string mapperClassName, string resultErrorType, string httpResultType);
}