using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000112 RID: 274
	public class ActionEquipPraetor : ActionOrderGOAPNode<OrderAttachGameItemToGamePiece>
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00014B8F File Offset: 0x00012D8F
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00014B92 File Offset: 0x00012D92
		public override ActionID ID
		{
			get
			{
				return ActionID.Attach_Praetor_Legion;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00014B95 File Offset: 0x00012D95
		public override string ActionName
		{
			get
			{
				return string.Format("Equip Praetor {0} to {1} {2}", this.Praetor, this.TargetGamePiece, this.ActionCondition);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00014BB3 File Offset: 0x00012DB3
		private string ActionCondition
		{
			get
			{
				if (!this._equipAfterPurchasing)
				{
					return string.Empty;
				}
				return "after purchasing.";
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public ActionEquipPraetor(GamePiece targetGamePiece, Praetor praetor, bool equipAfterPurchasing = false, EquipPraetorMode equipPraetorMode = EquipPraetorMode.Legion, Cost resourcesGenerated = null)
		{
			this.TargetGamePiece = targetGamePiece;
			this.Praetor = praetor;
			this.EquipPraetorMode = equipPraetorMode;
			this.TributeGeneratedByPraetor = resourcesGenerated;
			this._equipAfterPurchasing = equipAfterPurchasing;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00014BFC File Offset: 0x00012DFC
		public override void Prepare()
		{
			base.AddConstraint(new WPGamePieceHasFreeSlot(this.TargetGamePiece, FreeSlotsMode.Any));
			base.AddConstraint(new WPGamePieceHasPraetor(this.TargetGamePiece, false));
			base.AddConstraint(new WPGamePieceActive(this.TargetGamePiece));
			base.AddPrecondition(new WPIdlePraetor(this.Praetor));
			base.AddEffect(new WPGamePieceHasPraetor(this.TargetGamePiece, true));
			if (this.EquipPraetorMode == EquipPraetorMode.Legion)
			{
				if (this.Praetor != null && this.TargetGamePiece != null)
				{
					PraetorStaticData praetorStaticData = base.GameDatabase.Fetch<PraetorStaticData>(this.Praetor.StaticDataId);
					if (praetorStaticData != null)
					{
						CombatStats bonus = praetorStaticData.Components.OfType<GamePieceModifierStaticData>().CalculatePowerChange(this.TargetGamePiece);
						base.AddEffect(WPCombatAdvantage.BonusFor(this.TargetGamePiece, bonus));
					}
					else
					{
						base.Disable("No praetor found");
					}
				}
			}
			else if (this.EquipPraetorMode == EquipPraetorMode.PoP)
			{
				base.AddEffect(new WPTribute(this.TributeGeneratedByPraetor * this.TributeEarningsMultiplier));
				int num = this.TributeGeneratedByPraetor[ResourceTypes.Prestige];
				if (num > 0)
				{
					base.AddEffect(new WPPrestigeProduction(num));
				}
				base.AddEffect(new WPTributeBoost());
			}
			base.Prepare();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00014D3C File Offset: 0x00012F3C
		protected override OrderAttachGameItemToGamePiece GenerateOrder()
		{
			GameItem gameItem = this.OwningPlanner.AIPreviewTurn.FetchGameItem<GameItem>(this.Praetor);
			return new OrderAttachGameItemToGamePiece(this.TargetGamePiece, gameItem, GameItemCategory.None);
		}

		// Token: 0x04000274 RID: 628
		public EquipPraetorMode EquipPraetorMode;

		// Token: 0x04000275 RID: 629
		public readonly GamePiece TargetGamePiece;

		// Token: 0x04000276 RID: 630
		public readonly Praetor Praetor;

		// Token: 0x04000277 RID: 631
		public Cost TributeGeneratedByPraetor;

		// Token: 0x04000278 RID: 632
		public int TributeEarningsMultiplier = 3;

		// Token: 0x04000279 RID: 633
		private bool _equipAfterPurchasing;
	}
}
