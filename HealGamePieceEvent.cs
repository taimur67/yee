using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002C2 RID: 706
	public class HealGamePieceEvent : GameEvent
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00036703 File Offset: 0x00034903
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00036706 File Offset: 0x00034906
		[JsonConstructor]
		protected HealGamePieceEvent()
		{
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00036715 File Offset: 0x00034915
		public HealGamePieceEvent(int triggeringPlayerID, GamePiece affectedGamePiece, int amount) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			this.HealingAmount = amount;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00036744 File Offset: 0x00034944
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} healed {1}", this.Target, this.HealingAmount);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00036768 File Offset: 0x00034968
		public override void DeepClone(out GameEvent clone)
		{
			HealGamePieceEvent healGamePieceEvent = new HealGamePieceEvent
			{
				Target = this.Target,
				HealingAmount = this.HealingAmount
			};
			base.DeepCloneGameEventParts<HealGamePieceEvent>(healGamePieceEvent);
			clone = healGamePieceEvent;
		}

		// Token: 0x0400061F RID: 1567
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000620 RID: 1568
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int HealingAmount;
	}
}
