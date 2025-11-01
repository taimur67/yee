using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020004A3 RID: 1187
	public static class DecisionProcessorFactory
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x000527D4 File Offset: 0x000509D4
		static DecisionProcessorFactory()
		{
			foreach (Type type in TypeUtils.Implementable<DecisionProcessor>())
			{
				Type type2 = TypeExtensions.SearchBaseTypes(type, (Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(DecisionProcessor<, >));
				if (!(type2 == null))
				{
					Type type3 = type2.GetGenericArguments()[0];
					if (typeof(DecisionRequest).IsAssignableFrom(type3))
					{
						DecisionProcessorFactory.RegisterProcessor(type3, type);
					}
				}
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00052868 File Offset: 0x00050A68
		public static void RegisterProcessor(Type requestType, Type processor)
		{
			DecisionProcessorFactory.ProcessorMapping[requestType] = processor;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00052876 File Offset: 0x00050A76
		public static DecisionProcessor PrepareProcessor(TurnProcessContext context, PlayerState player, DecisionRequest decisionRequest, DecisionResponse response)
		{
			return DecisionProcessorFactory.CreateProcessor(decisionRequest.GetType()).Configure(context, player, decisionRequest);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0005288C File Offset: 0x00050A8C
		public static DecisionProcessor CreateProcessor(Type requestType)
		{
			Type type;
			if (!DecisionProcessorFactory.ProcessorMapping.TryGetValue(requestType, out type))
			{
				return null;
			}
			return (DecisionProcessor)Activator.CreateInstance(type);
		}

		// Token: 0x04000B12 RID: 2834
		private static Dictionary<Type, Type> ProcessorMapping = new Dictionary<Type, Type>();
	}
}
