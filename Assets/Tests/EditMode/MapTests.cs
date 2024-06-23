using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.WSA;

namespace Tests.EditMode
{
    public class MapTests
    {
        [Test]
        public void AtLeastOneMapExist()
        {
            var maps = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Map Data" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Path.GetExtension(path) == ".csv");

            maps.Should().NotBeEmpty();
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
            var wallCode = '1';

            var edgeTiles = ParseMap(map)
                .Where(tile => tile.onEdge && tile.code != wallCode)
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
                lineOnEdge && (tileIndex == 0 || tileIndex == tiles.Length - 1)));
        }
    }
}