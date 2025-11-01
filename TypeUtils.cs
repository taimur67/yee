using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoG
{
	// Token: 0x020006F0 RID: 1776
	public static class TypeUtils
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x00076884 File Offset: 0x00074A84
		public static IEnumerable<Type> AllTypes
		{
			get
			{
				return (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.IsDynamic
				select a).SelectMany((Assembly assembly) => assembly.GetExportedTypes());
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000768E3 File Offset: 0x00074AE3
		public static IEnumerable<Type> Implementable(this Assembly assembly)
		{
			return assembly.GetExportedTypes().Where(new Func<Type, bool>(TypeUtils.IsImplementable));
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000768FC File Offset: 0x00074AFC
		public static IEnumerable<Type> Implementable<T>()
		{
			return TypeUtils.AllTypes.Where(new Func<Type, bool>(TypeUtils.IsImplementable<T>));
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00076914 File Offset: 0x00074B14
		public static IEnumerable<Type> EnumerateImplementableTypes(this Assembly assembly)
		{
			return assembly.GetExportedTypes().Where(new Func<Type, bool>(TypeUtils.IsImplementable));
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00076930 File Offset: 0x00074B30
		public static bool IsTriviallyConstructed(this Type type, bool allowNonPublic = false)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (allowNonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, CallingConventions.HasThis, new Type[0], null);
			return constructor != null && constructor.GetParameters().Length == 0;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00076970 File Offset: 0x00074B70
		public static bool HasAttributeInHierarchy<T>(this Type type) where T : Attribute
		{
			foreach (Type type2 in type.EnumerateSelfAndHierarchy())
			{
				if (!(type2 == typeof(object)) && type2.GetCustomAttribute<T>() != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000769DC File Offset: 0x00074BDC
		public static bool CanImplicitCast<TargetType>(this Type type)
		{
			return TypeUtils.CanImplicitCast(type, typeof(TargetType));
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000769F0 File Offset: 0x00074BF0
		public static bool CanImplicitCast(Type type, Type target)
		{
			return target.IsAssignableFrom(type) || (from mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
			where mi.Name == "op_Implicit" && mi.ReturnType == target
			select mi).Any(delegate(MethodInfo mi)
			{
				ParameterInfo parameterInfo = IEnumerableExtensions.FirstOrDefault<ParameterInfo>(mi.GetParameters());
				return parameterInfo != null && parameterInfo.ParameterType == type;
			});
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00076A55 File Offset: 0x00074C55
		public static bool IsPrimitive(this Type type)
		{
			return type == typeof(string) || (type.IsValueType & type.IsPrimitive);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00076A78 File Offset: 0x00074C78
		public static bool IsImplementable(this Type type)
		{
			return type != null && !type.IsAbstract && !type.ContainsGenericParameters;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00076A96 File Offset: 0x00074C96
		public static bool IsImplementable<T>(this Type type)
		{
			return type.IsImplementable() && typeof(T).IsAssignableFrom(type);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00076AB2 File Offset: 0x00074CB2
		public static IEnumerable<Type> EnumerateBaseHierarchy(this Type type)
		{
			if (type == null)
			{
				yield break;
			}
			type = type.BaseType;
			while (type != null)
			{
				yield return type;
				type = type.BaseType;
			}
			yield break;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00076AC2 File Offset: 0x00074CC2
		public static IEnumerable<Type> EnumerateSelfAndHierarchy(this Type type)
		{
			yield return type;
			foreach (Type type2 in type.EnumerateBaseHierarchy())
			{
				yield return type2;
			}
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00076AD2 File Offset: 0x00074CD2
		public static IEnumerable<PropertyInfo> EnumeratePropertiesOfType<T>(this Type type)
		{
			return type.EnumeratePropertiesOfType(typeof(T));
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00076AE4 File Offset: 0x00074CE4
		public static IEnumerable<PropertyInfo> EnumeratePropertiesOfType(this Type type, Type other)
		{
			return from property in other.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where other.IsAssignableFrom(property.PropertyType)
			select property;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00076B1C File Offset: 0x00074D1C
		public static IEnumerable<FieldInfo> EnumerateFieldsOfType<T>(this Type type)
		{
			return type.EnumerateFieldsOfType(typeof(T));
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00076B30 File Offset: 0x00074D30
		public static IEnumerable<FieldInfo> EnumerateFieldsOfType(this Type type, Type other)
		{
			return from field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where other.IsAssignableFrom(field.FieldType)
			select field;
		}
	}
}
