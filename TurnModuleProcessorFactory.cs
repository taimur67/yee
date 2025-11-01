using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200053B RID: 1339
	public static class TurnModuleProcessorFactory
	{
		// Token: 0x060019F5 RID: 6645 RVA: 0x0005AB94 File Offset: 0x00058D94
		static TurnModuleProcessorFactory()
		{
			foreach (Type type in TypeUtils.Implementable<TurnModuleProcessor>())
			{
				Type type2 = TypeExtensions.SearchBaseTypes(type, (Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(TurnModuleProcessor<>));
				if (!(type2 == null))
				{
					Type type3 = type2.GetGenericArguments()[0];
					if (typeof(TurnModuleInstance).IsAssignableFrom(type3))
					{
						TurnModuleProcessorFactory.RegisterProcessor(type3, type);
					}
				}
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0005AC28 File Offset: 0x00058E28
		public static void RegisterProcessor(Type request, Type processor)
		{
			TurnModuleProcessorFactory.ProcessorMapping[request] = processor;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005AC36 File Offset: 0x00058E36
		public static TurnModuleProcessor CreateProcessor(TurnModuleInstance instance, TurnProcessContext context)
		{
			return TurnModuleProcessorFactory.CreateProcessor(instance.GetType()).Configure(context, instance);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0005AC4C File Offset: 0x00058E4C
		private static TurnModuleProcessor CreateProcessor(Type requestType)
		{
			Type type;
			if (!TurnModuleProcessorFactory.ProcessorMapping.TryGetValue(requestType, out type))
			{
				return null;
			}
			return (TurnModuleProcessor)Activator.CreateInstance(type);
		}

		// Token: 0x04000BD4 RID: 3028
		private static Dictionary<Type, Type> ProcessorMapping = new Dictionary<Type, Type>();
	}
}
