using System;
using System.Collections.Generic;
using Core.StaticData;

namespace LoG
{
	// Token: 0x02000536 RID: 1334
	public static class TurnModuleInstanceFactory
	{
		// Token: 0x060019E1 RID: 6625 RVA: 0x0005A8A8 File Offset: 0x00058AA8
		static TurnModuleInstanceFactory()
		{
			foreach (Type type in TypeUtils.Implementable<TurnModuleProcessor>())
			{
				Type type2 = TypeExtensions.SearchBaseTypes(type, new Predicate<Type>(TurnModuleInstanceFactory.IsValid));
				if (!(type2 == null))
				{
					Type[] genericArguments = type2.GetGenericArguments();
					if (genericArguments.Length == 2)
					{
						Type type3 = genericArguments[0];
						if (typeof(TurnModuleInstance).IsAssignableFrom(type3))
						{
							Type type4 = genericArguments[1];
							if (typeof(IdentifiableStaticData).IsAssignableFrom(type4))
							{
								TurnModuleInstanceFactory.RegisterAssociation(type4, type3);
							}
						}
					}
				}
			}
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005A954 File Offset: 0x00058B54
		private static bool IsValid(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(TurnModuleProcessor<, >);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0005A975 File Offset: 0x00058B75
		public static void RegisterAssociation(Type key, Type val)
		{
			TurnModuleInstanceFactory.TypeMapping.Add(key, val);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0005A983 File Offset: 0x00058B83
		public static T CreateInstance<T>(TurnState turn) where T : TurnModuleInstance
		{
			return (T)((object)TurnModuleInstanceFactory.CreateInstance(turn, typeof(T)));
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0005A99C File Offset: 0x00058B9C
		public static TurnModuleInstance CreateInstance(TurnState turn, Type type)
		{
			TurnModuleInstance turnModuleInstance = (TurnModuleInstance)Activator.CreateInstance(type);
			if (turnModuleInstance == null)
			{
				return null;
			}
			turnModuleInstance.Id = (TurnModuleInstanceId)turn.NextIdentifier();
			turnModuleInstance.CreatedTurn = turn.TurnValue;
			return turnModuleInstance;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0005A9D4 File Offset: 0x00058BD4
		public static TurnModuleInstance CreateInstance(TurnState turn, IdentifiableStaticData data)
		{
			if (data == null)
			{
				return null;
			}
			Type type;
			if (!TurnModuleInstanceFactory.TypeMapping.TryGetValue(data.GetType(), out type))
			{
				return null;
			}
			TurnModuleInstance turnModuleInstance = (TurnModuleInstance)Activator.CreateInstance(type);
			if (turnModuleInstance == null)
			{
				return null;
			}
			turnModuleInstance.StaticDataId = data.ConfigRef;
			turnModuleInstance.Id = (TurnModuleInstanceId)turn.NextIdentifier();
			turnModuleInstance.CreatedTurn = turn.TurnValue;
			return turnModuleInstance;
		}

		// Token: 0x04000BCF RID: 3023
		private static Dictionary<Type, Type> TypeMapping = new Dictionary<Type, Type>();
	}
}
