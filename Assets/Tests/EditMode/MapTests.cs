﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace Tests.EditMode
{
    public class MapTests
    {
        private const char WallCode = '1';
        private static IEnumerable<string> MapPaths =>
            AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Map Data" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Path.GetExtension(path) == ".csv");

        [Test]
        public void AtLeastOneMapExist()
        {
            var maps = MapPaths;
            maps.Should().NotBeEmpty();
        }

        [TestCaseSource(nameof(MapPaths))]
        public void AllEdgeTilesAreWalls(string mapPath)
        {
            var map = AssetDatabase.LoadAssetAtPath<TextAsset>(mapPath).text;
            
            var edgeTiles = ParseMap(map)
                .Where(tile => tile.onEdge && tile.code != WallCode)
                .Select(tile => new { XY = (tile.x, tile.y), Code = tile.code });
            
            edgeTiles
                .Should()
                .BeEmpty();
        }
        
        [Test]
        public void EdgeTilesShouldBeWalls()
        {
            var map = @"
                        1,1,1,1
                        1,0,0,1
                        1,0,X,1
                        1,1,1,1
                       ";

            var edgeTiles = ParseMap(map)
                .Where(tile => tile.onEdge && tile.code != WallCode)
                .Select(tile => new { XY = (tile.x, tile.y), Code = tile.code });
            edgeTiles
                .Should()
                .BeEmpty();
        }

        private IEnumerable<(int x, int y, char code, bool onEdge)> ParseMap(string map)
        {
            var lines = map
                .Trim()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            return lines.SelectMany((line, lineIndex) =>
                ParseLine(
                    line,
                    lineIndex,
                    lineIndex == 0 || lineIndex == lines.Length - 1));
        }

        private IEnumerable<(int x, int y, char code, bool onEdge)> ParseLine(string line, int lineIndex, bool lineOnEdge)
        { 
            var tiles = line.Trim().Split(',');
            return tiles.Select((tile, tileIndex) => (
                lineIndex,
                tileIndex,
                tile.Single(),
                lineOnEdge || tileIndex == 0 || tileIndex == tiles.Length - 1));
        }
    }
}