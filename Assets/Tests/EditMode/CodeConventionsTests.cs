using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode
{
    public class CodeConventionsTests
    {
        [Test]
        public void AllClassesShouldBeInNamespaces()
        {
            var sources =
                AssetDatabase
                    .FindAssets("t:TextAsset", new[] { "Assets" })
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(path => Path.GetExtension(path) == ".cs")
                    .Select(AssetDatabase.LoadAssetAtPath<TextAsset>);


            foreach (var cs in sources)
                CSharpSyntaxTree
                        .ParseText(cs.text)
                        .GetRoot()
                        .DescendantNodesAndSelf()
                        .OfType<ClassDeclarationSyntax>()
                        .Where(NotInNamespace)
                        .Should().BeEmpty();
        }

        private bool NotInNamespace(ClassDeclarationSyntax node) => !InNamespace(node);

        private bool InNamespace(ClassDeclarationSyntax node) =>
            node
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .Any();
    }
}