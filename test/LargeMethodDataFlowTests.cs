// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;
using ILLink.Shared;
using Xunit;
using VerifyCS = ILLink.RoslynAnalyzer.Tests.CSharpCodeFixVerifier<
    ILLink.RoslynAnalyzer.DynamicallyAccessedMembersAnalyzer,
    ILLink.CodeFix.DynamicallyAccessedMembersCodeFixProvider>;

namespace ILLink.RoslynAnalyzer.Tests
{
    public class LargeMethodDataFlowTests
    {
        // Each statement adds two blocks, so one sweep over the method takes more block visits
        // than a fixed limit of 10,000 allows.
        const int StatementCount = 6000;

        [Fact]
        public Task WarningAfterManyBlocksIsReported()
        {
            string statements = string.Join("\n", Enumerable.Range(0, StatementCount).Select(i => $"        if (args.Length > {i}) x++;"));
            string source = $$"""
            using System;
            using System.Diagnostics.CodeAnalysis;

            public static class C
            {
                static void Require([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type type) { }

                static Type Unannotated() => typeof(object);

                public static void Main(string[] args)
                {
                    int x = 0;
            {{statements}}
                    Require(Unannotated());
                }
            }
            """;
            int requireLine = 13 + StatementCount;

            return VerifyCS.VerifyAnalyzerAsync(
                source,
                consoleApplication: false,
                TestCaseUtils.UseMSBuildProperties(MSBuildPropertyOptionNames.EnableTrimAnalyzer),
                expected: VerifyCS.Diagnostic(DiagnosticId.DynamicallyAccessedMembersMismatchMethodReturnTypeTargetsParameter)
                    .WithSpan(requireLine, 9, requireLine, 31)
                    .WithSpan(8, 5, 8, 49)
                    .WithArguments("type", "C.Require(Type)", "C.Unannotated()", "'DynamicallyAccessedMemberTypes.PublicMethods'"));
        }
    }
}
