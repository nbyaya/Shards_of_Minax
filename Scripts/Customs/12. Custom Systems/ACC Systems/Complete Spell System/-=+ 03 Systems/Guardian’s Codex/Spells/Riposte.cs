using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class Riposte : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Riposte", "Ateris Mors",
                                                        // SpellCircle.Sixth,
                                                        21005,
                                                        9301,
                                                        false,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public Riposte(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You prepare to unleash a powerful counterattack!");

                // Effects
                Caster.FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);
                Caster.PlaySound(0x211);

                ArrayList targets = new ArrayList();

                // Get all creatures in 1-tile radius
                foreach (Mobile m in Caster.GetMobilesInRange(1))
                {
                    if (Caster.CanBeHarmful(m) && m != Caster)
                    {
                        targets.Add(m);
                    }
                }

                // Perform the attack on all targets
                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    if (Caster.CanBeHarmful(m, false))
                    {
                        Caster.DoHarmful(m);
                        int damage = (int)(Caster.Skills[SkillName.Swords].Value / 2.0); // Calculate damage based on skill level
                        AOS.Damage(m, Caster, damage, 100, 0, 0, 0, 0); // Physical damage
                        m.SendMessage("You have been struck by a powerful riposte!");
                        m.FixedParticles(0x374A, 1, 15, 9909, EffectLayer.Head); // Visual effect for target
                        m.PlaySound(0x29D); // Sound effect for target
                    }
                }
            }

            FinishSequence();
        }
    }
}
