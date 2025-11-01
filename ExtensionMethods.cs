using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ModestTree;

namespace LoG
{
	// Token: 0x020001DB RID: 475
	public static class ExtensionMethods
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x0002C539 File Offset: 0x0002A739
		public static FieldInfo[] AllMemberFields<T>()
		{
			return typeof(T).AllMemberFields();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002C54A File Offset: 0x0002A74A
		public static FieldInfo[] AllNonDefaultMemberFields<T>(object instance)
		{
			return typeof(T).AllNonDefaultMemberFields(instance);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002C55C File Offset: 0x0002A75C
		public static FieldInfo[] AllMemberFields(this Type t)
		{
			return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002C566 File Offset: 0x0002A766
		public static FieldInfo[] AllMemberFields(this Type t, BindingFlags additionalFlags)
		{
			return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | additionalFlags);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002C574 File Offset: 0x0002A774
		public static FieldInfo[] AllNonDefaultMemberFields(this Type t, object instance)
		{
			return IEnumerableExtensions.ToArray<FieldInfo>(from x in t.AllMemberFields()
			where !x.IsDefaultValue(instance)
			select x);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002C5AC File Offset: 0x0002A7AC
		public static MemberInfo[] AllMemberFieldsAndProperties(this Type t, BindingFlags flags)
		{
			IEnumerable<MemberInfo> first = t.GetFields(flags).Cast<MemberInfo>();
			IEnumerable<MemberInfo> second = t.GetProperties(flags).Cast<MemberInfo>();
			return IEnumerableExtensions.ToArray<MemberInfo>(first.Concat(second));
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002C5DD File Offset: 0x0002A7DD
		public static MemberInfo[] AllMemberFieldsAndProperties(this Type t)
		{
			return t.AllMemberFieldsAndProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
		public static object GetValue(this MemberInfo memberInfo, object obj)
		{
			MemberTypes memberType = memberInfo.MemberType;
			if (memberType == MemberTypes.Field)
			{
				return ((FieldInfo)memberInfo).GetValue(obj);
			}
			if (memberType != MemberTypes.Property)
			{
				throw new NotImplementedException();
			}
			return ((PropertyInfo)memberInfo).GetValue(obj);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002C626 File Offset: 0x0002A826
		public static MemberInfo GetFieldOrProperty(this Type type, string name)
		{
			return type.GetField(name) ?? type.GetProperty(name);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002C63C File Offset: 0x0002A83C
		public static void SetFieldOrPropertyValue(this MemberInfo memberInfo, object obj, object value)
		{
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(obj, value);
				return;
			}
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj, value);
				return;
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0002C670 File Offset: 0x0002A870
		public static bool IsDefaultValue(this FieldInfo field, object instance)
		{
			object value = field.GetValue(instance);
			DefaultValueAttribute defaultValueAttribute = TypeExtensions.TryGetAttribute<DefaultValueAttribute>(field);
			object obj = TypeExtensions.GetDefaultValue(field.FieldType);
			if (defaultValueAttribute != null)
			{
				obj = defaultValueAttribute.Value;
			}
			return value.Equals(obj);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002C6A8 File Offset: 0x0002A8A8
		public static FieldInfo ReturnField<T>(this T source, string fieldName)
		{
			return source.GetType().AllMemberFields().FirstOrDefault((FieldInfo sourceField) => sourceField.Name == fieldName);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002C6E8 File Offset: 0x0002A8E8
		public static void SetField<T, TType>(this T source, string fieldName, TType fieldInfo)
		{
			foreach (FieldInfo fieldInfo2 in source.GetType().AllMemberFields())
			{
				if (!(fieldInfo2.Name != fieldName) && !(fieldInfo2.FieldType != fieldInfo.GetType()))
				{
					fieldInfo2.SetValue(source, fieldInfo);
					return;
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002C758 File Offset: 0x0002A958
		public static void CopyFieldsFrom<T>(this T target, T source)
		{
			foreach (FieldInfo fieldInfo in source.GetType().AllMemberFields())
			{
				fieldInfo.SetValue(target, fieldInfo.GetValue(source));
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002C7A4 File Offset: 0x0002A9A4
		public static void CopyFieldsWithAttributeFrom<T>(this T target, T source, Type attributeType)
		{
			if (target == null)
			{
				return;
			}
			if (source == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in from field in source.GetType().AllMemberFields()
			where Attribute.IsDefined(field, attributeType)
			select field)
			{
				fieldInfo.SetValue(target, fieldInfo.GetValue(source));
			}
		}
	}
}
