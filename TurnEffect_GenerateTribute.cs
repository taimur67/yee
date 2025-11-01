using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200034F RID: 847
	public class TurnEffect_GenerateTribute : TurnAbilityEffect
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0003F9B9 File Offset: 0x0003DBB9
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Tribute;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003F9C0 File Offset: 0x0003DBC0
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			if (this.OnlyOnOfferingTurn && !context.IsOfferingTurn())
			{
				return;
			}
			PlayerState playerState = context.CurrentTurn.FindPlayerState(piece.ControllingPlayerId, null);
			List<ResourceNFT> list = new List<ResourceNFT>();
			foreach (CostStaticData costStaticData in this.Tokens)
			{
				list.Add(context.CurrentTurn.CreateNFT(new ResourceAccumulation[]
				{
					costStaticData
				}));
			}
			PaymentReceivedEvent ev = context.GivePayment(playerState, new Payment
			{
				Resources = list
			}, null);
			AbilityGenerateTributeEvent abilityGenerateTributeEvent = new AbilityGenerateTributeEvent(playerState.Id, ability.ProviderId, ability.SourceId);
			abilityGenerateTributeEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			context.CurrentTurn.AddGameEvent<AbilityGenerateTributeEvent>(abilityGenerateTributeEvent);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0003FA9C File Offset: 0x0003DC9C
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_GenerateTribute turnEffect_GenerateTribute = new TurnEffect_GenerateTribute
			{
				Tokens = this.Tokens.DeepClone<CostStaticData>(),
				OnlyOnOfferingTurn = this.OnlyOnOfferingTurn
			};
			base.DeepCloneTurnAbilityEffectParts(turnEffect_GenerateTribute);
			clone = turnEffect_GenerateTribute;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0003FAD8 File Offset: 0x0003DCD8
		public ResourceAccumulation GetTotalResourcesGenerated()
		{
			ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
			foreach (CostStaticData costStaticData in this.Tokens)
			{
				ResourceAccumulation resourceAccumulation2 = resourceAccumulation;
				resourceAccumulation2[ResourceTypes.Prestige] = resourceAccumulation2[ResourceTypes.Prestige] + costStaticData.Prestige;
				resourceAccumulation2 = resourceAccumulation;
				resourceAccumulation2[ResourceTypes.Souls] = resourceAccumulation2[ResourceTypes.Souls] + costStaticData.Soul;
				resourceAccumulation2 = resourceAccumulation;
				resourceAccumulation2[ResourceTypes.Ichor] = resourceAccumulation2[ResourceTypes.Ichor] + costStaticData.Ichor;
				resourceAccumulation2 = resourceAccumulation;
				resourceAccumulation2[ResourceTypes.Hellfire] = resourceAccumulation2[ResourceTypes.Hellfire] + costStaticData.Hellfire;
				resourceAccumulation2 = resourceAccumulation;
				resourceAccumulation2[ResourceTypes.Darkness] = resourceAccumulation2[ResourceTypes.Darkness] + costStaticData.Darkness;
			}
			return resourceAccumulation;
		}

		// Token: 0x0400077B RID: 1915
		[JsonProperty]
		public List<CostStaticData> Tokens = new List<CostStaticData>();

		// Token: 0x0400077C RID: 1916
		[JsonProperty]
		[DefaultValue(false)]
		public bool OnlyOnOfferingTurn;
	}
}
