﻿using System.Collections.Generic;
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