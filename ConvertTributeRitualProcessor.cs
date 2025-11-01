using System;
using System.Collections.Generic;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000663 RID: 1635
	public class ConvertTributeRitualProcessor : TargetedRitualActionProcessor<ConvertTributeRitualOrder, ConvertTributeRitualData, RitualCastEvent>
	{
		// Token: 0x06001E31 RID: 7729 RVA: 0x00068060 File Offset: 0x00066260
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this._player.ValidatePayment(base.request.ConversionPayment);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00068094 File Offset: 0x00066294
		public override Result Preview(ActionProcessContext context)
		{
			Problem problem = base.Preview(context) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return this._player.RemovePayment(base.request.ConversionPayment);
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000680CC File Offset: 0x000662CC
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			int num = (int)Math.Floor((double)((float)base.request.ConversionPayment.Resources.Count * base.data.TokenCountConversionPercentage));
			num = Math.Max(1, num);
			List<ResourceNFT> list = new List<ResourceNFT>();
			for (int i = 0; i < num; i++)
			{
				ResourceNFT resourceNFT = base._currentTurn.CreateNFT(Array.Empty<ResourceAccumulation>());
				resourceNFT.Values[base.request.TargetResourceType] = 1;
				list.Add(resourceNFT);
			}
			int num2 = (int)Math.Floor((double)((float)base.request.ConversionPayment.Total.ValueSum * base.data.TokenValueConversionPercentage));
			num2 = Math.Min(num2, num * base.data.MaxTokenSize);
			num2 -= num;
			for (int j = 0; j < num2; j++)
			{
				ResourceAccumulation values = IEnumerableExtensions.ToList<ResourceNFT>(from x in list
				where x.Values[base.request.TargetResourceType] < base.data.MaxTokenSize
				select x).GetRandom(base.Random).Values;
				ResourceTypes targetResourceType = base.request.TargetResourceType;
				int value = values[targetResourceType] + 1;
				values[targetResourceType] = value;
			}
			this.TurnProcessContext.RemovePayment(this._player, base.request.ConversionPayment, null);
			PaymentReceivedEvent ev = this._player.GiveResources(list);
			ritualCastEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			return Result.Success;
		}
	}
}
