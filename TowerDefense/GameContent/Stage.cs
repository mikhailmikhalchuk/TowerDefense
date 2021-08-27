using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.GameContent
{
    public class Stage
    {
        public string Name { get; private set; } = string.Empty;

        public List<Tile> TileMap { get; private set; } = new();

        public static Stage currentLoadedStage;

        private static byte[] fileHeader = new byte[5] { 84, 68, 83, 84, 71 };

        private const int SavingSystemVersion = 1; //INCREMENT THIS AFTER EVERY BREAKING CHANGE TO THE FILE SAVING SYSTEM

        public Stage(string name) {
            Name = name;
        }

        public static void SaveStage(Stage stage) {
            string root = Path.Combine(TowerDefense.ExePath, "Stages");
            string path = Path.Combine(root, $"{stage.Name}.stg");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            stage.TileMap.Clear();

            foreach (var tile in Tile.Tiles) {
                if (tile != null) {
                    stage.TileMap.Add(tile);
                }
            }

            using var writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));

            //File format
            //1. File header (5 bytes) - spells out "TDSTG" in ASCII
            //2. Stage saving system version (int) - dictates the stage saving system's current version
            //3. Stage count (int) - defines the length of the stage's tile map
            //4. Tile X (int) - tile X position
            //5. Tile Y (int) - tile Y position
            //6. Tile elevation (bool) - whether or not tile is elevated
            //7. Tile type (int) - tile's type value

            writer.Write(fileHeader);
            writer.Write(SavingSystemVersion);
            writer.Write(stage.TileMap.Count);
            foreach (var tl in stage.TileMap) {
                writer.Write(tl.X);
                writer.Write(tl.Y);
                writer.Write(tl.Elevated);
                writer.Write(tl.type);
            }
            TowerDefense.BaseLogger.Write($"Saved stage file \"{stage.Name}.stg\" to stages folder.", Internals.Logger.LogType.Info);
        }

        public static Stage LoadStage(string fileName) {
            string root = Path.Combine(TowerDefense.ExePath, "Stages");
            string path = Path.Combine(root, $"{fileName}.stg");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            Stage returnStage = new(fileName);

            if (!File.Exists(path)) {
                TowerDefense.BaseLogger.Write($"Could not find stage file \"{fileName}.stg\" in the stages folder, with the path {path}. Did you make a typo?", Internals.Logger.LogType.Error);
                return returnStage;
            }

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read))) {
                try {
                    byte[] header = reader.ReadBytes(5);
                    if (!header.SequenceEqual(fileHeader)) {
                        TowerDefense.BaseLogger.Write($"File \"{fileName}.stg\" is not a valid stage file (header invalid)", Internals.Logger.LogType.Error);
                        return returnStage;
                    }
                    int stageVersion = reader.ReadInt32();
                    if (stageVersion != SavingSystemVersion) {
                        TowerDefense.BaseLogger.Write($"File \"{fileName}.stg\" was saved using an older version of the file saving system. Unable to load.", Internals.Logger.LogType.Error); //eventually implement conversion of old files to current version
                        return returnStage;
                    }
                    int tileAmount = reader.ReadInt32();
                    for (int i = 0; i < tileAmount; i++) {
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        bool raised = reader.ReadBoolean();
                        int tileType = reader.ReadInt32();
                        returnStage.TileMap.Add(new Tile(x, y, raised, tileType));
                    }
                }
                catch (Exception e) {
                    TowerDefense.BaseLogger.Write($"An error occurred while loading the stage file \"{fileName}.stg\": {e}", Internals.Logger.LogType.Error);
                }
            }
            TowerDefense.BaseLogger.Write($"Loaded stage file \"{fileName}.stg\" into current.", Internals.Logger.LogType.Info);
            return returnStage;
        }

        public static void SetStage(Stage stage) {
            currentLoadedStage = stage;
            foreach (var tl in stage.TileMap) {
                Tile.Tiles[tl.X, tl.Y] = tl;
            }
            TowerDefense.BaseLogger.Write($"Set stage \"{stage.Name}\" as current.", Internals.Logger.LogType.Info);
        }
    }
}