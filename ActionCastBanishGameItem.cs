using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020000EC RID: 236
	public class ActionCastBanishGameItem : ActionCastRitual<BanishGameItemRitualOrder>
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Banish_Praetor;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		protected override string GetRitualId()
		{
			return "banish_praetor";
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00010BEB File Offset: 0x0000EDEB
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00010BEE File Offset: 0x0000EDEE
		protected override Identifier GetTargetItemId()
		{
			return this._targetGameItemID;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00010BF6 File Offset: 0x0000EDF6
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00010BF9 File Offset: 0x0000EDF9
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00010BFC File Offset: 0x0000EDFC
		public ActionCastBanishGameItem(Identifier targetGameItemID)
		{
			this._targetGameItemID = targetGameItemID;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00010C0C File Offset: 0x0000EE0C
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			int targetPlayerId = this.GetTargetPlayerId();
			if (!base.IsMasked)
			{
				base.AddPrecondition(new WPCanAttack(targetPlayerId, false));
			}
			GameItem gameItem = this.OwningPlanner.PlayerViewOfTurnState.FetchGameItem(this._targetGameItemID);
			if (gameItem == null)
			{
				base.Disable(string.Format("Item {0} is invalid", this._targetGameItemID));
				return;
			}
			PlayerState playerState = this.OwningPlanner.PlayerViewOfTurnState.FindControllingPlayer(this._targetGameItemID);
			if (playerState == null)
			{
				base.Disable(string.Format("Item {0} has invalid owner", this._targetGameItemID));
				return;
			}
			base.AddEffect(new WPDuelAdvantage(playerState.Id));
			GamePiece gamePiece;
			if (this.OwningPlanner.PlayerViewOfTurnState.TryFindControllingPiece(this._targetGameItemID, out gamePiece))
			{
				PraetorStaticData praetorStaticData = base.GameDatabase.Fetch<PraetorStaticData>(gameItem.StaticDataId);
				if (praetorStaticData == null)
				{
					base.Disable(string.Format("Item {0} has no PraetorStaticData", this._targetGameItemID));
					return;
				}
				CombatStats bonus = praetorStaticData.Components.OfType<GamePieceModifierStaticData>().CalculatePowerChange(gamePiece);
				base.AddEffect(WPCombatAdvantage.BonusAgainst(gamePiece, bonus));
			}
		}

		// Token: 0x0400021A RID: 538
		public const string RitualId = "banish_praetor";

		// Token: 0x0400021B RID: 539
		private readonly Identifier _targetGameItemID;
	}
}
