using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.Utils;

namespace LoG
{
	// Token: 0x020004CA RID: 1226
	public class SelectTribute_DecisionProcessor : DecisionProcessor<SelectTributeDecisionRequest, SelectTributeDecisionResponse>
	{
		// Token: 0x060016FC RID: 5884 RVA: 0x0005405E File Offset: 0x0005225E
		protected override SelectTributeDecisionResponse GenerateTypedFallbackResponse()
		{
			return new SelectTributeDecisionResponse
			{
				SelectedIds = IEnumerableExtensions.ToList<int>(base.request.Candidates.ItemIds.GetRandom(this.TurnProcessContext.Random, base.request.SelectionMax))
			};
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x0005409C File Offset: 0x0005229C
		protected override Result Validate(SelectTributeDecisionResponse response)
		{
			if (base.request.SelectionMax == 0)
			{
				return Result.Success;
			}
			if (response.SelectedIds.Count > base.request.SelectionMax)
			{
				return Result.SelectedTooManyOptions;
			}
			if (response.SelectedIds.Count < base.request.SelectionMax)
			{
				return Result.SelectedTooFewOptions;
			}
			List<int> list = IEnumerableExtensions.ToList<int>(from x in response.SelectedIds
			where base.request.Candidates.ItemIds.All((int id) => id != x)
			select x);
			if (list.Count > 0)
			{
				return Result.CounterfeitTribute(this._player.Id, list);
			}
			return Result.Success;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00054138 File Offset: 0x00052338
		protected override Result Process(SelectTributeDecisionResponse response)
		{
			List<ResourceNFT> list = null;
			DemandPayload candidates = base.request.Candidates;
			if (((candidates != null) ? candidates.Tokens : null) != null)
			{
				list = IEnumerableExtensions.ToList<ResourceNFT>((from t in base.request.Candidates.Tokens
				where response.SelectedIds.Contains(t.Id)
				select t).Take(base.request.SelectionMax));
				this._player.GiveResources(list);
				EntityTag_TributeSiphon entityTag_TributeSiphon;
				if (this._player.TryGetTag<EntityTag_TributeSiphon>(base._random, out entityTag_TributeSiphon))
				{
					IEnumerable<ResourceNFT> pristineResources = from t in base.request.Candidates.Tokens
					where !response.SelectedIds.Contains(t.Id)
					select t;
					List<ResourceNFT> list2 = this.TurnProcessContext.TransmuteSiphonedTribute(this._player, pristineResources);
					if (list2 != null)
					{
						foreach (ResourceNFT resource in list2)
						{
							this.TurnProcessContext.AddSiphonedTribute(entityTag_TributeSiphon.PlayerId, resource);
						}
					}
				}
			}
			DemandPayload candidates2 = base.request.Candidates;
			if (((candidates2 != null) ? candidates2.Manuscripts : null) != null)
			{
				int num = base.request.SelectionMax;
				if (list != null)
				{
					num -= list.Count;
				}
				foreach (Manuscript item in (from m in base.request.Candidates.Manuscripts
				where response.SelectedIds.Contains((int)m.Id)
				select m).Take(num))
				{
					base._currentTurn.AddGameEvent<ItemAcquiredEvent>(new ItemAcquiredEvent(this._player.Id, item));
					this._player.AddToVault(item);
				}
			}
			return Result.Success;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00054324 File Offset: 0x00052524
		protected override Result Preview(SelectTributeDecisionResponse response)
		{
			return this.Process(response);
		}
	}
}
