using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020000F1 RID: 241
	public class ActionCastDebuffGamePiece : ActionCastRitual<DebuffGamePieceRitualOrder>
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000112B5 File Offset: 0x0000F4B5
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Debuff_Gamepiece_Destruction;
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000112B9 File Offset: 0x0000F4B9
		protected override string GetRitualId()
		{
			return "enfeeble";
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000112C0 File Offset: 0x0000F4C0
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000112C3 File Offset: 0x0000F4C3
		protected override Identifier GetTargetItemId()
		{
			return this._targetGamePieceID;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x000112CB File Offset: 0x0000F4CB
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000112CE File Offset: 0x0000F4CE
		protected override int CooldownDuration
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000112D1 File Offset: 0x0000F4D1
		public ActionCastDebuffGamePiece(Identifier targetGamePieceId)
		{
			this._targetGamePieceID = targetGamePieceId;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000112E0 File Offset: 0x0000F4E0
		private int StatValue([TupleElementNames(new string[]
		{
			"statType",
			"statValue"
		})] ValueTuple<CombatStatType, ModifiableValue> statValuePair)
		{
			return statValuePair.Item2.Value;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000112F0 File Offset: 0x0000F4F0
		protected override DebuffGamePieceRitualOrder GenerateOrder()
		{
			DebuffGamePieceRitualOrder debuffGamePieceRitualOrder = base.GenerateOrder();
			if (debuffGamePieceRitualOrder == null)
			{
				return null;
			}
			GamePiece gamePiece = this.OwningPlanner.TrueTurn.FetchGameItem<GamePiece>(this._targetGamePieceID);
			if (gamePiece == null)
			{
				return debuffGamePieceRitualOrder;
			}
			foreach (ValueTuple<CombatStatType, ModifiableValue> valueTuple in gamePiece.CombatStats.EnumerateStats().OrderByDescending(new Func<ValueTuple<CombatStatType, ModifiableValue>, int>(this.StatValue)))
			{
				CombatStatType item = valueTuple.Item1;
				if (!debuffGamePieceRitualOrder.TryAddStat(this.OwningPlanner.Database, item))
				{
					break;
				}
			}
			return debuffGamePieceRitualOrder;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00011394 File Offset: 0x0000F594
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			GamePiece gamePiece;
			if (!this.OwningPlanner.TrueTurn.TryFetchGameItem<GamePiece>(this._targetGamePieceID, out gamePiece))
			{
				base.Disable(string.Format("Can't access GamePiece {0}", this._targetGamePieceID));
				return;
			}
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindControllingPlayer(gamePiece);
			if (playerState == null)
			{
				base.Disable(string.Format("Can't access controlling players of {0}", this._targetGamePieceID));
				return;
			}
			Identifier strongholdId = playerState.StrongholdId;
			if (this._targetGamePieceID == strongholdId)
			{
				base.AddEffect(new WPArchfiendEliminated(playerState.Id));
				base.AddPrecondition(new WPCanEliminate(playerState.Id));
			}
			else if (playerState.Id != -1)
			{
				base.AddPrecondition(new WPCanAttack(playerState.Id, false));
			}
			int maxTurnsToReach = gamePiece.IsFixture() ? 1 : 2;
			base.AddPrecondition(new WPHasForcesNearby(gamePiece, maxTurnsToReach));
			Identifier targetItemId = this.GetTargetItemId();
			GamePiece pandaemonium = this.OwningPlanner.TrueTurn.GetPandaemonium();
			if (targetItemId == pandaemonium.Id && !this.OwningPlanner.PlayerState.Excommunicated)
			{
				base.AddPrecondition(WPForcesFlanking.RequiredTargetToBeFlanked(this._targetGamePieceID));
			}
			int val = this.OwningPlanner.PlayerState.PowersLevels[PowerType.Destruction].CurrentLevel.Value / 2 + 3;
			CombatStats combatStats = new CombatStats();
			using (List<CombatStatType>.Enumerator enumerator = base.Order.TargetStats.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case CombatStatType.Ranged:
						combatStats.Ranged = val;
						break;
					case CombatStatType.Melee:
						combatStats.Melee = val;
						break;
					case CombatStatType.Infernal:
						combatStats.Infernal = val;
						break;
					}
				}
			}
			base.AddEffect(WPCombatAdvantage.BonusAgainst(this._targetGamePieceID, combatStats));
		}

		// Token: 0x04000226 RID: 550
		public const string RitualId = "enfeeble";

		// Token: 0x04000227 RID: 551
		private readonly Identifier _targetGamePieceID;
	}
}
