using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class CodeConventionsTests
    {
        [Test]
        public void AllClassesShouldBeInNamespaces()
        {
            const string sourceCode = "namespace N {class C {}}";

            var classesNotInNamespaces =
                CSharpSyntaxTree
                    .ParseText(sourceCode)
                    .GetRoot()
                    .DescendantNodesAndSelf()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(NotInNamespace);

            classesNotInNamespaces.Should().BeEmpty();
        }

        private bool NotInNamespace(ClassDeclarationSyntax node) => !InNamespace(node);

        private bool InNamespace(ClassDeclarationSyntax node) =>
            node
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .Any();
    }
}