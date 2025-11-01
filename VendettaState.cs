using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000524 RID: 1316
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VendettaState : DiplomaticState
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00059FB9 File Offset: 0x000581B9
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Vendetta;
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00059FBC File Offset: 0x000581BC
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00059FBF File Offset: 0x000581BF
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.OnStop;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00059FC2 File Offset: 0x000581C2
		public override CantonCaptureRestrictions GetCantonCaptureRestrictions(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRestrictions.SharedPerimeter;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00059FC5 File Offset: 0x000581C5
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x00059FC8 File Offset: 0x000581C8
		// (set) Token: 0x0600199B RID: 6555 RVA: 0x00059FD0 File Offset: 0x000581D0
		[JsonIgnore]
		public Vendetta Vendetta
		{
			get
			{
				return this._vendetta;
			}
			set
			{
				this._vendetta = value;
			}
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00059FD9 File Offset: 0x000581D9
		[JsonConstructor]
		public VendettaState()
		{
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00059FE1 File Offset: 0x000581E1
		public VendettaState(Vendetta vendetta)
		{
			this._vendetta = vendetta;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00059FF0 File Offset: 0x000581F0
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			if (this._vendetta != null)
			{
				this._vendetta.TurnStarted = context.CurrentTurn.TurnValue;
			}
			base.OnStateEntered(context, pair, prevState, interrupt);
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0005A01B File Offset: 0x0005821B
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new VendettaState
			{
				_vendetta = this._vendetta.DeepClone<Vendetta>()
			};
		}

		// Token: 0x04000BAD RID: 2989
		[JsonProperty]
		private Vendetta _vendetta;
	}
}
