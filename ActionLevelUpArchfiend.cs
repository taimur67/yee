using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020000E5 RID: 229
	public class ActionLevelUpArchfiend : ActionOrderGOAPNode<OrderLevelUp>
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000FBC2 File Offset: 0x0000DDC2
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000FBC5 File Offset: 0x0000DDC5
		public override string ActionName
		{
			get
			{
				return string.Format("Level up power {0} to {1}", this.PowerType, this.NewLevel);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000FBE7 File Offset: 0x0000DDE7
		public override ActionID ID
		{
			get
			{
				return ActionID.Levelup_Archfiend;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000FBEB File Offset: 0x0000DDEB
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000FBEE File Offset: 0x0000DDEE
		public ActionLevelUpArchfiend(PowerType powerType, int newLevel)
		{
			this.PowerType = powerType;
			this.NewLevel = newLevel;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000FC04 File Offset: 0x0000DE04
		public static void ApplyEffects(GOAPNode node, PowerType powerType, int newLevel)
		{
			float amount = 0f;
			node.AddEffect(new WPPowerLevel(powerType, newLevel));
			PlayerState playerState = node.OwningPlanner.PlayerState;
			foreach (PlayerState playerState2 in node.OwningPlanner.AIPreviewTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState2.Id != playerState.Id && playerState2.PowersLevels[powerType].CurrentLevel > newLevel - 1)
				{
					node.AddEffect(new WPPowerSuperiority(powerType, playerState2.Id, Identifier.Invalid));
				}
			}
			TurnState playerViewOfTurnState = node.OwningPlanner.PlayerViewOfTurnState;
			int num = playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id).Count<GamePiece>();
			switch (powerType)
			{
			case PowerType.Wrath:
				node.AddEffect(new WPCommandRating(1));
				if (num >= playerState.CommandRating)
				{
					foreach (PlayerState playerState3 in playerViewOfTurnState.EnumeratePlayerStatesExcept(node.OwningPlanner.PlayerId, false, false))
					{
						node.AddEffect(new WPMilitarySuperiority(node.OwningPlanner.PlayerId, playerState3.Id, 0.5f));
					}
				}
				using (List<AITag>.Enumerator enumerator2 = playerState.AITags.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AITag aitag = enumerator2.Current;
						if (aitag != AITag.Assassin)
						{
							if (aitag != AITag.Conqueror)
							{
								if (aitag == AITag.Iconoclast)
								{
									ref amount.LerpTo01(0.6f);
								}
							}
							else
							{
								ref amount.LerpTo01(0.8f);
							}
						}
						else
						{
							ref amount.LerpTo01(0.7f);
						}
					}
					goto IL_503;
				}
				break;
			case PowerType.Deceit:
				break;
			case PowerType.Prophecy:
				goto IL_284;
			case PowerType.Destruction:
				goto IL_355;
			case PowerType.Charisma:
				goto IL_3FA;
			default:
				goto IL_503;
			}
			int num2 = playerState.AITags.Contains(AITag.Trickster) ? 4 : 5;
			if (newLevel <= num2)
			{
				foreach (PlayerState playerState4 in playerViewOfTurnState.EnumeratePlayerStates(false, false))
				{
					if (playerState.Id != playerState4.Id)
					{
						node.AddEffect(new WPUndermineArchfiend(playerState4.Id));
					}
				}
			}
			using (List<AITag>.Enumerator enumerator2 = playerState.AITags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					AITag aitag2 = enumerator2.Current;
					if (aitag2 != AITag.Hoarder)
					{
						if (aitag2 == AITag.Trickster)
						{
							ref amount.LerpTo01(0.9f);
						}
					}
					else
					{
						ref amount.LerpTo01(0.5f);
					}
				}
				goto IL_503;
			}
			IL_284:
			node.AddEffect(new WPRitualSlot());
			if (newLevel <= 3)
			{
				foreach (PlayerState playerState5 in playerViewOfTurnState.EnumeratePlayerStates(false, false))
				{
					if (playerState.Id != playerState5.Id)
					{
						node.AddEffect(new WPUndermineArchfiend(playerState5.Id));
					}
				}
			}
			using (List<AITag>.Enumerator enumerator2 = playerState.AITags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					AITag aitag3 = enumerator2.Current;
					if (aitag3 != AITag.Conqueror)
					{
						if (aitag3 != AITag.Schemer)
						{
							if (aitag3 == AITag.Trickster)
							{
								ref amount.LerpTo01(0.4f);
							}
						}
						else
						{
							ref amount.LerpTo01(0.8f);
						}
					}
					else
					{
						node.AddScalarCostIncrease(0.3f, PFCostModifier.Heuristic_Bonus);
					}
				}
				goto IL_503;
			}
			IL_355:
			if (num >= 2)
			{
				foreach (PlayerState playerState6 in playerViewOfTurnState.EnumeratePlayerStatesExcept(node.OwningPlanner.PlayerId, false, false))
				{
					node.AddEffect(new WPMilitarySuperiority(node.OwningPlanner.PlayerId, playerState6.Id, 0.5f));
				}
			}
			using (List<AITag>.Enumerator enumerator2 = playerState.AITags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == AITag.Supervillain)
					{
						ref amount.LerpTo01(0.6f);
					}
				}
				goto IL_503;
			}
			IL_3FA:
			if (WPHasAnyPraetor.Check(playerViewOfTurnState, playerState.Id))
			{
				foreach (PlayerState playerState7 in playerViewOfTurnState.EnumeratePlayerStates(false, false))
				{
					if (playerState7.Id != playerState.Id)
					{
						node.AddEffect(new WPDuelAdvantage(playerState7.Id));
					}
				}
			}
			using (List<AITag>.Enumerator enumerator2 = playerState.AITags.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					switch (enumerator2.Current)
					{
					case AITag.Conqueror:
						node.AddScalarCostIncrease(0.2f, PFCostModifier.Heuristic_Bonus);
						break;
					case AITag.Duellist:
						ref amount.LerpTo01(0.2f);
						break;
					case AITag.Hoarder:
						ref amount.LerpTo01(0.3f);
						break;
					case AITag.Iconoclast:
						node.AddScalarCostIncrease(0.6f, PFCostModifier.Heuristic_Bonus);
						break;
					case AITag.Supervillain:
						node.AddScalarCostIncrease(0.4f, PFCostModifier.Heuristic_Bonus);
						break;
					}
				}
			}
			IL_503:
			int num3 = (from powerLevel in playerState.PowersLevels.Powers
			select powerLevel.CurrentLevel.Value).Max();
			if (newLevel > num3)
			{
				if (num3 < 4)
				{
					ActionLevelUpArchfiend.AddExtraOrderSlotEffects(node, playerViewOfTurnState, playerState);
				}
				node.AddEffect(new WPTributeDraw(WorldProperty.MaxWeight));
				ref amount.LerpTo01(0.5f);
			}
			if (newLevel == 4)
			{
				ActionLevelUpArchfiend.AddExtraOrderSlotEffects(node, playerViewOfTurnState, playerState);
				ref amount.LerpTo01(0.6f);
			}
			node.AddScalarCostReduction(amount, PFCostModifier.Archfiend_Bonus);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010224 File Offset: 0x0000E424
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			ActionLevelUpArchfiend.ApplyEffects(this, this.PowerType, this.NewLevel);
			base.Prepare();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010258 File Offset: 0x0000E458
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_ArchfiendPowersAtLevel objectiveCondition_ArchfiendPowersAtLevel = objectiveCondition as ObjectiveCondition_ArchfiendPowersAtLevel;
			if (objectiveCondition_ArchfiendPowersAtLevel != null && this.NewLevel >= objectiveCondition_ArchfiendPowersAtLevel.TargetLevel)
			{
				return true;
			}
			ObjectiveCondition_ArchfiendPowerLevel objectiveCondition_ArchfiendPowerLevel = objectiveCondition as ObjectiveCondition_ArchfiendPowerLevel;
			return (objectiveCondition_ArchfiendPowerLevel != null && objectiveCondition_ArchfiendPowerLevel.ArchfiendPower == this.PowerType) || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000102A0 File Offset: 0x0000E4A0
		private static void AddExtraOrderSlotEffects(GOAPNode node, TurnState turnView, PlayerState controlledPlayer)
		{
			if (turnView.GetAllActiveLegionsForPlayer(controlledPlayer.Id).Count<GamePiece>() > controlledPlayer.OrderSlots)
			{
				foreach (PlayerState playerState in turnView.EnumeratePlayerStatesExcept(node.OwningPlanner.PlayerId, false, false))
				{
					node.AddEffect(new WPMilitarySuperiority(node.OwningPlanner.PlayerId, playerState.Id, 0.5f));
				}
			}
			node.AddEffect(new WPTributeDraw(WorldProperty.MaxWeight));
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00010344 File Offset: 0x0000E544
		protected override OrderLevelUp GenerateOrder()
		{
			return new OrderLevelUp
			{
				PowerType = this.PowerType,
				Level = this.NewLevel,
				Priority = ActionOrderPriority.Low
			};
		}

		// Token: 0x0400020D RID: 525
		public PowerType PowerType;

		// Token: 0x0400020E RID: 526
		public int NewLevel;
	}
}
