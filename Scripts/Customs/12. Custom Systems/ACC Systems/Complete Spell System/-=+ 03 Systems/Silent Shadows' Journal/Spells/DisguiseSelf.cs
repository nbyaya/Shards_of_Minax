using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class DisguiseSelf : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disguise Self", "In Exum Forma",
            21004, 9203
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 30; } }

        private static readonly TimeSpan m_Duration = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan m_Cooldown = TimeSpan.FromMinutes(5);

        public DisguiseSelf(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            if (target == null || target.Deleted || !Caster.CanSee(target) || !Caster.InRange(target, 12))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (target is PlayerMobile)
            {
                if (Caster.BeginAction(typeof(DisguiseSelf)))
                {
                    Caster.PlaySound(0x4B0); // Disguise sound
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5021); // Visual effect

                    Caster.NameMod = "Disguised " + target.Name; // Change name
                    Caster.BodyMod = target.Body; // Change body appearance
                    Caster.HueMod = target.Hue; // Change hue

                    Timer.DelayCall(m_Duration, EndDisguise, Caster); // End disguise after duration
                    Timer.DelayCall(m_Cooldown, () => Caster.EndAction(typeof(DisguiseSelf))); // Cooldown
                }
                else
                {
                    Caster.SendMessage("You must wait before using this ability again.");
                }
            }

            FinishSequence();
        }

        private static void EndDisguise(Mobile caster)
        {
            if (caster != null && !caster.Deleted)
            {
                caster.PlaySound(0x4B0); // Revert sound
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5021); // Visual effect

                caster.NameMod = null; // Revert name
                caster.BodyMod = 0; // Revert body
                caster.HueMod = -1; // Revert hue
                caster.SendMessage("You are no longer disguised.");
            }
        }

        private class InternalTarget : Target
        {
            private DisguiseSelf m_Owner;

            public InternalTarget(DisguiseSelf owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendMessage("You can only disguise yourself as another person.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
