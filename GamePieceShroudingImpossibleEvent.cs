using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006C3 RID: 1731
	public class GamePieceShroudingImpossibleEvent : GameEvent
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x0006CA1A File Offset: 0x0006AC1A
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0006CA1D File Offset: 0x0006AC1D
		[JsonConstructor]
		protected GamePieceShroudingImpossibleEvent()
		{
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0006CA2C File Offset: 0x0006AC2C
		public GamePieceShroudingImpossibleEvent(int triggeringPlayerID, GamePiece affectedGamePiece, bool shroudingImpossible = true) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			this.ShroudingImpossible = shroudingImpossible;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0006CA5B File Offset: 0x0006AC5B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} shrouding impossibility has been set to {1} for player {2}", this.Target, this.ShroudingImpossible, this.TriggeringPlayerID);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0006CA88 File Offset: 0x0006AC88
		public override void DeepClone(out GameEvent clone)
		{
			GamePieceShroudingImpossibleEvent gamePieceShroudingImpossibleEvent = new GamePieceShroudingImpossibleEvent
			{
				Target = this.Target,
				ShroudingImpossible = this.ShroudingImpossible,
				WasAbilityAdded = this.WasAbilityAdded
			};
			base.DeepCloneGameEventParts<GamePieceShroudingImpossibleEvent>(gamePieceShroudingImpossibleEvent);
			clone = gamePieceShroudingImpossibleEvent;
		}

		// Token: 0x04000D16 RID: 3350
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000D17 RID: 3351
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public bool ShroudingImpossible;

		// Token: 0x04000D18 RID: 3352
		[JsonProperty]
		public bool WasAbilityAdded;
	}
}
