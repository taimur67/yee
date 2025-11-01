using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000141 RID: 321
	public class GoalCaptureSpecificPoP : GoalGOAPNode
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x000201D8 File Offset: 0x0001E3D8
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Capture_Specific_PoP;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x000201DC File Offset: 0x0001E3DC
		public override string ActionName
		{
			get
			{
				return "Goal - Capture " + base.Context.DebugName(this.TargetPoP);
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000201F9 File Offset: 0x0001E3F9
		public GoalCaptureSpecificPoP(Identifier targetPoP, int expectedTurnsToReach)
		{
			this.TargetPoP = targetPoP;
			this.TurnsToReach = expectedTurnsToReach;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002020F File Offset: 0x0001E40F
		public override void Prepare()
		{
			base.AddPrecondition(new WPSpecificPopCaptured(this.TargetPoP));
			base.Prepare();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00020228 File Offset: 0x0001E428
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (this.OwningPlanner.IsEndGame)
			{
				return 0f;
			}
			int num = playerViewOfTurnState.GetAllActivePoPsForPlayer(playerState.Id, false).Count<GamePiece>();
			float num2 = 1f;
			float num3 = 1f + 0.2f * (float)num;
			num2 /= num3;
			float num4 = 1f + 0.3f * MathF.Max(0f, (float)(this.TurnsToReach - 2));
			num2 /= num4;
			using (List<AITag>.Enumerator enumerator = playerState.AITags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float amount;
					if (enumerator.Current == AITag.Conqueror)
					{
						amount = 0.2f;
					}
					else
					{
						amount = 0f;
					}
					ref num2.LerpTo01(amount);
				}
			}
			PlayerState playerState2;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2))
			{
				ref num2.LerpTo01(-0.4f);
			}
			if (playerViewOfTurnState.CheckInstigatedVendettaWithAnyPlayer(playerState, false))
			{
				ref num2.LerpTo01(-0.8f);
			}
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref num2.LerpTo01(-0.8f);
			}
			if (!this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.GameProgress > 0.7f)
			{
				ref num2.LerpTo01(-0.5f);
			}
			return num2;
		}

		// Token: 0x040002FB RID: 763
		public Identifier TargetPoP;

		// Token: 0x040002FC RID: 764
		public int TurnsToReach;
	}
}
