using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class MasterfulProvocation : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Masterful Provocation", "Taunt All!",
                                                        21005,
                                                        9500
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 30; } }

        public MasterfulProvocation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(10)) // Range of 10 tiles
                {
                    if (m != Caster && m.Alive && Caster.CanBeHarmful(m) && m is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        if (bc.ControlMaster == null) // Ensure it's not a player-controlled creature
                        {
                            targets.Add(m);
                        }
                    }
                }

                if (targets.Count > 0)
                {
                    Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist); // Visual effect on caster
                    Caster.PlaySound(0x2B1); // Sound effect
                    Caster.Say("You will all focus on me!"); // Taunt message

                    foreach (Mobile target in targets)
                    {
                        BaseCreature creature = (BaseCreature)target;
                        creature.FocusMob = Caster; // Forces the creatures to target the caster
                        creature.Combatant = Caster;

                        // Flashy visual effect and sound on the creatures
                        creature.FixedParticles(0x374A, 10, 15, 5030, EffectLayer.Head);
                        creature.PlaySound(0x145);

                        // Apply provocation effect (provocation duration based on caster's skill level)
                        double duration = 5.0 + (Caster.Skills[SkillName.Provocation].Value * 0.05);

                        // Simulate buff effect using a timer
                        Timer.DelayCall(TimeSpan.FromSeconds(duration), () => 
                        {
                            if (creature != null && creature.Alive)
                            {
                                creature.SendMessage("The provocation effect has worn off.");
                            }
                        });
                    }

                    // Boost caster's defense temporarily
                    Caster.VirtualArmorMod += 20; // Increase armor by 20
                    Timer.DelayCall(TimeSpan.FromSeconds(10), delegate
                    {
                        Caster.VirtualArmorMod -= 20; // Remove the defense boost after 10 seconds
                    });
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Reduced cast delay for quicker use in battle
        }
    }
}
