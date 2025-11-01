using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BurtsevAlexey;
using ModestTree;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006EE RID: 1774
	public static class SimulationTypeCache
	{
		// Token: 0x060021E6 RID: 8678 RVA: 0x000764E4 File Offset: 0x000746E4
		public static SimulationTypeCache.Cache GetCache(Type type)
		{
			object typeLock = SimulationTypeCache._typeLock;
			SimulationTypeCache.Cache result;
			lock (typeLock)
			{
				SimulationTypeCache.Cache cache;
				if (!SimulationTypeCache._typeCache.TryGetValue(type, out cache))
				{
					cache = new SimulationTypeCache.Cache(type);
					SimulationTypeCache._typeCache.Add(type, cache);
				}
				result = cache;
			}
			return result;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00076544 File Offset: 0x00074744
		public static void PurgeTypeCache()
		{
			object typeLock = SimulationTypeCache._typeLock;
			lock (typeLock)
			{
				SimulationTypeCache._typeCache.Clear();
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00076588 File Offset: 0x00074788
		public static void PreCache(Assembly assembly)
		{
			SimulationTypeCache.PreCache(assembly.EnumerateImplementableTypes());
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00076598 File Offset: 0x00074798
		public static void PreCache(IEnumerable<Type> types)
		{
			foreach (Type type in types)
			{
				SimulationTypeCache._typeCache.Add(type, new SimulationTypeCache.Cache(type));
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000765EC File Offset: 0x000747EC
		public static bool TryReflectMemberValue(object source, string propertyName, out object value, out MemberInfo memberInfo)
		{
			SimulationTypeCache.Cache cache = SimulationTypeCache.GetCache(source.GetType());
			FieldInfo fieldInfo;
			if (cache.TryGetField(propertyName, out fieldInfo))
			{
				memberInfo = fieldInfo;
				value = fieldInfo.GetValue(source);
				return true;
			}
			PropertyInfo propertyInfo;
			if (cache.TryGetProperty(propertyName, out propertyInfo))
			{
				memberInfo = propertyInfo;
				value = propertyInfo.GetValue(source);
				return true;
			}
			memberInfo = null;
			value = null;
			return false;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x00076640 File Offset: 0x00074840
		public static bool TryReflectMemberValue<T>(object source, string propertyName, out T value, out MemberInfo memberInfo)
		{
			object obj;
			if (SimulationTypeCache.TryReflectMemberValue(source, propertyName, out obj, out memberInfo) && obj is T)
			{
				T t = (T)((object)obj);
				value = t;
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0007667C File Offset: 0x0007487C
		public static bool TryReflectMemberValue<T>(object source, string propertyName, out T value)
		{
			MemberInfo memberInfo;
			return SimulationTypeCache.TryReflectMemberValue<T>(source, propertyName, out value, out memberInfo);
		}

		// Token: 0x04000F02 RID: 3842
		private static object _typeLock = new object();

		// Token: 0x04000F03 RID: 3843
		private static Dictionary<Type, SimulationTypeCache.Cache> _typeCache = new Dictionary<Type, SimulationTypeCache.Cache>();

		// Token: 0x02000B10 RID: 2832
		public class Cache
		{
			// Token: 0x1700078E RID: 1934
			// (get) Token: 0x06003414 RID: 13332 RVA: 0x000A0A73 File Offset: 0x0009EC73
			// (set) Token: 0x06003415 RID: 13333 RVA: 0x000A0A7B File Offset: 0x0009EC7B
			public Type Type { get; protected set; }

			// Token: 0x1700078F RID: 1935
			// (get) Token: 0x06003416 RID: 13334 RVA: 0x000A0A84 File Offset: 0x0009EC84
			public IReadOnlyCollection<FieldInfo> Fields
			{
				get
				{
					return this._fields.Values;
				}
			}

			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x06003417 RID: 13335 RVA: 0x000A0A91 File Offset: 0x0009EC91
			public IReadOnlyCollection<PropertyInfo> Properties
			{
				get
				{
					return this._properties.Values;
				}
			}

			// Token: 0x06003418 RID: 13336 RVA: 0x000A0A9E File Offset: 0x0009EC9E
			public bool TryGetProperty(string name, out PropertyInfo info)
			{
				return this._properties.TryGetValue(name, out info);
			}

			// Token: 0x06003419 RID: 13337 RVA: 0x000A0AAD File Offset: 0x0009ECAD
			public bool TryGetField(string name, out FieldInfo info)
			{
				return this._fields.TryGetValue(name, out info);
			}

			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x0600341A RID: 13338 RVA: 0x000A0ABC File Offset: 0x0009ECBC
			public IReadOnlyList<FieldInfo> CloneFields
			{
				get
				{
					List<FieldInfo> result;
					if ((result = this._cloneFields) == null)
					{
						result = (this._cloneFields = IEnumerableExtensions.ToList<FieldInfo>(this.Fields.Where(new Func<FieldInfo, bool>(SimulationTypeCache.Cache.IsClonableField))));
					}
					return result;
				}
			}

			// Token: 0x0600341B RID: 13339 RVA: 0x000A0AF8 File Offset: 0x0009ECF8
			public Cache(Type type)
			{
				this.Set(type);
			}

			// Token: 0x0600341C RID: 13340 RVA: 0x000A0B1D File Offset: 0x0009ED1D
			public void Set(Type type)
			{
				this.Type = type;
				this.CopyFields(type);
				this.CopyProperties(type);
			}

			// Token: 0x0600341D RID: 13341 RVA: 0x000A0B34 File Offset: 0x0009ED34
			private void CopyProperties(Type type)
			{
				this._properties.Clear();
				foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					this.AddProperty(propertyInfo);
				}
			}

			// Token: 0x0600341E RID: 13342 RVA: 0x000A0B70 File Offset: 0x0009ED70
			private void CopyFields(Type type)
			{
				this._fields.Clear();
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
				foreach (FieldInfo fieldInfo in type.GetFields(bindingAttr))
				{
					this.AddField(fieldInfo);
				}
				this.RecursiveCopyBaseTypePrivateFields(type.BaseType);
			}

			// Token: 0x0600341F RID: 13343 RVA: 0x000A0BB8 File Offset: 0x0009EDB8
			private void RecursiveCopyBaseTypePrivateFields(Type typeToReflect)
			{
				if (typeToReflect == null)
				{
					return;
				}
				this.RecursiveCopyBaseTypePrivateFields(typeToReflect.BaseType);
				foreach (FieldInfo fieldInfo in from t in typeToReflect.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
				where t.IsPrivate
				select t)
				{
					this.AddField(fieldInfo);
				}
			}

			// Token: 0x06003420 RID: 13344 RVA: 0x000A0C44 File Offset: 0x0009EE44
			private void AddField(FieldInfo fieldInfo)
			{
				this._fields.Add(fieldInfo.Name, fieldInfo);
			}

			// Token: 0x06003421 RID: 13345 RVA: 0x000A0C58 File Offset: 0x0009EE58
			private void AddProperty(PropertyInfo propertyInfo)
			{
				if (propertyInfo.IsSpecialName)
				{
					return;
				}
				if (propertyInfo.GetIndexParameters().Length != 0)
				{
					return;
				}
				this._properties.Add(propertyInfo.Name, propertyInfo);
			}

			// Token: 0x06003422 RID: 13346 RVA: 0x000A0C7F File Offset: 0x0009EE7F
			public static bool IsClonableField(FieldInfo fieldInfo)
			{
				return !TypeExtensions.HasAttribute<JsonIgnoreAttribute>(fieldInfo) && !fieldInfo.FieldType.IsPrimitive();
			}

			// Token: 0x04001C0D RID: 7181
			private Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();

			// Token: 0x04001C0E RID: 7182
			private Dictionary<string, FieldInfo> _fields = new Dictionary<string, FieldInfo>();

			// Token: 0x04001C0F RID: 7183
			private List<FieldInfo> _cloneFields;
		}
	}
}
