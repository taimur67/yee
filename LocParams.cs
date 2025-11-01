using System;

namespace LoG
{
	// Token: 0x020001F4 RID: 500
	public static class LocParams
	{
		// Token: 0x020008AB RID: 2219
		public static class Hex
		{
			// Token: 0x040012FF RID: 4863
			public const string Coord = "canton_coordinates";

			// Token: 0x04001300 RID: 4864
			public const string TerrainType = "terrain_type";
		}

		// Token: 0x020008AC RID: 2220
		public static class Diplomacy
		{
			// Token: 0x04001301 RID: 4865
			public const string ScapegoatName = "scapegoat_name";

			// Token: 0x04001302 RID: 4866
			public const string TrueSourceName = "true_source_name";

			// Token: 0x04001303 RID: 4867
			public const string Armistice = "armistice";

			// Token: 0x04001304 RID: 4868
			public const string LiegeName = "liege_name";

			// Token: 0x04001305 RID: 4869
			public const string VassalName = "vassal_name";

			// Token: 0x04001306 RID: 4870
			public const string IsSelfVassal = "vassal_self";

			// Token: 0x04001307 RID: 4871
			public const string IsSelfLiege = "liege_self";

			// Token: 0x04001308 RID: 4872
			public const string IsSelfScapegoat = "scapegoat_self";

			// Token: 0x04001309 RID: 4873
			public const string StateName = "state_name";

			// Token: 0x0400130A RID: 4874
			public const string PendingType = "pending_type";

			// Token: 0x0400130B RID: 4875
			public const string IsSelfExcommunicated = "excommunicated_self";
		}

		// Token: 0x020008AD RID: 2221
		public static class Events
		{
			// Token: 0x0400130C RID: 4876
			public const string OngoingDescription = "ongoing_description";

			// Token: 0x0400130D RID: 4877
			public const string OngoingSubheading = "ongoing_subheading";

			// Token: 0x0400130E RID: 4878
			public const string EndedDescription = "ended_description";

			// Token: 0x0400130F RID: 4879
			public const string EndedSubheading = "ended_subheading";
		}

		// Token: 0x020008AE RID: 2222
		public static class GameItem
		{
			// Token: 0x04001310 RID: 4880
			public const string Name = "gameitem_name";

			// Token: 0x04001311 RID: 4881
			public const string TargetName = "target_name";

			// Token: 0x04001312 RID: 4882
			public const string Category = "gameitem_category";

			// Token: 0x04001313 RID: 4883
			public const string Manuscript = "manuscript";

			// Token: 0x04001314 RID: 4884
			public const string StatName = "attribute_name";
		}

		// Token: 0x020008AF RID: 2223
		public static class GamePiece
		{
			// Token: 0x04001315 RID: 4885
			public const string Name = "gamepiece_name";

			// Token: 0x04001316 RID: 4886
			public const string Category = "gamepiece_category";

			// Token: 0x04001317 RID: 4887
			public const string Blocker = "blocker";
		}

		// Token: 0x020008B0 RID: 2224
		public static class Order
		{
			// Token: 0x04001318 RID: 4888
			public const string Type = "order_type";

			// Token: 0x04001319 RID: 4889
			public const string ConflictingType = "conflicting_order";
		}

		// Token: 0x020008B1 RID: 2225
		public static class Battle
		{
			// Token: 0x0400131A RID: 4890
			public const string Ability = "ability";

			// Token: 0x0400131B RID: 4891
			public const string Attacker = "attacker";

			// Token: 0x0400131C RID: 4892
			public const string Defender = "defender";

			// Token: 0x0400131D RID: 4893
			public const string Winner = "winner";

			// Token: 0x0400131E RID: 4894
			public const string Loser = "loser";

			// Token: 0x0400131F RID: 4895
			public const string RecoveredItems = "recovered_items";

			// Token: 0x04001320 RID: 4896
			public const string AttackerPlayer = "attacker_player";

			// Token: 0x04001321 RID: 4897
			public const string DefenderPlayer = "defender_player";

			// Token: 0x04001322 RID: 4898
			public const string Phase = "phase";
		}

		// Token: 0x020008B2 RID: 2226
		public static class Objective
		{
			// Token: 0x04001323 RID: 4899
			public const string Conditions = "objective_conditions";

			// Token: 0x04001324 RID: 4900
			public const string TargetValue = "target_value";

			// Token: 0x04001325 RID: 4901
			public const string Count = "objective_count";

			// Token: 0x04001326 RID: 4902
			public const string Target = "objective_target";
		}

		// Token: 0x020008B3 RID: 2227
		public static class Manuscript
		{
			// Token: 0x04001327 RID: 4903
			public const string Category = "manuscript_category";
		}

		// Token: 0x020008B4 RID: 2228
		public static class Archfiend
		{
			// Token: 0x04001328 RID: 4904
			public const string IsSelf = "archfiend_self";

			// Token: 0x04001329 RID: 4905
			public const string Name = "archfiend_name";

			// Token: 0x0400132A RID: 4906
			public const string Rank = "rank";

			// Token: 0x0400132B RID: 4907
			public const string TriggeringName = "source_name";

			// Token: 0x0400132C RID: 4908
			public const string AffectedName = "affected_name";

			// Token: 0x0400132D RID: 4909
			public const string AffectedNames = "affected_names";

			// Token: 0x0400132E RID: 4910
			public const string StatName = "attribute_name";
		}

		// Token: 0x020008B5 RID: 2229
		public static class Player
		{
			// Token: 0x0400132F RID: 4911
			public const string DisplayName = "display_name";

			// Token: 0x04001330 RID: 4912
			public const string SourceDisplayName = "source_name_display_name";
		}

		// Token: 0x020008B6 RID: 2230
		public static class PraetorDuel
		{
			// Token: 0x04001331 RID: 4913
			public const string DuelOutcomes = "duel_outcomes";

			// Token: 0x04001332 RID: 4914
			public const string Technique = "technique";

			// Token: 0x04001333 RID: 4915
			public const string TechniqueStyle = "style";
		}

		// Token: 0x020008B7 RID: 2231
		public static class Powers
		{
			// Token: 0x04001334 RID: 4916
			public const string Name = "power_name";
		}

		// Token: 0x020008B8 RID: 2232
		public static class Ritual
		{
			// Token: 0x04001335 RID: 4917
			public const string Name = "ritual_name";

			// Token: 0x04001336 RID: 4918
			public const string MaskingInfo = "ritual_masking";

			// Token: 0x04001337 RID: 4919
			public const string Identification = "ritual_id";

			// Token: 0x04001338 RID: 4920
			public const string FramedPlayerName = "ritual_framed";

			// Token: 0x04001339 RID: 4921
			public const string RevealedInformation = "revealed_list";
		}

		// Token: 0x020008B9 RID: 2233
		public static class Edict
		{
			// Token: 0x0400133A RID: 4922
			public const string Effect = "effect";

			// Token: 0x0400133B RID: 4923
			public const string VoteResults = "vote_results";

			// Token: 0x0400133C RID: 4924
			public const string OptionNames = "options_name";

			// Token: 0x0400133D RID: 4925
			public const string OptionDescriptions = "options_description";
		}

		// Token: 0x020008BA RID: 2234
		public static class Data
		{
			// Token: 0x0400133E RID: 4926
			public const string Name = "data_name";

			// Token: 0x0400133F RID: 4927
			public const string Description = "data_description";

			// Token: 0x04001340 RID: 4928
			public const string UnlockDescription = "data_unlock";
		}

		// Token: 0x020008BB RID: 2235
		public static class Generic
		{
			// Token: 0x04001341 RID: 4929
			public const string Prestige = "prestige";

			// Token: 0x04001342 RID: 4930
			public const string Cost = "cost";

			// Token: 0x04001343 RID: 4931
			public const string Turns = "turns";

			// Token: 0x04001344 RID: 4932
			public const string TotalTurns = "total_turns";

			// Token: 0x04001345 RID: 4933
			public const string Turn = "turn";

			// Token: 0x04001346 RID: 4934
			public const string TurnValue = "turn_value";

			// Token: 0x04001347 RID: 4935
			public const string Value = "value";

			// Token: 0x04001348 RID: 4936
			public const string ValueBis = "value_2";

			// Token: 0x04001349 RID: 4937
			public const string MinValue = "min_value";

			// Token: 0x0400134A RID: 4938
			public const string MaxValue = "max_value";

			// Token: 0x0400134B RID: 4939
			public const string MinValueBis = "min_value_2";

			// Token: 0x0400134C RID: 4940
			public const string MaxValueBis = "max_value_2";

			// Token: 0x0400134D RID: 4941
			public const string Active = "active";

			// Token: 0x0400134E RID: 4942
			public const string Duration = "duration";

			// Token: 0x0400134F RID: 4943
			public const string Target = "target";

			// Token: 0x04001350 RID: 4944
			public const string Distance = "distance";

			// Token: 0x04001351 RID: 4945
			public const string TurnPhase = "game_state";

			// Token: 0x04001352 RID: 4946
			public const string Resource = "resource_type";
		}

		// Token: 0x020008BC RID: 2236
		public static class Result
		{
			// Token: 0x04001353 RID: 4947
			public const string Description = "result_description";
		}
	}
}
