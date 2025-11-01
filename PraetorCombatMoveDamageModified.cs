using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AF RID: 943
	public class PraetorCombatMoveDamageModified : GameEvent
	{
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0004684C File Offset: 0x00044A4C
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0004684F File Offset: 0x00044A4F
		[JsonConstructor]
		protected PraetorCombatMoveDamageModified()
		{
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0004685E File Offset: 0x00044A5E
		public PraetorCombatMoveDamageModified(Identifier praetor, int oldValue, int newValue)
		{
			this.Praetor = praetor;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00046882 File Offset: 0x00044A82
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Praetor {0} Card Power changed from {1} to {2}", context.Debug_GetItemName(this.Praetor), this.OldValue, this.NewValue);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x000468B0 File Offset: 0x00044AB0
		public override void DeepClone(out GameEvent clone)
		{
			PraetorCombatMoveDamageModified praetorCombatMoveDamageModified = new PraetorCombatMoveDamageModified
			{
				Praetor = this.Praetor,
				OldValue = this.OldValue,
				NewValue = this.NewValue
			};
			base.DeepCloneGameEventParts<PraetorCombatMoveDamageModified>(praetorCombatMoveDamageModified);
			clone = praetorCombatMoveDamageModified;
		}

		// Token: 0x04000896 RID: 2198
		[JsonIgnore]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Praetor = Identifier.Invalid;

		// Token: 0x04000897 RID: 2199
		[JsonProperty]
		public int OldValue;

		// Token: 0x04000898 RID: 2200
		[JsonProperty]
		public int NewValue;
	}
}
