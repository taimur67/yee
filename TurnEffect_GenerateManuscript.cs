using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200034E RID: 846
	public class TurnEffect_GenerateManuscript : TurnAbilityEffect
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x0003F881 File Offset: 0x0003DA81
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Tribute;
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003F888 File Offset: 0x0003DA88
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			if (this.OnlyOnOfferingTurn && !context.IsOfferingTurn())
			{
				return;
			}
			PlayerState playerState = context.CurrentTurn.FindPlayerState(piece.ControllingPlayerId, null);
			List<Manuscript> list = new List<Manuscript>();
			for (int i = 0; i < this.GenerateCount; i++)
			{
				ManuscriptStaticData weightedPurchasableManuscript = context.GetWeightedPurchasableManuscript(this.CandidateCategories);
				Manuscript item = context.CurrentTurn.SpawnManuscript(weightedPurchasableManuscript, playerState);
				list.Add(item);
			}
			GameEvent ev = context.GiveItems(playerState, list);
			AbilityGenerateManuscriptEvent abilityGenerateManuscriptEvent;
			if (context.CurrentTurn.TryGetGameEvent<AbilityGenerateManuscriptEvent>(out abilityGenerateManuscriptEvent))
			{
				abilityGenerateManuscriptEvent.NumGenerated += this.GenerateCount;
				abilityGenerateManuscriptEvent.AddChildEvent(ev);
				return;
			}
			abilityGenerateManuscriptEvent = new AbilityGenerateManuscriptEvent(playerState.Id, this.GenerateCount, ability.ProviderId, ability.SourceId);
			abilityGenerateManuscriptEvent.AddChildEvent(ev);
			context.CurrentTurn.AddGameEvent<AbilityGenerateManuscriptEvent>(abilityGenerateManuscriptEvent);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003F964 File Offset: 0x0003DB64
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_GenerateManuscript turnEffect_GenerateManuscript = new TurnEffect_GenerateManuscript
			{
				CandidateCategories = this.CandidateCategories.DeepClone<ManuscriptCategory>(),
				GenerateCount = this.GenerateCount,
				OnlyOnOfferingTurn = this.OnlyOnOfferingTurn
			};
			base.DeepCloneTurnAbilityEffectParts(turnEffect_GenerateManuscript);
			clone = turnEffect_GenerateManuscript;
		}

		// Token: 0x04000778 RID: 1912
		[JsonProperty]
		public ManuscriptCategory[] CandidateCategories;

		// Token: 0x04000779 RID: 1913
		[JsonProperty]
		[DefaultValue(1)]
		public int GenerateCount = 1;

		// Token: 0x0400077A RID: 1914
		[JsonProperty]
		[DefaultValue(false)]
		public bool OnlyOnOfferingTurn;
	}
}
