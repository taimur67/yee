using System;
using Game.StaticData;
using LoG.Core.Injection;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace LoG
{
	// Token: 0x020006A9 RID: 1705
	public class RelicDescriptionUI : MonoBehaviour
	{
		// Token: 0x06002E14 RID: 11796 RVA: 0x000BF872 File Offset: 0x000BDA72
		public void SetKnown(bool known)
		{
			this._knownRelicGrp.SetActive(known);
			this._unknownRelicGrp.SetActive(!known);
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000BF88F File Offset: 0x000BDA8F
		public void Setup(GameItemClientDataComponent clientData, ModifierStaticData modifier, Gender gender)
		{
			this.Setup(clientData.Name, clientData.Lore, clientData.Description, modifier, gender);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000BF8AC File Offset: 0x000BDAAC
		public void Setup(LocalizedString relicName, LocalizedString lore, LocalizedString description, ModifierStaticData modifier, Gender gender)
		{
			this._heading.StringReference = relicName;
			LocalizedString localizedStringWithModifiers = LocalizationKeys.Modifiers.GetLocalizedStringWithModifiers(description, modifier, gender, this._gameStateService.Rules ?? new GameRules());
			this._description.StringReference = localizedStringWithModifiers;
		}

		// Token: 0x0400267E RID: 9854
		[InjectField]
		private GameStateService _gameStateService;

		// Token: 0x0400267F RID: 9855
		[SerializeField]
		private GameObject _knownRelicGrp;

		// Token: 0x04002680 RID: 9856
		[SerializeField]
		private GameObject _unknownRelicGrp;

		// Token: 0x04002681 RID: 9857
		[SerializeField]
		private LocalizeStringEvent _heading;

		// Token: 0x04002682 RID: 9858
		[SerializeField]
		private LocalizeStringEvent _description;
	}
}
