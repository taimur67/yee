using System;
using Core.StaticData;
using Game.StaticData;
using LoG.Core.UIManager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

namespace LoG
{
	// Token: 0x020004AD RID: 1197
	public class RelicUIController : UIController
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0009556F File Offset: 0x0009376F
		public DynamicContextTooltipTargetUI Tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x00095577 File Offset: 0x00093777
		public ConfigRef RelicConfigRef
		{
			get
			{
				return this._relicConfigRef;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x0009557F File Offset: 0x0009377F
		private SpriteLoader SpriteLoader
		{
			get
			{
				if (this._spriteLoader == null)
				{
					this._spriteLoader = base.gameObject.AddComponent<SpriteLoader>();
				}
				return this._spriteLoader;
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000955A6 File Offset: 0x000937A6
		private void Awake()
		{
			this.SpriteLoader.AssetChanged += this.SpriteChanged;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000955BF File Offset: 0x000937BF
		private void OnDestroy()
		{
			this.SpriteLoader.AssetChanged -= this.SpriteChanged;
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000955D8 File Offset: 0x000937D8
		private void SpriteChanged(Sprite sprite)
		{
			this._icon.overrideSprite = sprite;
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000955E8 File Offset: 0x000937E8
		public void Setup(GameItemClientDataComponent clientData, ModifierStaticData modifier, ConfigRef configRef, Gender gender, ArchfiendUIContext kingmakerTargetCtx = null)
		{
			this.Setup(clientData.IconRef, clientData.Name, clientData.Lore, clientData.Description, modifier, configRef, gender, kingmakerTargetCtx);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x00095619 File Offset: 0x00093819
		public void Setup(AssetReferenceSprite icon, LocalizedString relicName, LocalizedString lore, LocalizedString description, ModifierStaticData modifier, ConfigRef relicStaticData, Gender gender, ArchfiendUIContext kingmakerTargetCtx)
		{
			this.SpriteLoader.SetReference(icon);
			this._relicConfigRef = relicStaticData;
			this.SetSelected(false);
			this.SetupTooltip(icon, relicName, lore, description, modifier, gender, kingmakerTargetCtx);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x00095647 File Offset: 0x00093847
		public void SetSelected(bool isSelected)
		{
			if (this._selectedOverlay != null)
			{
				this._selectedOverlay.gameObject.SetActive(isSelected);
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00095668 File Offset: 0x00093868
		private void SetupTooltip(AssetReferenceSprite icon, LocalizedString relicName, LocalizedString lore, LocalizedString description, ModifierStaticData modifier, Gender gender, ArchfiendUIContext kingmakerTargetCtx)
		{
			HeadingUIContext headingUIContext = new HeadingUIContext
			{
				HeadingIconRef = icon,
				HeadingString = relicName
			};
			LocalizedString localizedStringWithModifiers = LocalizationKeys.Modifiers.GetLocalizedStringWithModifiers(description, modifier, gender, this._gameRules ?? new GameRules());
			DescriptionUIContext descriptionUIContext = new DescriptionUIContext
			{
				LoreDescription = lore,
				ShortDescription = localizedStringWithModifiers,
				LongDescription = localizedStringWithModifiers
			};
			DiContainer diContainer = new DiContainer(this._injector);
			diContainer.Bind<HeadingUIContext>().FromInstance(headingUIContext);
			diContainer.Bind<DescriptionUIContext>().FromInstance(descriptionUIContext);
			diContainer.Bind<ArchfiendUIContext>().FromInstance(kingmakerTargetCtx);
			diContainer.Inject(this._tooltip);
		}

		// Token: 0x04001BE6 RID: 7142
		[InjectOptional]
		private DiContainer _injector;

		// Token: 0x04001BE7 RID: 7143
		[InjectOptional]
		private GameRules _gameRules;

		// Token: 0x04001BE8 RID: 7144
		public RelicType Type;

		// Token: 0x04001BE9 RID: 7145
		private ConfigRef _relicConfigRef;

		// Token: 0x04001BEA RID: 7146
		[SerializeField]
		private Image _selectedOverlay;

		// Token: 0x04001BEB RID: 7147
		[SerializeField]
		private Image _icon;

		// Token: 0x04001BEC RID: 7148
		[SerializeField]
		private DynamicContextTooltipTargetUI _tooltip;

		// Token: 0x04001BED RID: 7149
		private SpriteLoader _spriteLoader;
	}
}
