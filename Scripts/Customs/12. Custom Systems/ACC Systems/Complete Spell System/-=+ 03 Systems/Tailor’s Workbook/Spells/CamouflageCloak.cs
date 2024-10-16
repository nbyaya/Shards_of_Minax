using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class CamouflageCloak : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage Cloak", "In Vis Ul",
            21011,
            9300,
            false,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public CamouflageCloak(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckBSequence(target))
            {
                // Apply the camouflage effect
                TimeSpan duration = TimeSpan.FromSeconds(30.0 + (Caster.Skills[CastSkill].Value / 5)); // Duration based on caster's skill
                Effects.SendTargetParticles(target, 0x375A, 9, 32, 5008, EffectLayer.Waist);
                target.PlaySound(0x1FD); // Play a sound effect

                target.Hidden = true; // Make the target invisible
                target.SendMessage("You have become one with your surroundings, blending into the environment!");

                Timer.DelayCall(duration, () =>
                {
                    if (target != null && !target.Deleted && target.Hidden)
                    {
                        target.Hidden = false;
                        target.SendMessage("Your camouflage fades away, revealing you once again.");
                    }
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private CamouflageCloak m_Owner;

            public InternalTarget(CamouflageCloak owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
