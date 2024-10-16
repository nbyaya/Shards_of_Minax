using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class SavageBlow : TacticsSpell // Assuming PastoraliconSpell is the base class for custom spells
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Savage Blow", "Die!",
            21004, // Button ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Set appropriate circle level
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public SavageBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SavageBlow m_Owner;

            public InternalTarget(SavageBlow owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1005442); // You can't do that.
                        return;
                    }

                    if (SpellHelper.CheckTown(from, target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        SpellHelper.Turn(from, target);

                        // Play visual and sound effects
                        Effects.SendTargetEffect(target, 0x37B9, 10, 30, 0x47E, 0); // Flashy visual effect
                        from.PlaySound(0x511); // Sound effect

                        // Calculate damage and stun chance
                        double damage = from.Skills[SkillName.Swords].Value / 5.0 + Utility.RandomMinMax(10, 20);
                        target.Damage((int)damage, from);

                        if (Utility.RandomDouble() < 0.3) // 30% chance to stun
                        {
                            TimeSpan stunDuration = TimeSpan.FromSeconds(2.0);
                            target.Paralyze(stunDuration);
                            target.SendMessage("You have been stunned by the savage blow!");
                        }
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
