using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020000F9 RID: 249
	public abstract class ActionCastRitual<T> : ActionOrderGOAPNode<T> where T : CastRitualOrder, new()
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00011CA6 File Offset: 0x0000FEA6
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00011CAC File Offset: 0x0000FEAC
		public override string ActionName
		{
			get
			{
				int targetPlayerId = this.GetTargetPlayerId();
				Identifier targetItemId = this.GetTargetItemId();
				HexCoord targetHexCoord = this.GetTargetHexCoord();
				if (targetPlayerId == this.OwningPlanner.PlayerId)
				{
					if (targetItemId != Identifier.Invalid)
					{
						return "Cast " + this.GetRitualId() + " on item " + base.Context.DebugName(targetItemId);
					}
					if (targetHexCoord != HexCoord.Invalid)
					{
						return string.Format("Cast {0} on hex {1}", this.GetRitualId(), targetHexCoord);
					}
					return "Cast " + this.GetRitualId();
				}
				else
				{
					if (targetItemId != Identifier.Invalid)
					{
						return string.Concat(new string[]
						{
							"Cast ",
							this.GetRitualId(),
							" on ",
							base.Context.DebugName(targetPlayerId),
							"'s item ",
							base.Context.DebugName(targetItemId)
						});
					}
					if (targetHexCoord != HexCoord.Invalid)
					{
						return string.Format("Cast {0} on {1}'s hex {2}", this.GetRitualId(), base.Context.DebugName(targetPlayerId), targetHexCoord);
					}
					return "Cast " + this.GetRitualId() + " on player " + base.Context.DebugName(targetPlayerId);
				}
			}
		}

		// Token: 0x06000408 RID: 1032
		protected abstract string GetRitualId();

		// Token: 0x06000409 RID: 1033
		protected abstract PowerType GetPowerType();

		// Token: 0x0600040A RID: 1034 RVA: 0x00011DD8 File Offset: 0x0000FFD8
		private bool TryGetScapegoat(out int scapegoatId)
		{
			scapegoatId = int.MinValue;
			IEnumerable<int> allPlayersSortedByAnimosity = this.OwningPlanner.PlayerState.Animosity.GetAllPlayersSortedByAnimosity();
			if (!IEnumerableExtensions.Any<int>(allPlayersSortedByAnimosity))
			{
				return false;
			}
			int targetPlayerId = this.GetTargetPlayerId();
			PlayerState nemesis;
			if (!this.OwningPlanner.TrueTurn.TryGetNemesis(this.OwningPlanner.PlayerState, out nemesis))
			{
				scapegoatId = IEnumerableExtensions.First<int>(this.OwningPlanner.PlayerState.Animosity.GetAllPlayersSortedByAnimosity());
				return true;
			}
			if (targetPlayerId != nemesis.Id)
			{
				scapegoatId = nemesis.Id;
				return true;
			}
			int num;
			if (allPlayersSortedByAnimosity.TryFirst(out num, (int id) => id != nemesis.Id))
			{
				scapegoatId = num;
				return true;
			}
			return false;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00011E94 File Offset: 0x00010094
		protected virtual RitualMaskingSettings GetMaskingSettings()
		{
			int maskingTargetId;
			if (this.OwningPlanner.AIPreviewPlayerState.IsRitualFramingAvailable() && this.TryGetScapegoat(out maskingTargetId))
			{
				return RitualMaskingSettings.Framing(maskingTargetId);
			}
			if (this.OwningPlanner.AIPreviewPlayerState.IsRitualMaskingAvailable())
			{
				return RitualMaskingSettings.Masked;
			}
			return RitualMaskingSettings.NoMasking;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00011EEC File Offset: 0x000100EC
		protected virtual int GetTargetPlayerId()
		{
			Identifier targetItemId = this.GetTargetItemId();
			if (targetItemId == Identifier.Invalid)
			{
				HexCoord targetHexCoord = this.GetTargetHexCoord();
				if (targetHexCoord == HexCoord.Invalid)
				{
					return this.OwningPlanner.PlayerId;
				}
				Hex hex = this.OwningPlanner.PlayerViewOfTurnState.HexBoard[targetHexCoord];
				if (hex == null)
				{
					return this.OwningPlanner.PlayerId;
				}
				return hex.ControllingPlayerID;
			}
			else
			{
				PlayerState playerState = this.OwningPlanner.PlayerViewOfTurnState.FindControllingPlayer(targetItemId);
				if (playerState == null)
				{
					return this.OwningPlanner.PlayerId;
				}
				return playerState.Id;
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00011F79 File Offset: 0x00010179
		protected virtual Identifier GetTargetItemId()
		{
			return Identifier.Invalid;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00011F7C File Offset: 0x0001017C
		protected virtual HexCoord GetTargetHexCoord()
		{
			return HexCoord.Invalid;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00011F83 File Offset: 0x00010183
		protected virtual int CooldownDuration
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00011F86 File Offset: 0x00010186
		public bool IsMasked
		{
			get
			{
				return this._isMasked;
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00011F8E File Offset: 0x0001018E
		private string GetPowerVariant()
		{
			return this.OwningPlanner.PlayerState.GetBestUnlockedVariantOfRitual(base.Context, this.GetRitualId(), this.GetPowerType());
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00011FB4 File Offset: 0x000101B4
		public bool IsAnyVariantAlreadyOngoing(string rootId)
		{
			ScalableAbility scalableAbility;
			if (!base.GameDatabase.TryFetch<ScalableAbility>(rootId, out scalableAbility))
			{
				return this.IsAlreadyOngoing(rootId);
			}
			foreach (LevelValue levelValue in scalableAbility.Levels)
			{
				if (this.IsAlreadyOngoing(levelValue.Ability.Id))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00012034 File Offset: 0x00010234
		public bool IsAlreadyOngoing(string variantId)
		{
			TurnContext trueContext = this.OwningPlanner.TrueContext;
			foreach (ActiveRitual activeRitual in trueContext.CurrentTurn.GetActiveRituals(this.OwningPlanner.PlayerState))
			{
				RitualStaticData ritualStaticData;
				if (trueContext.Database.TryFetch<RitualStaticData>(activeRitual.StaticDataId, out ritualStaticData) && ritualStaticData.ConfigRef.Id == variantId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000120C8 File Offset: 0x000102C8
		public override void Prepare()
		{
			if (this.GetPowerVariant() == null)
			{
				base.Disable("No variant of " + this.GetRitualId() + " has been unlocked");
				return;
			}
			if (this.IsAnyVariantAlreadyOngoing(this.GetRitualId()))
			{
				base.Disable("Ritual " + this.GetRitualId() + " is already on the Ritual table");
				return;
			}
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			if (this.CooldownDuration > 0)
			{
				base.AddConstraint(new WPActionCooldown(this.ID, this.CooldownDuration));
			}
			base.AddPrecondition(new WPRitualSlot());
			int targetPlayerId = this.GetTargetPlayerId();
			if (targetPlayerId != this.OwningPlanner.PlayerId)
			{
				base.AddPrecondition(new WPPowerSuperiority(this.GetPowerType(), targetPlayerId, this.GetTargetItemId()));
			}
			Identifier targetItemId = this.GetTargetItemId();
			if (targetItemId != Identifier.Invalid)
			{
				GameItem gameItem = this.OwningPlanner.AIPreviewTurn.FetchGameItem(targetItemId);
				GamePiece gamePiece = gameItem as GamePiece;
				if (gamePiece != null)
				{
					base.AddConstraint(new WPGamePieceActive(gameItem));
					if (!gamePiece.CanBeAffectedByRituals)
					{
						base.Disable(string.Format("{0} cannot be affected by rituals", gamePiece));
						return;
					}
				}
				else
				{
					GamePiece controllingPiece = this.OwningPlanner.TrueTurn.GetControllingPiece(targetItemId);
					if (controllingPiece != null && !controllingPiece.CanBeAffectedByRituals)
					{
						base.Disable(string.Format("{0} is attached to a game piece that cannot be affected by rituals", gameItem));
						return;
					}
				}
			}
			base.Prepare();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00012224 File Offset: 0x00010424
		protected override T GenerateOrder()
		{
			T t = Activator.CreateInstance<T>();
			t.AbilityId = this.GetPowerVariant();
			RitualMaskingSettings maskingSettings = this.GetMaskingSettings();
			this._isMasked = (maskingSettings.MaskingMode > RitualMaskingMode.NoMasking);
			t.RitualMaskingSettings = maskingSettings;
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this.GetTargetPlayerId());
			Identifier targetItemId = this.GetTargetItemId();
			if (targetItemId != Identifier.Invalid)
			{
				TurnState trueTurn = this.OwningPlanner.TrueTurn;
				GameItem gameItem = trueTurn.FetchGameItem(targetItemId);
				if (gameItem == null)
				{
					base.Disable(string.Format("Could not grab Game Piece {0}", targetItemId));
					return t;
				}
				if (gameItem.Category == GameItemCategory.GamePiece)
				{
					GamePiece targetGamePiece = trueTurn.FetchGameItem<GamePiece>(targetItemId);
					targetContext.SetTargetGamePiece(targetGamePiece);
				}
				else
				{
					targetContext.SetTargetGameItem(targetItemId);
				}
			}
			HexCoord targetHexCoord = this.GetTargetHexCoord();
			if (targetHexCoord != HexCoord.Invalid)
			{
				targetContext.SetTargetHex(targetHexCoord);
			}
			t.TargetContext = targetContext;
			t.Priority = this.Priority;
			return t;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001231B File Offset: 0x0001051B
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			if (this.CooldownDuration > 0)
			{
				this.OwningPlanner.AIPersistentData.RecordActionUsed(this.ID, context.CurrentTurn);
			}
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001234C File Offset: 0x0001054C
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_CastRitual objectiveCondition_CastRitual = objectiveCondition as ObjectiveCondition_CastRitual;
			if (objectiveCondition_CastRitual != null && objectiveCondition_CastRitual.Ritual.Id == this.GetRitualId())
			{
				return true;
			}
			ObjectiveCondition_CastRituals objectiveCondition_CastRituals = objectiveCondition as ObjectiveCondition_CastRituals;
			if (objectiveCondition_CastRituals != null && objectiveCondition_CastRituals.Category == this.GetPowerType())
			{
				return true;
			}
			ObjectiveCondition_DestroyLegionAfterUsingRitual objectiveCondition_DestroyLegionAfterUsingRitual = objectiveCondition as ObjectiveCondition_DestroyLegionAfterUsingRitual;
			if (objectiveCondition_DestroyLegionAfterUsingRitual != null)
			{
				using (List<ConfigRef<RitualStaticData>>.Enumerator enumerator = objectiveCondition_DestroyLegionAfterUsingRitual.AcceptedRituals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Id == this.GetRitualId())
						{
							return true;
						}
					}
				}
			}
			return base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x04000237 RID: 567
		private bool _isMasked;
	}
}
