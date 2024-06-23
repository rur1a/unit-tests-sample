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
        public void AllCodeShouldNotContainUnderscore()
        {
            var codeWithUnderscore =
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                from code in CodeWithoutUnderscore(source.text)
                select (code, path);
            codeWithUnderscore.Should().BeEmpty();
        }

        private IEnumerable<string> CodeWithoutUnderscore(string text)
        {
            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetRoot();
            
            foreach (var node in root.DescendantNodes())
            {
                switch (node)
                {
                    case ClassDeclarationSyntax classNode:
                        if (classNode.Identifier.Text.Contains("_"))
                            yield return $"Class: {classNode.Identifier.Text}";
                        break;
                    case MethodDeclarationSyntax methodNode:
                        if (methodNode.Identifier.Text.Contains("_"))
                            yield return $"Method: {methodNode.Identifier.Text}";
                        break;
                    case NamespaceDeclarationSyntax namespaceNode:
                        if (namespaceNode.Name.ToString().Contains("_"))
                            yield return $"Namespace: {namespaceNode.Name}";
                        break;
                    case FieldDeclarationSyntax fieldNode:
                        foreach (var variable in fieldNode.Declaration.Variables)
                        {
                            if (variable.Identifier.Text.Contains("_"))
                                yield return $"Field: {variable.Identifier.Text}";
                        }
                        break;
                    case PropertyDeclarationSyntax propertyNode:
                        if (propertyNode.Identifier.Text.Contains("_"))
                            yield return $"Property: {propertyNode.Identifier.Text}";
                        break;
                    case VariableDeclaratorSyntax variableNode:
                        if (variableNode.Identifier.Text.Contains("_"))
                            yield return $"Variable: {variableNode.Identifier.Text}";
                        break;
                    case ParameterSyntax parameterNode:
                        if (parameterNode.Identifier.Text.Contains("_"))
                            yield return $"Parameter: {parameterNode.Identifier.Text}";
                        break;
                    case InterfaceDeclarationSyntax interfaceNode:
                        if (interfaceNode.Identifier.Text.Contains("_"))
                            yield return $"Interface: {interfaceNode.Identifier.Text}";
                        break;
                    case StructDeclarationSyntax structNode:
                        if (structNode.Identifier.Text.Contains("_"))
                            yield return $"Struct: {structNode.Identifier.Text}";
                        break;
                    case EnumDeclarationSyntax enumNode:
                        if (enumNode.Identifier.Text.Contains("_"))
                            yield return $"Enum: {enumNode.Identifier.Text}";
                        break;
                    case EnumMemberDeclarationSyntax enumMemberNode:
                        if (enumMemberNode.Identifier.Text.Contains("_"))
                            yield return $"Enum Member: {enumMemberNode.Identifier.Text}";
                        break;
                        }
            }
        }

        [Test]
        public void AllClassesShouldUseOnlySpaces()
        { 
            var classesWithSpaces = 
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                let linesWithSpaces = GetLinesWithTabs(source.text)
                where linesWithSpaces.Any()
                select (path, linesWithSpaces);
    
            classesWithSpaces.Should().BeEmpty();
        }

        private IEnumerable<int> GetLinesWithTabs(string source)
        {
            var lines = source.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("\t"))
                    yield return i + 1;
            }
        }
        
        [Test]
        public void AllTypesShouldBeInNamespaces()
        {
            var classesNotInNamespaces = 
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                from @class in TypesNotInNamespace(source.text)
                select (@class, path);
            
            classesNotInNamespaces.Should().BeEmpty();
        }

        private IEnumerable<string> CSharpAssetPaths() =>
            AssetDatabase
                .FindAssets("t:TextAsset", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Path.GetExtension(path) == ".cs");

        private IEnumerable<string> TypesNotInNamespace(string source) =>
            CSharpSyntaxTree
                .ParseText(source)
                .GetRoot()
                .DescendantNodesAndSelf()
                .Where(type =>
                    type is ClassDeclarationSyntax 
                        or StructDeclarationSyntax
                        or EnumDeclarationSyntax
                        or InterfaceDeclarationSyntax
                    )
                .Select(type => type as BaseTypeDeclarationSyntax)
                .Where(NotInNamespace)
                .Select(@class => @class.Identifier.Text);

        private bool NotInNamespace(BaseTypeDeclarationSyntax node) => !InNamespace(node);

        private bool InNamespace(BaseTypeDeclarationSyntax node) =>
            node
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .Any();
    }
}