using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class WoundAnalysis : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Wound Analysis", "Targus Bargus",
                                                        21004, // Effect item ID
                                                        9300   // Sound ID
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } } // Quick cast delay for an immediate effect
        public override double RequiredSkill { get { return 30.0; } } // Low skill requirement
        public override int RequiredMana { get { return 20; } } // Mana cost

        public WoundAnalysis(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WoundAnalysis m_Owner;

            public InternalTarget(WoundAnalysis owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(from, target);

                        // Play visual effect and sound
                        Effects.SendTargetParticles(target, 0x374A, 10, 15, 5021, EffectLayer.Waist);
                        target.PlaySound(0x213); // Sound effect for impact

                        // Apply paralysis effect for 2 seconds
                        target.Paralyze(TimeSpan.FromSeconds(2));

                        // Display message
                        from.SendMessage("You hit your target with a paralyzing blow!");
                        target.SendMessage("You are paralyzed by a heavy blow!");

                        // Additional flashy effect (e.g., blood splatter)
                        Effects.SendLocationParticles(
                            EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                            0x36BD, 20, 10, 1153, 0, 5029, 0
                        );
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
