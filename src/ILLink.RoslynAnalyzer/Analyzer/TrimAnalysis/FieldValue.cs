// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ILLink.RoslynAnalyzer;
using ILLink.Shared.DataFlow;
using Microsoft.CodeAnalysis;

namespace ILLink.Shared.TrimAnalysis
{
    internal partial record FieldValue
    {
        public FieldValue(IFieldSymbol fieldSymbol)
            : this(fieldSymbol, fieldSymbol.Type, FlowAnnotations.GetFieldAnnotation(fieldSymbol))
        {
        }

        public FieldValue(IPropertySymbol propertySymbol)
            : this(propertySymbol, propertySymbol.Type, FlowAnnotations.GetBackingFieldAnnotation(propertySymbol))
        {
        }

        private FieldValue(ISymbol fieldSymbol, ITypeSymbol fieldType, DynamicallyAccessedMemberTypes annotations)
        {
            FieldSymbol = fieldSymbol;
            StaticType = new(fieldType);
            DynamicallyAccessedMemberTypes = annotations;
            _hashCode = EqualityComparer<ISymbol>.Default.GetHashCode(fieldSymbol);
        }

        public readonly ISymbol FieldSymbol;

        // Values are hashed on every set operation of the dataflow analysis, and symbol hashing is
        // expensive. Equal values have equal symbols, so the symbol's hash is a valid hash for the value.
        private readonly int _hashCode;

        public override int GetHashCode() => _hashCode;

        public override DynamicallyAccessedMemberTypes DynamicallyAccessedMemberTypes { get; }

        public override IEnumerable<string> GetDiagnosticArgumentsForAnnotationMismatch()
            => new string[] { FieldSymbol.GetDisplayName() };

        public override SingleValue DeepCopy() => this; // This value is immutable

        public override string ToString() => this.ValueToString(FieldSymbol, DynamicallyAccessedMemberTypes);
    }
}
