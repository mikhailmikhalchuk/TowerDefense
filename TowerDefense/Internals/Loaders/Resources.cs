using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using TDGame.GameContent;

namespace TDGame.Internals.Loaders
{
    public static class Resources
    {
		private static Dictionary<string, object> ResourceCache { get; set; } = new();

		public static T GetResource<T>(this ContentManager manager, string name) where T : class {
			if (ResourceCache.TryGetValue(Path.Combine(manager.RootDirectory, name), out var val) && val is T content) {
				return content;
			}
			return LoadResource<T>(manager, name);
		}
		public static T LoadResource<T>(ContentManager manager, string name) where T : class {
			T loaded = manager.Load<T>(name);

			ResourceCache[name] = loaded;
			return loaded;
		}

		public static T GetResourceBJ<T>(string name) where T : class {
			return GetResource<T>(TowerDefense.Instance.Content, name);
		}
	}
}