namespace System.Reflection.Metadata
{
    internal static class SR
    {
        internal static string Arg_NotSimpleTypeName => Strings.Arg_NotSimpleTypeName;
        internal static string ArgumentOutOfRange_NeedNonNegNum => "Non-negative number required.";
        internal static string Argument_AssemblyGetTypeCannotSpecifyAssembly => "Type names passed to Assembly.GetType() must not specify an assembly.";
        internal static string Argument_HasToBeArrayClass => Strings.Argument_HasToBeArrayClass;
        internal static string Argument_InvalidTypeName => Strings.Argument_InvalidTypeName;
        internal static string Format_StringZeroLength => "String cannot have zero length.";
        internal static string InvalidAssemblyName => Strings.InvalidAssemblyName;
        internal static string InvalidOperation_MaxNodesExceeded => Strings.InvalidOperation_MaxNodesExceeded;
        internal static string InvalidOperation_NestedTypeNamespace => Strings.InvalidOperation_NestedTypeNamespace;
        internal static string InvalidOperation_NoElement => Strings.InvalidOperation_NoElement;
        internal static string InvalidOperation_NotGenericType => Strings.InvalidOperation_NotGenericType;
        internal static string InvalidOperation_NotNestedType => Strings.InvalidOperation_NotNestedType;
        internal static string Format(string format, object? arg0) => string.Format(format, arg0);
        internal static string Format(string format, object? arg0, object? arg1) => string.Format(format, arg0, arg1);
    }
}
