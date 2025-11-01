using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026A RID: 618
	public class PraetorCombatMovePowerChangedEvent : GameEvent
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00030AEE File Offset: 0x0002ECEE
		[JsonIgnore]
		public int ValueChange
		{
			get
			{
				return this.NewValue - this.OldValue;
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00030AFD File Offset: 0x0002ECFD
		[JsonConstructor]
		protected PraetorCombatMovePowerChangedEvent()
		{
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00030B05 File Offset: 0x0002ED05
		public PraetorCombatMovePowerChangedEvent(int player, Identifier praetor, ConfigRef move, int previousValue, int newValue) : base(player)
		{
			this.Praetor = praetor;
			this.CombatMove = move;
			this.OldValue = previousValue;
			this.NewValue = newValue;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00030B2C File Offset: 0x0002ED2C
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} {1} Gained {2} Power. Final: ({3})", new object[]
			{
				this.Praetor,
				this.CombatMove.Id,
				this.ValueChange,
				this.NewValue
			});
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00030B84 File Offset: 0x0002ED84
		public override void DeepClone(out GameEvent clone)
		{
			PraetorCombatMovePowerChangedEvent praetorCombatMovePowerChangedEvent = new PraetorCombatMovePowerChangedEvent
			{
				Praetor = this.Praetor,
				CombatMove = this.CombatMove.DeepClone(),
				OldValue = this.OldValue,
				NewValue = this.NewValue
			};
			base.DeepCloneGameEventParts<PraetorCombatMovePowerChangedEvent>(praetorCombatMovePowerChangedEvent);
			clone = praetorCombatMovePowerChangedEvent;
		}

		// Token: 0x04000538 RID: 1336
		[JsonProperty]
		public Identifier Praetor;

		// Token: 0x04000539 RID: 1337
		[JsonProperty]
		public ConfigRef CombatMove;

		// Token: 0x0400053A RID: 1338
		[JsonProperty]
		public int OldValue;

		// Token: 0x0400053B RID: 1339
		[JsonProperty]
		public int NewValue;
	}
}
