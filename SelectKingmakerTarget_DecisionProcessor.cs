using System;
using System.Linq;

namespace LoG
{
	// Token: 0x020004C1 RID: 1217
	public class SelectKingmakerTarget_DecisionProcessor : DecisionProcessor<SelectKingmakerTargetRequest, SelectKingmakerTargetResponse>
	{
		// Token: 0x060016C2 RID: 5826 RVA: 0x0005382C File Offset: 0x00051A2C
		protected override SelectKingmakerTargetResponse GenerateTypedFallbackResponse()
		{
			SelectKingmakerTargetResponse response = new SelectKingmakerTargetResponse();
			response.SelectedTargetId = (from x in base._currentTurn.EnumeratePlayerStates(false, false)
			where response.IsValidArchfiend(this.TurnProcessContext, x.Id, this._player.Id)
			select x).GetRandom(base._random).Id;
			return response;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00053890 File Offset: 0x00051A90
		protected override Result Validate(SelectKingmakerTargetResponse response)
		{
			if (base.request.DecisionRequired(this.TurnProcessContext) && response.SelectedTargetId == -2147483648)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000538C0 File Offset: 0x00051AC0
		protected override Result Process(SelectKingmakerTargetResponse response)
		{
			if (base.request.DecisionRequired(this.TurnProcessContext) && response.SelectedTargetId == -2147483648)
			{
				response.SelectedTargetId = (from x in base._currentTurn.EnumeratePlayerStates(false, false)
				where x.Id != this._player.Id
				select x).GetRandom(base._currentTurn.Random).Id;
			}
			this._player.KingmakerPuppetId = response.SelectedTargetId;
			return Result.Success;
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x0005393C File Offset: 0x00051B3C
		protected override Result Preview(SelectKingmakerTargetResponse response)
		{
			return this.Process(response);
		}
	}
}
