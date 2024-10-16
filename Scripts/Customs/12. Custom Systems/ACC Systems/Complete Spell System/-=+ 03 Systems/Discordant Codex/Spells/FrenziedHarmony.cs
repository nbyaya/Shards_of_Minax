using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class FrenziedHarmony : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Frenzied Harmony", "In Vos Moni",
            // SpellCircle.Sixth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public FrenziedHarmony(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();

                Effects.PlaySound(Caster.Location, Caster.Map, 0x5B3); // Play a sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist); // Visual effect at the caster

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(2))
                {
                    if (Caster.CanBeBeneficial(m) && m != Caster && m is BaseCreature)
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = targets[i];

                        // Apply effects
                        m.SendMessage("You feel a surge of strength but notice your armor weakening!");
                        Effects.SendTargetParticles(m, 0x373A, 1, 15, 5007, EffectLayer.Waist); // Visual effect on targets

                        int strBoost = (int)(m.RawStr * 0.20); // 20% increase in strength
                        int armorReduction = -(int)(m.ArmorRating * 0.20); // 20% decrease in armor

                        m.AddStatMod(new StatMod(StatType.Str, "FrenziedHarmonyStr", strBoost, TimeSpan.FromSeconds(30)));
                        m.VirtualArmorMod -= armorReduction; // Temporarily reduce armor

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            if (m != null && !m.Deleted)
                            {
                                m.RemoveStatMod("FrenziedHarmonyStr");
                                m.VirtualArmorMod += armorReduction; // Restore armor
                            }
                        });
                    }
                }
                else
                {
                    Caster.SendMessage("There are no valid targets in range.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
