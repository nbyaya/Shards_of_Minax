using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class MightyBlow : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mighty Blow", "Take a BLOW!",
            21010,
            9306
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust based on your desired circle
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public MightyBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MightyBlow m_Owner;

            public InternalTarget(MightyBlow owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Apply damage with increased force
                        double damage = Utility.RandomMinMax(30, 50);
                        damage *= (1.0 + (from.Skills[m_Owner.CastSkill].Value / 100.0)); // Increase based on skill
                        AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                        // Chance to knock back enemies
                        if (Utility.RandomDouble() < 0.4) // 40% chance to knock back
                        {
                            Point3D loc = target.Location;
                            Point3D newLoc = new Point3D(loc.X + Utility.RandomMinMax(-2, 2), loc.Y + Utility.RandomMinMax(-2, 2), loc.Z);

                            if (from.Map.CanSpawnMobile(newLoc))
                            {
                                target.Location = newLoc;
                                target.FixedParticles(0x3779, 10, 15, 5013, EffectLayer.Head); // Knockback effect
                                target.PlaySound(0x1FE); // Sound for knockback
                            }
                        }

                        // Visual and sound effect for the attack
                        from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Waist);
                        from.PlaySound(0x1F5);

                        // Cooldown or delay after use (optional)
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
