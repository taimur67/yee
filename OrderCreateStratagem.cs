using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200062F RID: 1583
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderCreateStratagem : ActionableOrder, AttachmentProcessor.IRequest
	{
		// Token: 0x06001D39 RID: 7481 RVA: 0x00064F63 File Offset: 0x00063163
		public ConfigRef GetAbilityRef(TurnContext context)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return null;
			}
			return dataForRequest.ConfigRef;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x00064F77 File Offset: 0x00063177
		public override Result TargetItemStolen(ConfigRef order, int originalOwner, GameItem item)
		{
			return new Result.StratagemTargetStolenProblem(order, item);
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00064F85 File Offset: 0x00063185
		public override Result TargetItemBanished(ConfigRef order, GameItem item)
		{
			return new Result.StratagemTargetBanishedProblem(order, item);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00064F93 File Offset: 0x00063193
		public override Result TargetItemInvalid(ConfigRef order, GameItem item)
		{
			return Result.InvalidItem(item);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00064FA0 File Offset: 0x000631A0
		public Result TargetItemUnattached(ConfigRef order, GameItem unattachedItem)
		{
			return Result.Success;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00064FA7 File Offset: 0x000631A7
		public Result TargetItemLocked(ConfigRef order, GameItem lockedItem, GamePiece lockingPiece)
		{
			return Result.Success;
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001D3F RID: 7487 RVA: 0x00064FAE File Offset: 0x000631AE
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Stratagem;
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00064FB2 File Offset: 0x000631B2
		[JsonConstructor]
		public OrderCreateStratagem()
		{
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x00064FC6 File Offset: 0x000631C6
		public OrderCreateStratagem(Identifier targetPieceId, int tacticSlots)
		{
			this.TargetPieceId = targetPieceId;
			this.TacticsIds = new string[tacticSlots];
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00064FED File Offset: 0x000631ED
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new ForgeStratagemConflict(this.TargetPieceId);
			yield break;
		}

		// Token: 0x04000C87 RID: 3207
		[JsonProperty]
		public string[] TacticsIds = new string[0];

		// Token: 0x04000C88 RID: 3208
		[JsonProperty]
		public Identifier TargetPieceId;
	}
}
