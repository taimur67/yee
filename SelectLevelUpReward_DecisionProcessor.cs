using System;

namespace LoG
{
	// Token: 0x020004A6 RID: 1190
	public class SelectLevelUpReward_DecisionProcessor : DecisionProcessor<SelectLevelUpRewardRequest, SelectLevelUpRewardResponse>
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x0005294D File Offset: 0x00050B4D
		protected override Result Validate(SelectLevelUpRewardResponse response)
		{
			if (response.SelectedRewardIndex < 0)
			{
				return Result.SelectedTooFewOptions;
			}
			if (response.SelectedRewardIndex >= base.request.RewardOptions.Count)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00052984 File Offset: 0x00050B84
		protected override Result Process(SelectLevelUpRewardResponse response)
		{
			Problem problem = this.Validate(response) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.GamePieceId);
			if (gamePiece == null)
			{
				return Result.InvalidItem(base.request.GamePieceId);
			}
			new GamePieceModifier(base.request.RewardOptions[response.SelectedRewardIndex].Reward)
			{
				Source = new LevelUpContext()
			}.InstallInto(gamePiece, base._currentTurn, false);
			gamePiece.LevelUp(true, base.request.NewLevel);
			return Result.Success;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00052A1D File Offset: 0x00050C1D
		protected override Result Preview(SelectLevelUpRewardResponse response)
		{
			return this.Process(response);
		}
	}
}
