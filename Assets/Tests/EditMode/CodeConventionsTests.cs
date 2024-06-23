using System.Collections.Generic;
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
            var classesNotInNamespaces = 
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                from @class in ClassesNotInNamespace(source.text)
                select (@class, path);
            
            classesNotInNamespaces.Should().BeEmpty();
        }

        private IEnumerable<string> CSharpAssetPaths() =>
            AssetDatabase
                .FindAssets("t:TextAsset", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Path.GetExtension(path) == ".cs");

        private IEnumerable<string> ClassesNotInNamespace(string source) =>
            CSharpSyntaxTree
                .ParseText(source)
                .GetRoot()
                .DescendantNodesAndSelf()
                .OfType<ClassDeclarationSyntax>()
                .Where(NotInNamespace)
                .Select(@class => @class.Identifier.Text);

        private bool NotInNamespace(ClassDeclarationSyntax node) => !InNamespace(node);

        private bool InNamespace(ClassDeclarationSyntax node) =>
            node
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .Any();
    }
}