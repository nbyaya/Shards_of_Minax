using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class TimberStrike : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Timber Strike", "Accelerare Arbor",
                                                        21001,
                                                        9301,
                                                        false,
                                                        Reagent.BlackPearl,
                                                        Reagent.Bloodmoss
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public TimberStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You focus your energy, preparing for a burst of speed in chopping trees.");
                Caster.PlaySound(0x5B3); // Play a woodcutting-related sound effect
                Caster.FixedParticles(0x3779, 1, 15, 9902, 2413, 0, EffectLayer.Waist); // Green sparkle effect around the caster

                Caster.Target = new InternalTarget(this); // Start targeting for a tree
            }
        }

        private class InternalTarget : Target
        {
            private TimberStrike m_Owner;

            public InternalTarget(TimberStrike owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IChopable)
                {
                    Mobile caster = m_Owner.Caster;
                    IChopable tree = (IChopable)targeted;

                    // Cast tree to Item to get its location
                    Item treeItem = tree as Item;

                    if (treeItem == null || !caster.InRange(treeItem.GetWorldLocation(), 1))
                    {
                        caster.SendMessage("You are too far away to chop that tree.");
                        return;
                    }

                    // Apply the Timber Strike effect - speed boost and visuals
                    caster.SendMessage("You strike the tree with enhanced speed!");

                    // Temporary effect: Increase chop speed for 10 seconds
                    double originalSpeed = caster.NextSkillTime;
                    caster.NextSkillTime = (long)TimeSpan.FromSeconds(0.5).TotalMilliseconds; // Faster chopping rate

                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                    {
                        caster.NextSkillTime = (long)originalSpeed; // Revert back to original speed
                        caster.SendMessage("The effect of Timber Strike fades away.");
                    });

                    tree.OnChop(caster); // Perform the chop action
                }
                else
                {
                    m_Owner.Caster.SendMessage("You must target a tree.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
