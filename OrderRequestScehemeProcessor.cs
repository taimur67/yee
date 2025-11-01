using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000643 RID: 1603
	public class OrderRequestScehemeProcessor : ActionProcessor<OrderRequestScheme>
	{
		// Token: 0x06001D97 RID: 7575 RVA: 0x000661EF File Offset: 0x000643EF
		public override Result IsAvailable()
		{
			if (this._player.NumSchemes >= this._player.SchemeSlots)
			{
				return Result.NotEnoughSlots();
			}
			if (this._player.TotalSchemeOptions <= 0)
			{
				return Result.SelectedTooFewOptions;
			}
			return Result.Success;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00066230 File Offset: 0x00064430
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			List<SchemeObjective> list = IEnumerableExtensions.ToList<SchemeObjective>(SchemeFactory.GenerateSchemesFor(this.TurnProcessContext, this._player));
			if (list.Count == 0)
			{
				return Result.Failure;
			}
			foreach (SchemeObjective schemeObjective in list)
			{
				schemeObjective.SetStartingProgress(this.TurnProcessContext, this._player);
			}
			if (list.Count == 1)
			{
				return base._currentTurn.AddSchemes(this._player, list);
			}
			this._player.AddDecisionRequest(new SelectSchemeDecisionRequest(base._currentTurn)
			{
				Options = list,
				NumSelections = this._player.NumSchemeSelections
			});
			return Result.Success;
		}
	}
}
