﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESAPIX.Bootstrapper.Helpers
{
    public class ArgumentParser
    {
        public static string GetPatientId(string[] commandLineArgs)
        {
            return GetSingle(commandLineArgs, ArgumentKey.PatientId);
        }

        public static string GetPlanSetup(string[] commandLineArgs)
        {
            return GetSingle(commandLineArgs, ArgumentKey.PlanSetup);
        }

        public static string GetBrachyPlanSetup(string[] commandLineArgs)
        {
            return GetSingle(commandLineArgs, ArgumentKey.BrachyPlanSetup);
        }

        public static string GetExternalPlanSetup(string[] commandLineArgs)
        {
            return GetSingle(commandLineArgs, ArgumentKey.ExternalPlanSetup);
        }

        public static (string,string) GetImage(string[] commandLineArgs)
        {
            var multiple = GetMultiple(commandLineArgs, ArgumentKey.Image);
            if(multiple!=null && multiple.Count() == 2)
            {
                return (multiple[0], multiple[1]);
            }
            return (string.Empty,string.Empty);
        }

        public static string GetCourseId(string[] commandLineArgs)
        {
            return GetSingle(commandLineArgs, ArgumentKey.Course);
        }

        public static List<string> GetPlansInScope(string[] commandLineArgs)
        {
            return GetMultiple(commandLineArgs, ArgumentKey.PlansInScope)?.ToList();
        }

        public static List<string> GetPlanSumsInScope(string[] commandLineArgs)
        {
            return GetMultiple(commandLineArgs, ArgumentKey.PlanSumsInScope)?.ToList();
        }

        public static List<string> GetExternalPlansInScope(string[] commandLineArgs)
        {
            return GetMultiple(commandLineArgs, ArgumentKey.ExternalPlansInScope)?.ToList();
        }

        public static List<string> GetBrachyPlansInScope(string[] commandLineArgs)
        {
            return GetMultiple(commandLineArgs, ArgumentKey.BrachyPlansInScope)?.ToList();
        }

        public static string GetSingle(string[] commandLineArgs, string key)
        {
            if (commandLineArgs.Contains(key))
            {
                var index = commandLineArgs.ToList().IndexOf(key);
                var parts = commandLineArgs.Skip(index+1).TakeWhile(k => !k.StartsWith("-")).ToArray();
                return string.Join(" ", parts);
            }
            return null;
        }

        public static string[] GetMultiple(string[] commandLineArgs, string key)
        {
            if (commandLineArgs.Contains(key))
            {
                var index = commandLineArgs.ToList().IndexOf(key);
                return commandLineArgs.Skip(index+1).TakeWhile(k => !k.StartsWith("-")).ToArray();
            }
            return null;
        }
    }
}
