using Mutagen.Bethesda.Synthesis.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterZoneRandomizer {
    public class EnemyTypeSettings {
        [SynthesisTooltip("Lower bound when selecting the minimum level of the encounter zone. If multiple Enemy Types apply, the highest is chosen.")]
        public byte lowMin;
        [SynthesisTooltip("Upper bound when selecting the minimum level of the encounter zone. If multiple Enemy Types apply, the highest is chosen.")]
        public byte highMin;
        [SynthesisTooltip("Lower bound when selecting the maximum level of the encounter zone. If multiple Enemy Types apply, the highest is chosen.")]
        public byte lowMax;
        [SynthesisTooltip("Lower bound when selecting the maximum level of the encounter zone. If multiple Enemy Types apply, the highest is chosen.")]
        public byte highMax;

        public EnemyTypeSettings(byte lowMin, byte highMin, byte lowMax, byte highMax) {
            this.lowMin = lowMin;
            this.highMin = highMin;
            this.lowMax = lowMax;
            this.highMax = highMax;
        }
    }

    public class ModifierSettings {
        [SynthesisTooltip("Modifies the lower bound for the minimum level of the counter zone.")]
        public byte lowMin;
        [SynthesisTooltip("Modifies the upper bound for the minimum level of the counter zone.")]
        public byte highMin;
        [SynthesisTooltip("Modifies the lower bound for the maximum level of the counter zone.")]
        public byte lowMax;
        [SynthesisTooltip("Modifies the upper bound for the maximum level of the counter zone.")]
        public byte highMax;

        public ModifierSettings(byte lowMin, byte highMin, byte lowMax, byte highMax) {
            this.lowMin = lowMin;
            this.highMin = highMin;
            this.lowMax = lowMax;
            this.highMax = highMax;
        }
    }

    public class Settings {

        public EnemyTypeSettings Draugr = new(5, 50, 0, 0);
        public EnemyTypeSettings Dragon = new(15, 60, 0, 0);
        public EnemyTypeSettings DragonPriest = new(50, 80, 0, 0);
        public EnemyTypeSettings Forsworn = new(5, 45, 0, 0);
        public EnemyTypeSettings Hagraven = new(10, 50, 0, 0);
        public EnemyTypeSettings Bandit = new(1, 40, 0, 0);
        public EnemyTypeSettings Dwemer = new(10, 50, 0, 0);
        public EnemyTypeSettings Falmer = new(15, 50, 0, 0);
        public EnemyTypeSettings Vampire = new(15, 55, 0, 0);
        public EnemyTypeSettings Warlock = new(1, 50, 0, 0);
        public EnemyTypeSettings Daedra = new(5, 50, 0, 0);
        public EnemyTypeSettings Animals = new(1, 15, 0, 0);
        public EnemyTypeSettings BlackBook = new(25, 60, 0, 0);

        public ModifierSettings Dawnguard = new(5, 0, 10, 20);
        public ModifierSettings Dragonborn = new(10, 0, 20, 30);

        public ModifierSettings MinorBoss = new(5, 0, 10, 10);
        public ModifierSettings Boss = new(10, 0, 15, 15);
        public ModifierSettings MajorBoss = new(20, 0, 25, 25);

        public ModifierSettings Epic = new(25, 0, 30, 30);
        public ModifierSettings QuestDaedric = new(5, 0, 10, 10);
    }
}
