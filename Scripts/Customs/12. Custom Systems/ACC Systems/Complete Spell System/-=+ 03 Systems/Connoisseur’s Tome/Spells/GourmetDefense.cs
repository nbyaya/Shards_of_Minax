using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class GourmetDefense : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Gourmet Defense", "Defensus Gusto",
                                                        21010, // Icon ID
                                                        9306  // Cast Sound
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 22; } }

        public GourmetDefense(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel your defenses strengthen as you invoke the Gourmet Defense!");

                // Play visual effect around the caster
                Effects.SendTargetParticles(Caster, 0x375A, 10, 15, 5017, EffectLayer.Waist);

                // Play sound effect
                Caster.PlaySound(0x5C3);

                // Increase Armor temporarily
                int armorBoost = 10 + (int)(Caster.Skills[SkillName.TasteID].Value / 5); // Calculate armor increase based on skill
                TimeSpan duration = TimeSpan.FromSeconds(10 + (Caster.Skills[SkillName.TasteID].Value / 5)); // Duration based on skill



                // Apply the armor boost effect
                new GourmetDefenseTimer(Caster, armorBoost, duration).Start();
            }

            FinishSequence();
        }

        private class GourmetDefenseTimer : Timer
        {
            private Mobile m_Caster;
            private int m_ArmorBoost;

            public GourmetDefenseTimer(Mobile caster, int armorBoost, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
                m_ArmorBoost = armorBoost;

                caster.VirtualArmorMod += armorBoost; // Apply armor boost
            }

            protected override void OnTick()
            {
                if (m_Caster != null && !m_Caster.Deleted)
                {
                    m_Caster.VirtualArmorMod -= m_ArmorBoost; // Remove armor boost
                    m_Caster.SendMessage("The effects of Gourmet Defense fade away.");
                }
            }
        }
    }
}
