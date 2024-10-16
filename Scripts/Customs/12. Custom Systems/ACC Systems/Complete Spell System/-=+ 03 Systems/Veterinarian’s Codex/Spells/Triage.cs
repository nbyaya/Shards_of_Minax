using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class Triage : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Triage", "Vitalis Recuperare",
            // SpellCircle.Fifth,
            21005,
            9301,
            false,
            Reagent.Ginseng,
            Reagent.Garlic
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public Triage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.SendMessage("Select the area to perform triage.");
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Point3D p) // Change to Point3D
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Effects.PlaySound(p, Caster.Map, 0x5C2); // Play a healing sound.
                Effects.SendLocationParticles(EffectItem.Create(p, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Create visual effects.

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3)) // Range of 3 tiles.
                {
                    if (m is BaseCreature && m.Hits < m.HitsMax) // Check for damaged creatures.
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile m in targets)
                    {
                        int healAmount = (int)(Caster.Skills[SkillName.Veterinary].Value / 5); // Heal amount based on Veterinary skill.
                        m.Heal(healAmount);
                        m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Heal effect on the creature.
                        m.PlaySound(0x5C9); // Play a sound on healing.
                        m.SendMessage("You feel your wounds being mended by a soothing force.");
                    }
                }
                else
                {
                    Caster.SendMessage("No injured creatures found in the area.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Triage m_Owner;

            public InternalTarget(Triage owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                {
                    Point3D p = new Point3D((IPoint3D)o); // Convert IPoint3D to Point3D
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
