using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;


namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class OreBomb : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ore Bomb", "Blow Ore!",
            21011, // Effect ID for explosion visual
            9300 // Sound ID for explosion sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } } // Delay before the ability is cast
        public override double RequiredSkill { get { return 50.0; } } // Required skill level to use the ability
        public override int RequiredMana { get { return 30; } } // Mana cost for the ability

        public OreBomb(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private OreBomb m_Owner;

            public InternalTarget(OreBomb owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D point)
                {
                    m_Owner.Target(point);
                }
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
                return;
            }

            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                // Create the explosion effect
                Effects.SendLocationEffect(loc, Caster.Map, 0x36BD, 20, 10, 0xB72, 0); // Create explosion effect
                Effects.PlaySound(loc, Caster.Map, 0x307); // Play explosion sound

                // Deal damage to all mobiles in range
                foreach (Mobile m in Caster.Map.GetMobilesInRange(loc, 3)) // 3 tile radius
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        Caster.DoHarmful(m);
                        m.Damage(Utility.RandomMinMax(20, 30), Caster); // Deal random damage between 20 to 30
                        m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Damage effect on target
                    }
                }
            }

            FinishSequence();
        }
    }
}
