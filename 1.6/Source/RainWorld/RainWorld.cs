using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RainWorld
{
    [StaticConstructorOnStartup]
    public class RainWorldMod : Mod
    {
        public RainWorldMod(ModContentPack content) : base(content)
        {
            var h = new Harmony("ocarina.rainworld");
            h.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("RAINWORLD: startup succesful!");
        }
    }
}
