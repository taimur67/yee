using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000470 RID: 1136
	[Serializable]
	public class SimpleSchemeGenerator : BasicSchemeGenerator
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x000500E1 File Offset: 0x0004E2E1
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x000500E9 File Offset: 0x0004E2E9
		[JsonProperty]
		public List<ObjectiveCondition> Conditions { get; set; } = new List<ObjectiveCondition>();

		// Token: 0x0600152F RID: 5423 RVA: 0x000500F2 File Offset: 0x0004E2F2
		[JsonConstructor]
		public SimpleSchemeGenerator()
		{
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00050105 File Offset: 0x0004E305
		public SimpleSchemeGenerator(params ObjectiveCondition[] conditions) : this(conditions.AsEnumerable<ObjectiveCondition>())
		{
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00050113 File Offset: 0x0004E313
		public SimpleSchemeGenerator(IEnumerable<ObjectiveCondition> conditions)
		{
			this.Conditions = conditions.ToList<ObjectiveCondition>();
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00050134 File Offset: 0x0004E334
		protected override IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			List<ObjectiveCondition> conditions = this.Conditions;
			if (conditions != null)
			{
				conditions.ForEach(delegate(ObjectiveCondition x)
				{
					if (x != null)
					{
						x.Start(context.CurrentTurn, player);
					}
				});
			}
			return this.Conditions;
		}
	}
}
