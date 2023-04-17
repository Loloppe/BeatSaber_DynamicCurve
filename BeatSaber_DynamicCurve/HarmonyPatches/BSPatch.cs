using BeatSaber_DynamicCurve;
using BeatSaber_DynamicCurve.Algorithm;
using HarmonyLib;

namespace BeatmapScanner.HarmonyPatches
{
	[HarmonyPatch(typeof(StandardLevelDetailView), nameof(StandardLevelDetailView.RefreshContent))]
	public static class BSPatch
	{
		static void Postfix(IDifficultyBeatmap ____selectedDifficultyBeatmap, IBeatmapLevel ____level)
		{
			if (____selectedDifficultyBeatmap is CustomDifficultyBeatmap beatmap && beatmap.beatmapSaveData.colorNotes.Count > 0 && beatmap.level.beatsPerMinute > 0)
			{
				Plugin.Log.Info(____level.songName + " " + ____selectedDifficultyBeatmap.difficulty);
				Curve.Curves(beatmap.beatmapSaveData.colorNotes, beatmap.beatmapSaveData.bombNotes, beatmap.level.beatsPerMinute);
			}
		}
	}
}
