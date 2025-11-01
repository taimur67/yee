using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000662 RID: 1634
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ConvertTributeRitualOrder : CastTargetedRitualOrder
	{
		// Token: 0x06001E29 RID: 7721 RVA: 0x00068001 File Offset: 0x00066201
		public ConvertTributeRitualOrder() : this(string.Empty)
		{
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0006800E File Offset: 0x0006620E
		public ConvertTributeRitualOrder(string ritualId) : base(ritualId)
		{
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x00068017 File Offset: 0x00066217
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			this.TargetContext.SetTargetPlayer(player.Id);
			yield return new ActionPhase_SelectTributeType(delegate(ResourceTypes x)
			{
				this.TargetResourceType = x;
			}, new ActionPhase_Target<ResourceTypes>.IsValidFunc(this.IsValidTributeType), 1);
			yield return new ActionPhase_Tribute(delegate(Payment x)
			{
				this.ConversionPayment = x;
			});
			yield break;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0006802E File Offset: 0x0006622E
		private Result IsValidTributeType(TurnContext context, List<ResourceTypes> selected, ResourceTypes type, int castingPlayerId)
		{
			return Result.Success;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00068035 File Offset: 0x00066235
		public override IEnumerable<ResourceNFT> GetReservedResources()
		{
			foreach (ResourceNFT resourceNFT in base.GetReservedResources())
			{
				yield return resourceNFT;
			}
			IEnumerator<ResourceNFT> enumerator = null;
			foreach (ResourceNFT resourceNFT2 in this.ConversionPayment.Resources)
			{
				yield return resourceNFT2;
			}
			List<ResourceNFT>.Enumerator enumerator2 = default(List<ResourceNFT>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04000CCE RID: 3278
		[JsonProperty]
		public Payment ConversionPayment;

		// Token: 0x04000CCF RID: 3279
		[JsonProperty]
		public ResourceTypes TargetResourceType;
	}
}
