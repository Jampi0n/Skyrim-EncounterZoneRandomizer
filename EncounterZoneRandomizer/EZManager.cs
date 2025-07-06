using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BGSEZ = Mutagen.Bethesda.Plugins.IFormLinkGetter<Mutagen.Bethesda.Skyrim.IEncounterZoneGetter>;
using System.Diagnostics;
using Mutagen.Bethesda;
using System.Threading.Tasks.Dataflow;

namespace EncounterZoneRandomizer {
    public class EZ {
        internal List<EnemyTypeSettings> enemyTypes = new();
        internal List<ModifierSettings> modifiers = new();
        internal BGSEZ? sameAs = null;

        internal BGSEZ bgsEZ;
        internal EZ(BGSEZ bgsEZ) {
            this.bgsEZ = bgsEZ;
            EZManager.allEZs.Add(this);
        }
    }


    public static class EZManager {

        internal static HashSet<EZ> allEZs = new();
        internal static HashSet<EZ> dependentEZs = new();
        internal static Dictionary<FormKey, EZ> EZmap = new();

        public static int MaxLevel(int a, int b) {
            if(a == 0 || b == 0) {
                return 0;
            }
            return Math.Max(a, b);
        }

        public static sbyte ToSByte(int a) {
            if(a <= 1) {
                return 1;
            }
            if(a >= 127) {
                return 127;
            }
            return (sbyte)a;
        }

        public static void PatchAll() {
            foreach(var ez in allEZs) {
                if(ez.sameAs == null) {
                    int lowMin = int.MinValue;
                    int highMin = int.MinValue;
                    int lowMax = int.MinValue;
                    int highMax = int.MinValue;
                    foreach(var enemyType in ez.enemyTypes) {
                        lowMin = Math.Max(lowMin, enemyType.lowMin);
                        highMin = Math.Max(highMin, enemyType.highMin);
                        lowMax = MaxLevel(lowMax, enemyType.lowMax);
                        highMax = MaxLevel(highMax, enemyType.highMax);
                    }
                    foreach(var modifier in ez.modifiers) {
                        lowMin += modifier.lowMin;
                        highMin += modifier.highMin;
                        lowMax = MaxLevel(lowMax, lowMax + modifier.lowMax);
                        highMax = MaxLevel(highMax, highMax + modifier.highMax);
                    }

                    Random rnd = new Random();
                    var bgsEZ = Program.State.PatchMod.EncounterZones.GetOrAddAsOverride(ez.bgsEZ.Resolve(Program.State.LinkCache));

                    string printString = bgsEZ.EditorID + " (" + lowMin + " to " + highMin + ") - (" + lowMax + " to " + highMax + ")";

                    if(highMin < lowMin) {
                        highMin = lowMin;
                    }
                    if(lowMax == 0) {
                        highMax = 0;
                    }
                    if(highMax < lowMax) {
                        highMax = lowMax;
                    }

                    if(lowMin <= highMin) {
                        bgsEZ.MinLevel = ToSByte(rnd.Next(lowMin, highMin + 1));
                    } else {
                        throw new Exception("Invalid combination of lowMin and highMin: " + lowMin + ", " + highMin);
                    }
                    if(lowMax == 0 && highMax == 0) {
                        bgsEZ.MaxLevel = 0;
                    } else if(highMax == 0) {
                        var tmp = rnd.Next(lowMax, 130);
                        if(tmp >= 129) {
                            bgsEZ.MaxLevel = 0;
                        } else {
                            bgsEZ.MaxLevel = ToSByte(tmp);
                        }
                    } else if(lowMax == 0) {
                        throw new Exception("A maximum level of 0 means no maximum level. Cannot have the lower bound as 'no maximum level' without also having the upper bound as 'nomaximum level'. ");
                    } else if(lowMax <= highMax) {
                        bgsEZ.MaxLevel = ToSByte(rnd.Next(lowMax, highMax + 1));
                    } else {
                        throw new Exception("Invalid combination of lowMax and highMax: " + lowMax + ", " + highMax);
                    }

                    printString += " => " + bgsEZ.MinLevel + " - " + bgsEZ.MaxLevel;
                    Console.WriteLine(printString);

                }
            }
            foreach(var ez in dependentEZs) {
                if(ez.sameAs != null) {
                    var bgsEZ = Program.State.PatchMod.EncounterZones.GetOrAddAsOverride(ez.bgsEZ.Resolve(Program.State.LinkCache));
                    var sameAsBgsEZ = ez.sameAs.Resolve(Program.State.LinkCache);
                    bgsEZ.MinLevel = sameAsBgsEZ.MinLevel;
                    bgsEZ.MaxLevel = sameAsBgsEZ.MaxLevel;
                }
            }
        }
        public static EZ NewEZ(string editorID) {
            var bgsEZ = Program.State.PatchMod.EncounterZones.AddNew();
            bgsEZ.EditorID = editorID;
            var newEZ = new EZ(bgsEZ.ToLink());
            EZmap.Add(bgsEZ.FormKey, newEZ);
            return newEZ;
        }

        public static EZ ToEZ(this BGSEZ ez) {
            if(!EZmap.ContainsKey(ez.FormKey)) {
                EZmap[ez.FormKey] = new EZ(ez);
            }
            return EZmap[ez.FormKey];
        }

        public static EZ Draugr(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Draugr);
            return ez;
        }

        public static EZ Dragon(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Dragon);
            return ez;
        }

        public static EZ DragonPriest(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.DragonPriest);
            return ez;
        }

        public static EZ Forsworn(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Forsworn);
            return ez;
        }

        public static EZ Hagraven(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Hagraven);
            return ez;
        }

        public static EZ Bandit(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Bandit);
            return ez;
        }

        public static EZ Dwemer(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Dwemer);
            return ez;
        }

        public static EZ Falmer(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Falmer);
            return ez;
        }

        public static EZ Vampire(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Vampire);
            return ez;
        }

        public static EZ Warlock(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Warlock);
            return ez;
        }

        public static EZ Daedra(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Daedra);
            return ez;
        }

        public static EZ Animals(this EZ ez) {
            ez.enemyTypes.Add(Program.settings.Animals);
            return ez;
        }



        public static EZ Dawnguard(this EZ ez) {
            ez.modifiers.Add(Program.settings.Dawnguard);
            return ez;
        }

        public static EZ Dragonborn(this EZ ez) {
            ez.modifiers.Add(Program.settings.Dragonborn);
            return ez;
        }

        public static EZ Boss(this EZ ez) {
            ez.modifiers.Add(Program.settings.Boss);
            return ez;
        }

        public static EZ MinorBoss(this EZ ez) {
            ez.modifiers.Add(Program.settings.MinorBoss);
            return ez;
        }

        public static EZ MajorBoss(this EZ ez) {
            ez.modifiers.Add(Program.settings.MajorBoss);
            return ez;
        }

        public static EZ Epic(this EZ ez) {
            ez.modifiers.Add(Program.settings.Epic);
            return ez;
        }

        public static EZ QuestDaedric(this EZ ez) {
            ez.modifiers.Add(Program.settings.QuestDaedric);
            return ez;
        }

        public static EZ SameAs(this EZ ez, BGSEZ bGSEZ) {
            ez.sameAs = bGSEZ;
            dependentEZs.Add(ez);
            return ez;
        }
    }
}
