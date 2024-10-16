using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class QuickReflexes : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Reflexes", "Velox Acies",
            21011,
            9410
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the effect to the caster
                Caster.SendMessage("You feel your reflexes sharpen as you prepare for quick movements!");

                // Play a sound and visual effect for feedback
                Caster.PlaySound(0x3E9); // sound effect
                Caster.FixedParticles(0x3779, 1, 15, 9950, 4, 3, EffectLayer.Waist); // particle effect

                // Calculate bonuses
                double agilityBonus = Caster.Skills[SkillName.Tactics].Value * 0.1; // 10% of Tactics skill as agility bonus
                int defenseBonus = 10 + (int)(Caster.Skills[SkillName.Parry].Value * 0.05); // 5% of Parry skill as defense bonus

                // Apply agility bonus using StatMod
                StatMod dexMod = new StatMod(StatType.Dex, "QuickReflexesDex", (int)agilityBonus, TimeSpan.FromSeconds(30));
                Caster.AddStatMod(dexMod);

                // Apply defense bonus by increasing physical resistance (or any other suitable resistance)
                ResistanceMod physResMod = new ResistanceMod(ResistanceType.Physical, defenseBonus);
                Caster.AddResistanceMod(physResMod);

                // Schedule to remove bonuses after duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    Caster.RemoveStatMod("QuickReflexesDex");
                    Caster.RemoveResistanceMod(physResMod);
                    Caster.SendMessage("The effects of Quick Reflexes have worn off.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
