using System;

namespace LoG
{
	// Token: 0x02000648 RID: 1608
	public class SeekManuscriptsActionActionProcessor : ActionProcessor<OrderSeekManuscripts>
	{
		// Token: 0x06001DAA RID: 7594 RVA: 0x00066481 File Offset: 0x00064681
		public static int GetAdjustedManuscriptDraw(int baseDraw, int basePick, int orderSlot)
		{
			return Math.Max(basePick + 1, baseDraw - orderSlot);
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0006648E File Offset: 0x0006468E
		public override Result IsUnLocked()
		{
			if (this._player.PowersLevels[PowerType.Charisma] < 2)
			{
				return new PowerTooLowProblem(PowerType.Charisma, 2);
			}
			return Result.Success;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000664B8 File Offset: 0x000646B8
		public override Result IsAvailable()
		{
			Result result = this.IsUnLocked(base.request);
			Problem problem = result as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (!this._player.IsDrawTributeAvailable)
			{
				return Result.Failure;
			}
			return result;
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000664F7 File Offset: 0x000646F7
		public override Result Validate()
		{
			return this.IsAvailable();
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00066500 File Offset: 0x00064700
		public override Result Process(ActionProcessContext context)
		{
			SeekManuscriptUtils.ManuscriptParameters seekManuscriptParameters = this._player.GetSeekManuscriptParameters(this.TurnProcessContext);
			seekManuscriptParameters.Draw = SeekManuscriptsActionActionProcessor.GetAdjustedManuscriptDraw(seekManuscriptParameters.Draw, seekManuscriptParameters.Pick, context.OrderSlotIndex);
			Problem problem = SeekManuscriptUtils.CreateAndAddSeekManuscriptDecisionToPlayer(this.TurnProcessContext, this._player, seekManuscriptParameters) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.TurnProcessContext.TributeContext.IncrementDraw(this._player);
			return Result.Success;
		}
	}
}
