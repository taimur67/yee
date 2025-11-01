using System;
using System.Collections.Generic;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200046E RID: 1134
	[Serializable]
	public abstract class SchemeGenerator : StaticDataEntity
	{
		// Token: 0x06001525 RID: 5413 RVA: 0x00050059 File Offset: 0x0004E259
		public IEnumerable<SchemeObjective> GenerateSchemes(TurnContext context, PlayerState player)
		{
			TurnState turn = context.CurrentTurn;
			foreach (SchemeObjective schemeObjective in this.GenerateSchemesInternal(context, player))
			{
				schemeObjective.Id = (SchemeId)turn.NextIdentifier();
				schemeObjective.SourceId = base.Id;
				this.SetSchemeParams(schemeObjective, context, player);
				yield return schemeObjective;
			}
			IEnumerator<SchemeObjective> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001526 RID: 5414
		protected abstract IEnumerable<SchemeObjective> GenerateSchemesInternal(TurnContext context, PlayerState player);

		// Token: 0x06001527 RID: 5415 RVA: 0x00050077 File Offset: 0x0004E277
		protected virtual void SetSchemeParams(SchemeObjective scheme, TurnContext context, PlayerState player)
		{
		}
	}
}
