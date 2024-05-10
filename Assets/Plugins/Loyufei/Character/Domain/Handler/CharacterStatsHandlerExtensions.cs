using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character 
{
    public static class CharacterStatsHandlerExtensions
    {
        public static void StatsCreate(this CharacterStatsHandler self, CharacterStatsCreate create)
        {
            var mark = create.Mark;

            self.StatsCreate(mark.CharacterID, mark.GuidHash);
        }

        public static void StatsRelease(this CharacterStatsHandler self, CharacterStatsRelease release)
        {
            self.StatsRelease(release.Mark.GuidHash);
        }

        public static void CalculateStatIncrease(this CharacterStatsHandler self, CalculateStatIncrease increase)
        {
            var guidHash = increase.Mark.GuidHash;
            var statName = increase.Variable.StatId;
            var variable = increase.Variable.Variable;
            var response = increase.OnResponse;

            response.Invoke(self.CalculateStatIncrease(guidHash, statName, variable));
        }

        public static void CalculateStatDecrease(this CharacterStatsHandler self, CalculateStatDecrease decrease)
        {
            var guidHash = decrease.Mark.GuidHash;
            var statName = decrease.Variable.StatId;
            var variable = decrease.Variable.Variable;
            var response = decrease.OnResponse;

            response.Invoke(self.CalculateStatDecrease(guidHash, statName, variable));
        }

        public static void StandardStatIncrease(this CharacterStatsHandler self, StandardStatIncrease increase)
        {
            var guidHash = increase.Mark.GuidHash;
            var statName = increase.Variable.StatId;
            var variable = increase.Variable.Variable;
            var response = increase.OnResponse;

            response.Invoke(self.StandardStatIncrease(guidHash, statName, variable));
        }

        public static void StandardStatDecrease(this CharacterStatsHandler self, StandardStatDecrease decrease)
        {
            var guidHash = decrease.Mark.GuidHash;
            var statName = decrease.Variable.StatId;
            var variable = decrease.Variable.Variable;
            var response = decrease.OnResponse;

            response.Invoke(self.StandardStatDecrease(guidHash, statName, variable));
        }

        public static void StatFetch(this CharacterStatsHandler self, CharacterStatFetch statFetch) 
        {
            var guidHash = statFetch.Mark.GuidHash;
            var statName = statFetch.Variable.StatId;
            var response = statFetch.OnResponse;

            response.Invoke(self.StatFetch(guidHash, statName));
        }
    }
}