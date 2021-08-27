using System;
using System.IO;
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

            writer.Write(stage.TileMap.Count);
            foreach (var tl in stage.TileMap) {
                writer.Write(tl.X);
                writer.Write(tl.Y);
                writer.Write(tl.Elevated);
                writer.Write(tl.type);
            }
            TowerDefense.BaseLogger.Write($"Saved stage \"${stage.Name}.stg\" to Stages folder.", Internals.Logger.LogType.Info);
        }

        public static Stage LoadStage(string fileName) {
            string root = Path.Combine(TowerDefense.ExePath, "Stages");
            string path = Path.Combine(root, $"{fileName}.stg");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            Stage returnStage = new(fileName);

            if (!File.Exists(path)) {
                TowerDefense.BaseLogger.Write($"Could not find stage file \"{fileName}.stg\" in the stages folder. Did you make a typo?", Internals.Logger.LogType.Error);
                return returnStage;
            }

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read))) {
                try {
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
                    TowerDefense.BaseLogger.Write($"An error occurred while loading the stage \"${fileName}.stg\": {e}", Internals.Logger.LogType.Error);
                }
            }
            TowerDefense.BaseLogger.Write($"Loaded stage \"{fileName}.stg\" into current.", Internals.Logger.LogType.Info);
            return returnStage;
        }
    }
}