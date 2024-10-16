using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class RockSmash : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Rock Smash", "Terra Explosus",
                                                        21009, // Icon ID
                                                        9300,  // Animation ID
                                                        false,
                                                        Reagent.Bloodmoss, 
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public RockSmash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RockSmash m_Owner;

            public InternalTarget(RockSmash owner) : base(10, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Play visual effects and sounds
                Effects.PlaySound(loc, Caster.Map, 0x207); // Rock breaking sound
                Effects.SendLocationEffect(loc, Caster.Map, 0x3728, 20, 10, 1150, 0); // Rock smash animation

                // Area effect damage and stun
                Map map = Caster.Map;
                if (map != null)
                {
                    foreach (Mobile m in map.GetMobilesInRange(loc, 3))
                    {
                        if (m != Caster && Caster.CanBeHarmful(m, false))
                        {
                            Caster.DoHarmful(m);
                            AOS.Damage(m, Caster, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0); // Physical damage

                            if (Utility.RandomDouble() < 0.25) // 25% chance to stun
                            {
                                m.Freeze(TimeSpan.FromSeconds(2.0));
                                m.SendMessage("You are stunned by the force of the rock smash!");
                            }
                        }
                    }
                }
            }

            FinishSequence();
        }
    }
}
