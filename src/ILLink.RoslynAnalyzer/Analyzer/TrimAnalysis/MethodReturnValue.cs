// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ILLink.RoslynAnalyzer;
using ILLink.Shared.DataFlow;
using ILLink.Shared.TypeSystemProxy;
using Microsoft.CodeAnalysis;

namespace ILLink.Shared.TrimAnalysis
{
    internal partial record MethodReturnValue
    {
        public MethodReturnValue(IMethodSymbol methodSymbol, bool isNewObj)
            : this(methodSymbol, isNewObj, FlowAnnotations.GetMethodReturnValueAnnotation(methodSymbol))
        {
        }

        public MethodReturnValue(IMethodSymbol methodSymbol, bool isNewObj, DynamicallyAccessedMemberTypes dynamicallyAccessedMemberTypes)
        {
            Debug.Assert(!isNewObj || methodSymbol.MethodKind == MethodKind.Constructor, "isNewObj can only be true for constructors");
            MethodSymbol = methodSymbol;
            Method = new MethodProxy(methodSymbol);
            DynamicallyAccessedMemberTypes = dynamicallyAccessedMemberTypes;
            StaticType = new(isNewObj ? methodSymbol.ContainingType : methodSymbol.ReturnType);
            _hashCode = EqualityComparer<IMethodSymbol>.Default.GetHashCode(methodSymbol);
        }

        public readonly IMethodSymbol MethodSymbol;

        // Values are hashed on every set operation of the dataflow analysis, and symbol hashing is
        // expensive. Equal values have equal symbols, so the symbol's hash is a valid hash for the value.
        private readonly int _hashCode;

        public override int GetHashCode() => _hashCode;

        public override DynamicallyAccessedMemberTypes DynamicallyAccessedMemberTypes { get; }

        public override IEnumerable<string> GetDiagnosticArgumentsForAnnotationMismatch()
            => new string[] { MethodSymbol.GetDisplayName() };

        public override SingleValue DeepCopy() => this; // This value is immutable

        public override string ToString() => this.ValueToString(MethodSymbol, DynamicallyAccessedMemberTypes);
    }
}
