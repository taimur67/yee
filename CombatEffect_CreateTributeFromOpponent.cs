using System;

namespace LoG
{
	// Token: 0x02000336 RID: 822
	[Serializable]
	public class CombatEffect_CreateTributeFromOpponent : CombatAbilityEffect
	{
		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003E7AC File Offset: 0x0003C9AC
		protected override GameEvent OnPostBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			if (!battleEvent.BattleResult.DidWin(context.Actor))
			{
				return null;
			}
			GamePiece gamePiece;
			if (!battleEvent.BattleResult.TryGetLosingPiece_EndState(out gamePiece))
			{
				return null;
			}
			ResourceAccumulation resourceAccumulation = ResourceAccumulation.CreateWithoutPrestige((int)Math.Ceiling((double)((float)gamePiece.Level * this.LevelToTributePercentage)));
			ResourceNFT resourceNFT = context.Turn.CreateNFT(new ResourceAccumulation[]
			{
				resourceAccumulation
			});
			resourceNFT.VisualOverrideId = "Beelzebub";
			PlayerState playerState = context.Turn.FindPlayerState(context.Actor.ControllingPlayerId, null);
			Payment payment = new Payment(new ResourceNFT[]
			{
				resourceNFT
			});
			PaymentReceivedEvent ev = context.TurnContext.GivePayment(playerState, payment, null);
			TributeFromKillEvent tributeFromKillEvent = new TributeFromKillEvent(playerState.Id, gamePiece.Id);
			tributeFromKillEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			context.Turn.AddGameEvent<TributeFromKillEvent>(tributeFromKillEvent);
			return null;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003E888 File Offset: 0x0003CA88
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_CreateTributeFromOpponent combatEffect_CreateTributeFromOpponent = new CombatEffect_CreateTributeFromOpponent
			{
				LevelToTributePercentage = this.LevelToTributePercentage
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_CreateTributeFromOpponent);
			clone = combatEffect_CreateTributeFromOpponent;
		}

		// Token: 0x0400075A RID: 1882
		public float LevelToTributePercentage;
	}
}
