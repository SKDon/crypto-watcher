﻿using System;
using System.Collections.Generic;
using System.Linq;
using CryptoWatcher.Domain.Expressions;
using CryptoWatcher.Domain.Models;


namespace CryptoWatcher.Domain.Builders
{
    public static class ScriptVariablesBuilder
    {
        public static ScriptVariables BuildScriptVariables(List<DataPoint> lines)
        {
            var scriptVariables = new ScriptVariables(
                BuildTimes(lines),
                BuildCurrencies(lines),
                BuildIndicators(lines),
                BuildValues(lines)
            );


            // Return
            return scriptVariables;
        }

        public static DateTime[] BuildTimes(List<DataPoint> lines)
        {
            return lines.Select(x => x.Time).Distinct().ToArray();
        }

        public static string[] BuildCurrencies(List<DataPoint> lines)
        {
            return lines.Select(x => x.TargetId).Distinct().ToArray();
        }

        public static string[] BuildIndicators(List<DataPoint> lines)
        {
            return lines.Select(x => x.IndicatorId).Distinct().ToArray();
        }

        public static Dictionary<DateTime, Dictionary<string, Dictionary<string, decimal?>>> BuildValues(
            List<DataPoint> lines)
        {
            // Distinct
            var times = lines.Select(x => x.Time).Distinct().ToList();
            var targets = lines.Select(x => x.TargetId).Distinct().ToList();
            var indicators = lines.Select(x => x.IndicatorId).Distinct().ToList();

            // Loop
            var level1 = new Dictionary<DateTime, Dictionary<string, Dictionary<string, decimal?>>>();
            foreach (var time in times)
            {
                var level2 = new Dictionary<string, Dictionary<string, decimal?>>();
                foreach (var indicator in indicators)
                {
                    var level3 = new Dictionary<string, decimal?>();
                    foreach (var target in targets)
                    {
                        var line = lines.FirstOrDefault(LineExpression.Line(time, target, indicator).Compile());
                        level3.Add(target, line?.Value);
                    }
                    level2.Add(indicator, level3);
                }
                level1.Add(time, level2);
            }

            // Return
            return level1;
        }
    }
}
