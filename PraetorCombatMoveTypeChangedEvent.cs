using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026B RID: 619
	public class PraetorCombatMoveTypeChangedEvent : GameEvent
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x00030BD7 File Offset: 0x0002EDD7
		[JsonConstructor]
		public PraetorCombatMoveTypeChangedEvent()
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00030BDF File Offset: 0x0002EDDF
		public PraetorCombatMoveTypeChangedEvent(int player, Identifier praetor, PraetorCombatMoveInstance previousValue, PraetorCombatMoveInstance newValue) : base(player)
		{
			this.Praetor = praetor;
			this.OldValue = previousValue;
			this.NewValue = newValue;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00030C00 File Offset: 0x0002EE00
		public override void DeepClone(out GameEvent clone)
		{
			PraetorCombatMoveTypeChangedEvent praetorCombatMoveTypeChangedEvent = new PraetorCombatMoveTypeChangedEvent
			{
				Praetor = this.Praetor,
				OldValue = this.OldValue.DeepClone<PraetorCombatMoveInstance>(),
				NewValue = this.NewValue.DeepClone<PraetorCombatMoveInstance>()
			};
			base.DeepCloneGameEventParts<PraetorCombatMoveTypeChangedEvent>(praetorCombatMoveTypeChangedEvent);
			clone = praetorCombatMoveTypeChangedEvent;
		}

		// Token: 0x0400053C RID: 1340
		[JsonProperty]
		public Identifier Praetor;

		// Token: 0x0400053D RID: 1341
		[JsonProperty]
		public PraetorCombatMoveInstance OldValue;

		// Token: 0x0400053E RID: 1342
		[JsonProperty]
		public PraetorCombatMoveInstance NewValue;
	}
}
