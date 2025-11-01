using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000114 RID: 276
	public class ActionForgeCombatCard : ActionOrderGOAPNode<OrderCreateStratagem>
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00014E7A File Offset: 0x0001307A
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00014E7D File Offset: 0x0001307D
		public override ActionID ID
		{
			get
			{
				return ActionID.Forge_Combat_Card;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00014E81 File Offset: 0x00013081
		public override string ActionName
		{
			get
			{
				return "Forge Stratagem on " + base.Context.DebugName(this.LegionID);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014E9E File Offset: 0x0001309E
		public ActionForgeCombatCard(Identifier legionID)
		{
			this.LegionID = legionID;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014EAD File Offset: 0x000130AD
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00014EB8 File Offset: 0x000130B8
		public override void Prepare()
		{
			if (this.OwningPlanner.AIPreviewTurn.FetchGameItem<GamePiece>(this.LegionID) == null)
			{
				base.Disable(string.Format("Invalid game piece {0}", this.LegionID));
				return;
			}
			base.AddConstraint(new WPGamePieceHasFreeSlot(this.LegionID, FreeSlotsMode.Half));
			float num = (float)this.OwningPlanner.PlayerState.PowersLevels[PowerType.Wrath].CurrentLevel;
			int num2 = this.OwningPlanner.PlayerState.StratagemTacticSlots;
			int num3 = (int)MathF.Round(num * (float)num2 / 3f);
			if (num3 <= 0)
			{
				base.Disable("Wrath is too low for stratagems to be worth using");
				return;
			}
			CombatStats bonus = new CombatStats
			{
				Ranged = num3,
				Melee = num3,
				Infernal = num3
			};
			base.AddEffect(WPCombatAdvantage.BonusFor(this.LegionID, bonus));
			base.AddEffect(new WPLegionHasCombatCard(this.LegionID));
			this.DecideTactics(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.PlayerState);
			if (base.Order.TacticsIds == null || base.Order.TacticsIds.Count<string>() == 0)
			{
				base.Disable("Stratagem - no available tactics");
				return;
			}
			base.Prepare();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00014FFB File Offset: 0x000131FB
		protected override OrderCreateStratagem GenerateOrder()
		{
			this._numTacticsSlots = this.OwningPlanner.PlayerState.StratagemTacticSlots;
			return new OrderCreateStratagem(this.LegionID, this._numTacticsSlots);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001502C File Offset: 0x0001322C
		private void DecideTactics(TurnContext context, PlayerState playerState)
		{
			base.Order.TargetPieceId = this.LegionID;
			int num = this._numTacticsSlots;
			List<StratagemTacticStaticData> source = IEnumerableExtensions.ToList<StratagemTacticStaticData>(context.CurrentTurn.GetAvailableTactics(this.OwningPlanner.Database, playerState));
			List<StratagemTacticStaticData> list = new List<StratagemTacticStaticData>();
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(this.LegionID);
			if (num >= 0)
			{
				BattlePhase weakestStat = gamePiece.CombatStats.GetWeakestStat();
				if (weakestStat != BattlePhase.Undefined)
				{
					StratagemTacticStaticData stratagemTacticStaticData = null;
					if (weakestStat == BattlePhase.Ranged)
					{
						stratagemTacticStaticData = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Skip_Ranged);
					}
					else if (weakestStat == BattlePhase.Melee)
					{
						stratagemTacticStaticData = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Skip_Melee);
					}
					else if (weakestStat == BattlePhase.Infernal)
					{
						stratagemTacticStaticData = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Skip_Infernal);
					}
					if (stratagemTacticStaticData != null)
					{
						num--;
						list.Add(stratagemTacticStaticData);
					}
				}
			}
			if (num >= 0)
			{
				BattlePhase strongestStat = gamePiece.CombatStats.GetStrongestStat();
				StratagemTacticStaticData stratagemTacticStaticData2 = null;
				if (strongestStat == BattlePhase.Melee)
				{
					stratagemTacticStaticData2 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.GoFirst_Melee);
				}
				else if (strongestStat == BattlePhase.Infernal)
				{
					stratagemTacticStaticData2 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.GoFirst_Infernal);
				}
				if (stratagemTacticStaticData2 != null)
				{
					num--;
					list.Add(stratagemTacticStaticData2);
				}
			}
			if (num >= 0)
			{
				BattlePhase weakestStat2 = gamePiece.CombatStats.GetWeakestStat();
				StratagemTacticStaticData stratagemTacticStaticData3 = null;
				if (weakestStat2 != BattlePhase.Undefined)
				{
					if (weakestStat2 == BattlePhase.Ranged)
					{
						stratagemTacticStaticData3 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.DecreaseEnemy_Ranged);
					}
					else if (weakestStat2 == BattlePhase.Melee)
					{
						stratagemTacticStaticData3 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.DecreaseEnemy_Melee);
					}
					else if (weakestStat2 == BattlePhase.Infernal)
					{
						stratagemTacticStaticData3 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.DecreaseEnemy_Infernal);
					}
					if (stratagemTacticStaticData3 != null)
					{
						num--;
						list.Add(stratagemTacticStaticData3);
					}
				}
			}
			if (num >= 0)
			{
				StratagemTacticStaticData stratagemTacticStaticData4 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Increase_Health);
				if (stratagemTacticStaticData4 != null)
				{
					num--;
					list.Add(stratagemTacticStaticData4);
				}
			}
			if (num > 0)
			{
				int num2 = this.OwningPlanner.Random.Next(3);
				StratagemTacticStaticData stratagemTacticStaticData5;
				if (num2 == 1)
				{
					stratagemTacticStaticData5 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Increase_Ranged);
				}
				else if (num2 == 2)
				{
					stratagemTacticStaticData5 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Increase_Melee);
				}
				else
				{
					stratagemTacticStaticData5 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Increase_Infernal);
				}
				if (stratagemTacticStaticData5 != null)
				{
					num--;
					list.Add(stratagemTacticStaticData5);
				}
			}
			if (num > 0)
			{
				StratagemTacticStaticData stratagemTacticStaticData6 = source.FirstOrDefault((StratagemTacticStaticData t) => t.StratagemType == StratagemType.Dummy);
				if (stratagemTacticStaticData6 != null)
				{
					num--;
					list.Add(stratagemTacticStaticData6);
				}
			}
			List<string> list2 = new List<string>();
			foreach (StratagemTacticStaticData rootData in list)
			{
				StratagemTacticLevelStaticData tacticAtLevel = context.Database.GetTacticAtLevel(rootData, playerState.PowersLevels[PowerType.Wrath]);
				list2.Add(tacticAtLevel.Id);
			}
			base.Order.TacticsIds = list2.ToArray();
		}

		// Token: 0x0400027B RID: 635
		public Identifier LegionID;

		// Token: 0x0400027C RID: 636
		private int _numTacticsSlots;
	}
}
