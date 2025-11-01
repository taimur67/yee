using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BurtsevAlexey;

namespace LoG
{
	// Token: 0x020006EA RID: 1770
	public static class ObjectUtils
	{
		// Token: 0x060021B7 RID: 8631 RVA: 0x00075AB7 File Offset: 0x00073CB7
		public static IEnumerable<T> Explore<T>(object originalObject)
		{
			return ObjectUtils.Explore<T>(originalObject, new HashSet<object>(new ReferenceEqualityComparer()));
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00075AC9 File Offset: 0x00073CC9
		private static bool IsPrimitive(Type type)
		{
			return type == typeof(string) || (type.IsValueType & type.IsPrimitive);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00075AEC File Offset: 0x00073CEC
		private static IEnumerable<T> Explore<T>(object originalObject, HashSet<object> visited)
		{
			if (originalObject == null)
			{
				yield break;
			}
			if (visited.Contains(originalObject))
			{
				yield break;
			}
			visited.Add(originalObject);
			Type typeToReflect = originalObject.GetType();
			if (typeof(T).IsAssignableFrom(typeToReflect))
			{
				yield return (T)((object)originalObject);
			}
			IEnumerable enumerable = originalObject as IEnumerable;
			if (enumerable != null)
			{
				foreach (T t in ObjectUtils.Explore<T>(enumerable, visited))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
			}
			else if (!ObjectUtils.IsPrimitive(typeToReflect))
			{
				foreach (T t2 in ObjectUtils.RecurseMembers<T>(originalObject, visited))
				{
					yield return t2;
				}
				IEnumerator<T> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00075B03 File Offset: 0x00073D03
		private static IEnumerable<T> Explore<T>(IEnumerable lhs, HashSet<object> visited)
		{
			return ObjectUtils.Explore<T>(lhs.GetEnumerator(), visited);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00075B11 File Offset: 0x00073D11
		private static IEnumerable<T> Explore<T>(IEnumerator lhs, HashSet<object> visited)
		{
			while (lhs.MoveNext())
			{
				object originalObject = lhs.Current;
				foreach (T t in ObjectUtils.Explore<T>(originalObject, visited))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00075B28 File Offset: 0x00073D28
		private static IEnumerable<T> RecurseMembers<T>(object lhs, HashSet<object> visited)
		{
			Type type = lhs.GetType();
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				foreach (T t in ObjectUtils.Explore<T>(fieldInfo.GetValue(lhs), visited))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
			}
			FieldInfo[] array = null;
			yield break;
			yield break;
		}
	}
}
