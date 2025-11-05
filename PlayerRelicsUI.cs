using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Core.StaticData;
using Game.Client.Util;
using Game.Simulation.StaticData;
using Game.StaticData;
using UnityEngine;
using Zenject;

namespace LoG
{
	// Token: 0x0200069F RID: 1695
	public class PlayerRelicsUI : MonoBehaviour, IInitialize
	{
		// Token: 0x06002DF1 RID: 11761 RVA: 0x000BEFA4 File Offset: 0x000BD1A4
		public void Initialize()
		{
			if (this._turn == null || this._currentPlayer == null || this._targetPlayer == null)
			{
				return;
			}
			List<ConfigRef> list = IEnumerableExtensions.ToList<ConfigRef>(from x in IEnumerableExtensions.ToList<ValueTuple<Relic, RelicStaticData>>(this._turn.GetRelicDataPairs(this._database, this._targetPlayer))
			select x.Item2.ConfigRef);
			ArchfiendClientDataComponent archfiendClientDataComponent = this._clientDataAccessor.ArchfiendClientDataComponent(this._targetPlayer.ArchfiendId);
			ArchfiendUIContext kingmakerTargetCtx = new ArchfiendUIContext();
			if (this._targetPlayer.IsKingmaker && this._targetPlayer.KingmakerPuppetId != -2147483648)
			{
				PlayerState playerState = this._gameStateService.PlayerView.FindPlayerState(this._targetPlayer.KingmakerPuppetId, null);
				ArchfiendClientDataComponent archfiendClientDataComponent2 = this._clientDataAccessor.ArchfiendClientDataComponent(playerState.ArchfiendId);
				kingmakerTargetCtx = new ArchfiendUIContext
				{
					ArchfiendName = archfiendClientDataComponent2.Name,
					ArchfiendColor = archfiendClientDataComponent2.GetArchfiendColor(),
					ArchfiendSprite = archfiendClientDataComponent2.TurnPlaybackPortrait,
					PlayerId = playerState.Id,
					SigilSprite = archfiendClientDataComponent2.Sigil
				};
			}
			this._relicsLayout.SetRelics(list, archfiendClientDataComponent.Gender, kingmakerTargetCtx);
			int maxRelicsValue = this._database.FetchSingle<RelicsEconomyData>().MaxRelicsValue;
			int num = 0;
			for (int i = 0; i < this._relicDescriptions.Length; i++)
			{
				RelicDescriptionUI relicDescriptionUI = this._relicDescriptions[i];
				if (i >= list.Count)
				{
					if (num >= maxRelicsValue)
					{
						relicDescriptionUI.SetObjectActive(false);
					}
					else
					{
						relicDescriptionUI.SetObjectActive(true);
						relicDescriptionUI.SetKnown(false);
					}
				}
				else
				{
					Pair<RelicStaticData, GameItemClientDataComponent> relicInfo = list[i].GetRelicInfo(this._clientDataAccessor, this._database);
					RelicStaticData first = relicInfo.First;
					ModifierStaticData modifierStaticData = (first != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(first.GetModifiers()) : null;
					if (modifierStaticData == null)
					{
						StaticDataEntity staticDataEntity = this._database.Fetch<StaticDataEntity>((first != null) ? IEnumerableExtensions.FirstOrDefault<string>(IEnumerableExtensions.ToList<string>(first.ProvidedAbilities)) : null);
						modifierStaticData = ((staticDataEntity != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(staticDataEntity.GetModifiers()) : null);
					}
					relicDescriptionUI.SetKnown(true);
					relicDescriptionUI.Setup(relicInfo.Second, modifierStaticData, archfiendClientDataComponent.Gender);
					num += ((first != null) ? first.RelicValue : 0);
				}
			}
		}

		// Token: 0x0400263B RID: 9787
		[Inject]
		private SI2UIManager _uiManager;

		// Token: 0x0400263C RID: 9788
		[Inject]
		private GameDatabase _database;

		// Token: 0x0400263D RID: 9789
		[Inject]
		private GameStateService _gameStateService;

		// Token: 0x0400263E RID: 9790
		[Inject]
		private ClientDataAccessor _clientDataAccessor;

		// Token: 0x0400263F RID: 9791
		[InjectOptional]
		private TurnState _turn;

		// Token: 0x04002640 RID: 9792
		[InjectOptional(Id = "CurrentPlayer")]
		private PlayerState _currentPlayer;

		// Token: 0x04002641 RID: 9793
		[InjectOptional(Id = "TargetPlayer")]
		private PlayerState _targetPlayer;

		// Token: 0x04002642 RID: 9794
		[SerializeField]
		private RelicSelectionUIController _relicsLayout;

		// Token: 0x04002643 RID: 9795
		[SerializeField]
		private RelicDescriptionUI[] _relicDescriptions;
	}
}
