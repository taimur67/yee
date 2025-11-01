using System;
using System.Collections.Generic;
using System.Linq;
using BurtsevAlexey;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006F2 RID: 1778
	public static class Util
	{
		// Token: 0x06002213 RID: 8723 RVA: 0x00076B63 File Offset: 0x00074D63
		public static bool IsSinglePlayerGameMode(this GameType type)
		{
			return type != GameType.None && !type.IsMultiplayerGameMode();
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00076B73 File Offset: 0x00074D73
		public static bool IsMultiplayerGameMode(this GameType gameType)
		{
			return gameType == GameType.AsyncCustom || gameType == GameType.AsyncMatchmaking || gameType == GameType.LiveCustom || gameType == GameType.LiveMatchmaking;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x00076B87 File Offset: 0x00074D87
		public static bool IsAsyncGameMode(this GameType gameType)
		{
			return gameType == GameType.AsyncCustom || gameType == GameType.AsyncMatchmaking;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x00076B93 File Offset: 0x00074D93
		public static MultiplayerType GetMultiplayerType(this GameType gameType)
		{
			switch (gameType)
			{
			case GameType.LiveMatchmaking:
			case GameType.AsyncMatchmaking:
				return MultiplayerType.Matchmaking;
			case GameType.LiveCustom:
			case GameType.AsyncCustom:
				return MultiplayerType.CustomLobby;
			}
			return MultiplayerType.None;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00076BB8 File Offset: 0x00074DB8
		public static MultiplayerMode GetMultiplayerMode(this GameType gameType)
		{
			switch (gameType)
			{
			case GameType.LiveMatchmaking:
			case GameType.LiveCustom:
				return MultiplayerMode.Live;
			case GameType.AsyncMatchmaking:
			case GameType.AsyncCustom:
				return MultiplayerMode.Async;
			}
			return MultiplayerMode.None;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x00076BDD File Offset: 0x00074DDD
		public static float NextFloat(this Random random, float min, float max)
		{
			return (float)(random.NextDouble() * (double)(max - min) + (double)min);
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00076BF0 File Offset: 0x00074DF0
		public static int StringToSeed(string seed)
		{
			seed = seed.Trim();
			int result;
			if (!int.TryParse(seed, out result))
			{
				return seed.GetHashCode();
			}
			return result;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00076C18 File Offset: 0x00074E18
		public static void AddOrSetValue<T>(this Dictionary<T, int> dict, T key, int value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] += value;
				return;
			}
			dict[key] = value;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00076C4C File Offset: 0x00074E4C
		public static T DeepClone<T>(this T obj, CloneFunction cloneFunction = CloneFunction.FastClone)
		{
			T t2;
			using (SimProfilerBlock.ProfilerBlock(string.Format("DeepClone<{0}>", typeof(T))))
			{
				T t;
				switch (cloneFunction)
				{
				case CloneFunction.SerializeClone:
					t = obj.SerializeClone<T>();
					break;
				case CloneFunction.ReflectClone:
					t = obj.ReflectClone<T>();
					break;
				case CloneFunction.FastClone:
					t = obj.FastClone<T>();
					break;
				default:
					t2 = default(T);
					t = t2;
					break;
				}
				t2 = t;
			}
			return t2;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00076CCC File Offset: 0x00074ECC
		public static T SerializeClone<T>(this T obj)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, settings), settings);
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00076CF8 File Offset: 0x00074EF8
		public static T ReflectClone<T>(this T obj)
		{
			return ObjectExtensions.Copy<T>(obj);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00076D00 File Offset: 0x00074F00
		public static T FastClone<T>(this T obj)
		{
			return LoG.FastClone.Copy<T>(obj);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00076D08 File Offset: 0x00074F08
		public static HexCoord GetPreviousHexOnPath(this HexCoord coord, List<HexCoord> path, GamePiece actor)
		{
			if (path.Contains(coord))
			{
				int num = path.IndexOf(coord);
				if (num > 0)
				{
					return path[num - 1];
				}
				return actor.Location;
			}
			else
			{
				if (path.Count > 0)
				{
					return path.Last<HexCoord>();
				}
				return actor.Location;
			}
		}
	}
}
