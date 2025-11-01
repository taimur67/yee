using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006E0 RID: 1760
	public static class CloneUtils
	{
		// Token: 0x0600213B RID: 8507 RVA: 0x000735E0 File Offset: 0x000717E0
		public static T ShallowClone<T>(this T item) where T : ICloneable
		{
			return (T)((object)item.Clone());
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000735F4 File Offset: 0x000717F4
		public static T DeepClone<T>(this T item) where T : IDeepClone<T>
		{
			if (item == null)
			{
				return default(T);
			}
			T result;
			item.DeepClone(out result);
			return result;
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x00073623 File Offset: 0x00071823
		public static ConfigRef DeepClone(this ConfigRef configRef)
		{
			if (configRef == null)
			{
				return null;
			}
			return configRef.Clone();
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x00073630 File Offset: 0x00071830
		public static ConfigRef<T> DeepClone<T>(this ConfigRef<T> configRef) where T : IdentifiableStaticData
		{
			if (configRef == null)
			{
				return null;
			}
			return (ConfigRef<T>)configRef.Clone();
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x00073644 File Offset: 0x00071844
		public static HashSet<ConfigRef> DeepClone(this HashSet<ConfigRef> configRefSet)
		{
			if (configRefSet == null)
			{
				return null;
			}
			HashSet<ConfigRef> hashSet = new HashSet<ConfigRef>(configRefSet.Count);
			foreach (ConfigRef configRef in configRefSet)
			{
				hashSet.Add(configRef.DeepClone());
			}
			return hashSet;
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000736AC File Offset: 0x000718AC
		public static List<ConfigRef<T>> DeepClone<T>(this List<ConfigRef<T>> configRefSet) where T : IdentifiableStaticData
		{
			if (configRefSet == null)
			{
				return null;
			}
			List<ConfigRef<T>> list = new List<ConfigRef<T>>(configRefSet.Count);
			foreach (ConfigRef<T> configRef in configRefSet)
			{
				list.Add(configRef.DeepClone<T>());
			}
			return list;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x00073714 File Offset: 0x00071914
		public static string DeepClone(this string originalString)
		{
			if (originalString == null)
			{
				return null;
			}
			return new string(originalString);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00073728 File Offset: 0x00071928
		public static List<string> DeepClone(this List<string> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			List<string> list = new List<string>(originalList.Count);
			for (int i = 0; i < originalList.Count; i++)
			{
				list.Add(originalList[i].DeepClone());
			}
			return list;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0007376A File Offset: 0x0007196A
		public static List<int> DeepClone(this List<int> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<int>(originalList);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00073777 File Offset: 0x00071977
		public static List<AITag> DeepClone(this List<AITag> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<AITag>(originalList);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x00073784 File Offset: 0x00071984
		public static List<Identifier> DeepClone(this List<Identifier> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<Identifier>(originalList);
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00073791 File Offset: 0x00071991
		public static List<SchemeId> DeepClone(this List<SchemeId> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<SchemeId>(originalList);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0007379E File Offset: 0x0007199E
		public static List<ManuscriptCategory> DeepClone(this List<ManuscriptCategory> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<ManuscriptCategory>(originalList);
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000737AB File Offset: 0x000719AB
		public static List<ArchfiendStat> DeepClone(this List<ArchfiendStat> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<ArchfiendStat>(originalList);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000737B8 File Offset: 0x000719B8
		public static List<GamePieceCategory> DeepClone(this List<GamePieceCategory> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<GamePieceCategory>(originalList);
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000737C5 File Offset: 0x000719C5
		public static List<HexCoord> DeepClone(this List<HexCoord> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<HexCoord>(originalList);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000737D2 File Offset: 0x000719D2
		public static List<ResourceTypes> DeepClone(this List<ResourceTypes> originalList)
		{
			if (originalList == null)
			{
				return null;
			}
			return new List<ResourceTypes>(originalList);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000737E0 File Offset: 0x000719E0
		public static List<T> DeepClone<T>(this List<T> originalList) where T : IDeepClone<T>
		{
			if (originalList == null)
			{
				return null;
			}
			List<T> list = new List<T>(originalList.Count);
			for (int i = 0; i < originalList.Count; i++)
			{
				list.Add(originalList[i].DeepClone<T>());
			}
			return list;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x00073824 File Offset: 0x00071A24
		public static Dictionary<TKey, TValue> DeepClone<TKey, TValue>(this Dictionary<TKey, TValue> originalDictionary) where TKey : Enum where TValue : IDeepClone<TValue>
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(originalDictionary.Count);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in originalDictionary)
			{
				TKey tkey;
				TValue tvalue;
				keyValuePair.Deconstruct(out tkey, out tvalue);
				TKey key = tkey;
				TValue item = tvalue;
				dictionary.Add(key, item.DeepClone<TValue>());
			}
			return dictionary;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0007389C File Offset: 0x00071A9C
		public static Dictionary<int, TValue> DeepClone<TValue>(this Dictionary<int, TValue> originalDictionary) where TValue : IDeepClone<TValue>
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<int, TValue> dictionary = new Dictionary<int, TValue>(originalDictionary.Count);
			foreach (KeyValuePair<int, TValue> keyValuePair in originalDictionary)
			{
				int num;
				TValue tvalue;
				keyValuePair.Deconstruct(out num, out tvalue);
				int key = num;
				TValue item = tvalue;
				dictionary.Add(key, item.DeepClone<TValue>());
			}
			return dictionary;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00073914 File Offset: 0x00071B14
		public static Dictionary<int, List<TValue>> DeepClone<TValue>(this Dictionary<int, List<TValue>> originalDictionary) where TValue : IDeepClone<TValue>
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<int, List<TValue>> dictionary = new Dictionary<int, List<TValue>>(originalDictionary.Count);
			foreach (KeyValuePair<int, List<TValue>> keyValuePair in originalDictionary)
			{
				int num;
				List<TValue> originalList;
				keyValuePair.Deconstruct(out num, out originalList);
				int key = num;
				List<TValue> value = originalList.DeepClone<TValue>();
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00073990 File Offset: 0x00071B90
		public static Dictionary<int, Dictionary<int, int>> DeepClone(this Dictionary<int, Dictionary<int, int>> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>(originalDictionary.Count);
			foreach (KeyValuePair<int, Dictionary<int, int>> keyValuePair in originalDictionary)
			{
				int num;
				Dictionary<int, int> dictionary2;
				keyValuePair.Deconstruct(out num, out dictionary2);
				int key = num;
				Dictionary<int, int> originalDictionary2 = dictionary2;
				dictionary.Add(key, originalDictionary2.DeepClone());
			}
			return dictionary;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00073A08 File Offset: 0x00071C08
		public static Dictionary<int, int> DeepClone(this Dictionary<int, int> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			return new Dictionary<int, int>(originalDictionary);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00073A15 File Offset: 0x00071C15
		public static Dictionary<int, float> DeepClone(this Dictionary<int, float> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			return new Dictionary<int, float>(originalDictionary);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00073A22 File Offset: 0x00071C22
		public static Dictionary<TKey, int> DeepClone<TKey>(this Dictionary<TKey, int> originalDictionary) where TKey : Enum
		{
			if (originalDictionary == null)
			{
				return null;
			}
			return new Dictionary<TKey, int>(originalDictionary);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00073A30 File Offset: 0x00071C30
		public static Dictionary<string, int> DeepClone(this Dictionary<string, int> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>(originalDictionary.Count);
			foreach (KeyValuePair<string, int> keyValuePair in originalDictionary)
			{
				string text;
				int num;
				keyValuePair.Deconstruct(out text, out num);
				string originalString = text;
				int value = num;
				dictionary.Add(originalString.DeepClone(), value);
			}
			return dictionary;
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00073AA8 File Offset: 0x00071CA8
		public static Dictionary<int, string> DeepClone(this Dictionary<int, string> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<int, string> dictionary = new Dictionary<int, string>(originalDictionary.Count);
			foreach (KeyValuePair<int, string> keyValuePair in originalDictionary)
			{
				int num;
				string text;
				keyValuePair.Deconstruct(out num, out text);
				int key = num;
				string originalString = text;
				dictionary.Add(key, originalString.DeepClone());
			}
			return dictionary;
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00073B20 File Offset: 0x00071D20
		public static SortedDictionary<Identifier, GameItem> DeepClone(this SortedDictionary<Identifier, GameItem> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			SortedDictionary<Identifier, GameItem> sortedDictionary = new SortedDictionary<Identifier, GameItem>();
			foreach (KeyValuePair<Identifier, GameItem> keyValuePair in originalDictionary)
			{
				Identifier identifier;
				GameItem gameItem;
				keyValuePair.Deconstruct(out identifier, out gameItem);
				Identifier key = identifier;
				GameItem item = gameItem;
				sortedDictionary.Add(key, item.DeepClone<GameItem>());
			}
			return sortedDictionary;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00073B94 File Offset: 0x00071D94
		public static Dictionary<string, bool> DeepClone(this Dictionary<string, bool> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(originalDictionary.Count);
			foreach (KeyValuePair<string, bool> keyValuePair in originalDictionary)
			{
				string text;
				bool flag;
				keyValuePair.Deconstruct(out text, out flag);
				string originalString = text;
				bool value = flag;
				dictionary.Add(originalString.DeepClone(), value);
			}
			return dictionary;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00073C0C File Offset: 0x00071E0C
		public static Dictionary<SchemeId, bool> DeepClone(this Dictionary<SchemeId, bool> originalDictionary)
		{
			if (originalDictionary == null)
			{
				return null;
			}
			return new Dictionary<SchemeId, bool>(originalDictionary);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00073C1C File Offset: 0x00071E1C
		public static TStruct[] DeepClone<TStruct>(this TStruct[] originalArray) where TStruct : struct
		{
			if (originalArray == null)
			{
				return null;
			}
			TStruct[] array = new TStruct[originalArray.Length];
			Array.Copy(originalArray, array, originalArray.Length);
			return array;
		}
	}
}
