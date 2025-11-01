using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003C4 RID: 964
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Praetor_CombatMoveLevelUp_DecisionRequest : DecisionRequest<Praetor_CombatMoveLevelUp_DecisionResponse>
	{
		// Token: 0x060012ED RID: 4845 RVA: 0x00048319 File Offset: 0x00046519
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.None;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0004831C File Offset: 0x0004651C
		[JsonConstructor]
		protected Praetor_CombatMoveLevelUp_DecisionRequest()
		{
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00048332 File Offset: 0x00046532
		public Praetor_CombatMoveLevelUp_DecisionRequest(DecisionId decisionId, Identifier praetor) : base(decisionId)
		{
			this.Praetor = praetor;
		}

		// Token: 0x040008C7 RID: 2247
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Praetor = Identifier.Invalid;

		// Token: 0x040008C8 RID: 2248
		[JsonProperty]
		public int AdditionalPower = 1;
	}
}
