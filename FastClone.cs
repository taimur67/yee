using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BurtsevAlexey;
using BurtsevAlexey.ArrayExtensions;

namespace LoG
{
	// Token: 0x020006E1 RID: 1761
	public static class FastClone
	{
		// Token: 0x0600215A RID: 8538 RVA: 0x00073C42 File Offset: 0x00071E42
		public static T Copy<T>(T original)
		{
			return (T)((object)FastClone.InternalCopy(original, new Dictionary<object, object>(new ReferenceEqualityComparer())));
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00073C60 File Offset: 0x00071E60
		private static object ShallowClone(object originalObject)
		{
			ICloneable cloneable = originalObject as ICloneable;
			if (cloneable != null)
			{
				return cloneable.Clone();
			}
			return ObjectExtensions.CloneMethod.Invoke(originalObject, null);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00073C8C File Offset: 0x00071E8C
		private static object InternalCopy(object originalObject, [NotNull] Dictionary<object, object> visited)
		{
			object result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				if (originalObject == null)
				{
					result = null;
				}
				else
				{
					Type type = originalObject.GetType();
					object obj;
					if (type.IsPrimitive())
					{
						result = originalObject;
					}
					else if (visited.TryGetValue(originalObject, out obj))
					{
						result = obj;
					}
					else if (typeof(Delegate).IsAssignableFrom(type))
					{
						result = null;
					}
					else
					{
						object obj2 = FastClone.ShallowClone(originalObject);
						if (type.IsArray && !type.GetElementType().IsPrimitive())
						{
							Array array = (Array)obj2;
							if (array.LongLength > 0L)
							{
								ArrayTraverse arrayTraverse = new ArrayTraverse(array);
								do
								{
									array.SetValue(FastClone.InternalCopy(array.GetValue(arrayTraverse.Position), visited), arrayTraverse.Position);
								}
								while (arrayTraverse.Step());
							}
						}
						visited.Add(originalObject, obj2);
						FastClone.CopyMembers(originalObject, obj2, visited);
						result = obj2;
					}
				}
			}
			return result;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00073D88 File Offset: 0x00071F88
		private static void CopyMembers(object originalObject, object cloneObject, Dictionary<object, object> visited)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				foreach (FieldInfo fieldInfo in SimulationTypeCache.GetCache(originalObject.GetType()).CloneFields)
				{
					object value = FastClone.InternalCopy(fieldInfo.GetValue(originalObject), visited);
					fieldInfo.SetValue(cloneObject, value);
				}
			}
		}
	}
}
