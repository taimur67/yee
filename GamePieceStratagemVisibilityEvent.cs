using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006C2 RID: 1730
	public class GamePieceStratagemVisibilityEvent : GameEvent
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0006C969 File Offset: 0x0006AB69
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0006C96C File Offset: 0x0006AB6C
		[JsonConstructor]
		protected GamePieceStratagemVisibilityEvent()
		{
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0006C97B File Offset: 0x0006AB7B
		public GamePieceStratagemVisibilityEvent(int triggeringPlayerID, GamePiece affectedGamePiece, bool stratagemsRevealed = true) : base(triggeringPlayerID)
		{
			this.Target = affectedGamePiece.Id;
			this.StratagemsRevealed = stratagemsRevealed;
			base.AddAffectedPlayerId(affectedGamePiece.ControllingPlayerId);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0006C9AA File Offset: 0x0006ABAA
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePiece {0} stratagem visibility has been set to {1} for player {2}", this.Target, this.StratagemsRevealed, this.TriggeringPlayerID);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0006C9D8 File Offset: 0x0006ABD8
		public override void DeepClone(out GameEvent clone)
		{
			GamePieceStratagemVisibilityEvent gamePieceStratagemVisibilityEvent = new GamePieceStratagemVisibilityEvent
			{
				Target = this.Target,
				StratagemsRevealed = this.StratagemsRevealed,
				WasAbilityAdded = this.WasAbilityAdded
			};
			base.DeepCloneGameEventParts<GamePieceStratagemVisibilityEvent>(gamePieceStratagemVisibilityEvent);
			clone = gamePieceStratagemVisibilityEvent;
		}

		// Token: 0x04000D13 RID: 3347
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000D14 RID: 3348
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public bool StratagemsRevealed;

		// Token: 0x04000D15 RID: 3349
		[JsonProperty]
		public bool WasAbilityAdded;
	}
}
