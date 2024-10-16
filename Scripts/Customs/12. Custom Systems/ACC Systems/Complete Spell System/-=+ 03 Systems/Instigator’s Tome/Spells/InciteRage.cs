using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class InciteRage : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Incite Rage", "Incitatus Iram",
                                                        21005,
                                                        9400
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 20; } }

        public InciteRage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private InciteRage m_Owner;

            public InternalTarget(InciteRage owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Point3D p)
                {
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Point3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                // Convert Point3D to IPoint3D for compatibility
                IPoint3D ipoint = p;

                // Call the method with the correct type
                SpellHelper.GetSurfaceTop(ref ipoint);

                // Convert IPoint3D back to Point3D
                p = new Point3D(ipoint);

                Effects.PlaySound(p, Caster.Map, 0x2A1); // Play a sound effect
                Effects.SendLocationParticles(EffectItem.Create(p, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5042); // Display particles at the target location

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m != Caster && m.Alive && !IsEnemy(m, Caster) && Caster.CanBeBeneficial(m, false))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    Caster.DoBeneficial(m);
                    m.FixedEffect(0x375A, 10, 15, 1153, 0); // Display a visual effect on the ally
                    m.SendMessage("You feel a surge of rage inciting you to fight more fiercely!");

                    // Temporarily boost combat skills
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Rage, 1075656, 1075657, TimeSpan.FromSeconds(30), m)); // Buff with "Rage" icon for 30 seconds
                    m.CheckSkill(SkillName.Tactics, 1.0, m.Skills[SkillName.Tactics].Cap + 20.0); // Increase Tactics temporarily
                    m.CheckSkill(SkillName.Swords, 1.0, m.Skills[SkillName.Swords].Cap + 20.0); // Increase Swordsmanship temporarily
                    m.CheckSkill(SkillName.Archery, 1.0, m.Skills[SkillName.Archery].Cap + 20.0); // Increase Archery temporarily
                }
            }

            FinishSequence();
        }

        private bool IsEnemy(Mobile target, Mobile caster)
        {
            // You need to implement your custom logic to determine if the target is an enemy of the caster.
            // This could involve checking faction, alignment, or any other criteria based on your server setup.
            return !caster.CanBeBeneficial(target, false); // Placeholder example
        }
    }
}
