using System;
using System.Collections.Generic;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000495 RID: 1173
	public static class ActionInstanceFactory
	{
		// Token: 0x060015E9 RID: 5609 RVA: 0x00051E75 File Offset: 0x00050075
		static ActionInstanceFactory()
		{
			ActionInstanceFactory.Populate();
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00051E86 File Offset: 0x00050086
		public static void Init()
		{
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00051E88 File Offset: 0x00050088
		private static void Populate()
		{
			foreach (Type type in TypeUtils.Implementable<ActionProcessor>())
			{
				Type type2 = TypeExtensions.SearchBaseTypes(type, new Predicate<Type>(ActionInstanceFactory.IsValid));
				if (!(type2 == null))
				{
					Type[] genericArguments = type2.GetGenericArguments();
					if (genericArguments.Length == 2)
					{
						Type type3 = genericArguments[0];
						if (typeof(ActionableOrder).IsAssignableFrom(type3))
						{
							Type type4 = genericArguments[1];
							if (typeof(AbilityStaticData).IsAssignableFrom(type4))
							{
								ActionInstanceFactory.RegisterAssociation(type4, type3);
							}
						}
					}
				}
			}
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00051F2C File Offset: 0x0005012C
		private static bool IsValid(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ActionProcessor<, >);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00051F4D File Offset: 0x0005014D
		public static void RegisterAssociation(Type key, Type val)
		{
			ActionInstanceFactory.TypeMapping.Add(key, val);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00051F5C File Offset: 0x0005015C
		public static ActionableOrder CreateInstance(AbilityStaticData data)
		{
			Type type;
			if (!ActionInstanceFactory.TypeMapping.TryGetValue(data.GetType(), out type))
			{
				return null;
			}
			ActionableOrder actionableOrder = (ActionableOrder)Activator.CreateInstance(type);
			if (actionableOrder == null)
			{
				return null;
			}
			actionableOrder.AbilityId = data.Id;
			return actionableOrder;
		}

		// Token: 0x04000B04 RID: 2820
		private static Dictionary<Type, Type> TypeMapping = new Dictionary<Type, Type>();
	}
}
