using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class Purify : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Purify", "Sanctus Purificare",
            21005,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public Purify(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                // Check for existing poison and curses
                if (target.Poison != null)
                {
                    target.CurePoison(Caster);
                    target.SendMessage("You have been cleansed of poison!");
                }

                if (HasCursedDebuff(target))
                {
                    RemoveCurses(target);
                    target.SendMessage("You have been cleansed of curses!");
                }

                // Grant temporary immunity to future debuffs
                ApplyBuff(target);

                // Play sound and visual effects
                Effects.SendTargetParticles(target, 0x374A, 10, 15, 5020, EffectLayer.Waist);
                target.PlaySound(0x1F2);

                FinishSequence();
            }
        }

        private bool HasCursedDebuff(Mobile target)
        {
            // Implement your own logic to determine if the target is cursed.
            // This is just a placeholder method.
            return false; // Replace with actual check
        }

        private void RemoveCurses(Mobile target)
        {
            // Logic to remove curses, customize this based on your server's curse system
            // This is just a placeholder for curse removal.
        }

        private void ApplyBuff(Mobile target)
        {
            // Implement your own method to apply a buff.
            // If your server uses a specific buff system, use that here.
            // This is just a placeholder for applying a buff.
        }

        private class InternalTarget : Target
        {
            private Purify m_Owner;

            public InternalTarget(Purify owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
