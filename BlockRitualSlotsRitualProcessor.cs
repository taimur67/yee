using System;
using System.Collections.Generic;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200065B RID: 1627
	public class BlockRitualSlotsRitualProcessor : TargetedRitualActionProcessor<BlockRitualSlotsRitualOrder, BlockRitualSlotsRitualData, RitualCastEvent>
	{
		// Token: 0x06001E13 RID: 7699 RVA: 0x00067916 File Offset: 0x00065B16
		public override Result Validate()
		{
			return Result.Success;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00067920 File Offset: 0x00065B20
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			if (playerState == null)
			{
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			ModifierStaticDataExtensions.ArchfiendStatModification archfiendStatModification = base.data.Modifiers.EffectiveModifications(playerState).FirstOrDefault((ModifierStaticDataExtensions.ArchfiendStatModification m) => m.Stat == ArchfiendStat.RitualSlots);
			if (archfiendStatModification.Stat != ArchfiendStat.RitualSlots)
			{
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			int num = playerState.RitualState.NumRitualSlots + archfiendStatModification.EffectiveOffset;
			if (num < 0)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("It should never be possible for {0} to have a sub-zero ritual slot count", playerState));
				}
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			int num2 = num;
			if (base.data.ClearRitualTable)
			{
				num2 = 0;
			}
			while (playerState.RitualState.SlottedItems.Count > num2)
			{
				List<Identifier> slottedItems = playerState.RitualState.SlottedItems;
				int index = slottedItems.Count - 1;
				Identifier identifier = slottedItems[index];
				GameItem gameItem = base._currentTurn.FetchGameItem(identifier);
				Artifact artifact = gameItem as Artifact;
				if (artifact == null)
				{
					ActiveRitual activeRitual = gameItem as ActiveRitual;
					if (activeRitual == null)
					{
						SimLogger logger2 = SimLogger.Logger;
						if (logger2 != null)
						{
							logger2.Error(string.Format("Something is on {0}'s ritual table that isn't a Ritual or Artifact", playerState));
						}
						return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
					}
					ItemBanishedEvent itemBanishedEvent = this.TurnProcessContext.BanishGameItem(activeRitual, this._player.Id);
					if (itemBanishedEvent == null)
					{
						SimLogger logger3 = SimLogger.Logger;
						if (logger3 != null)
						{
							logger3.Error(string.Format("Could not Banish ActiveRitual {0}", activeRitual));
						}
						return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
					}
					ritualCastEvent.AddChildEvent<ItemBanishedEvent>(itemBanishedEvent);
				}
				else
				{
					Problem problem2 = this.TurnProcessContext.MoveItemToVault(playerState, identifier) as Problem;
					if (problem2 != null)
					{
						SimLogger logger4 = SimLogger.Logger;
						if (logger4 != null)
						{
							logger4.Error(string.Format("Unable to send Artifact {0} to its room", artifact));
						}
						return problem2;
					}
					ritualCastEvent.AddChildEvent<AttachmentRemovedEvent>(new AttachmentRemovedEvent(this._player.Id, playerState.Id, artifact));
				}
			}
			ArchfiendModifierActiveRitual archfiendModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(archfiendModifierActiveRitual.Id);
			archfiendModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
