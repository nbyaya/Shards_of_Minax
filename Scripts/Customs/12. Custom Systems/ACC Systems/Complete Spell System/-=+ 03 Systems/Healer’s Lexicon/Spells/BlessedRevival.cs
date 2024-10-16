using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class BlessedRevival : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Blessed Revival", "Anima Revivire",
            // SpellCircle.Sixth,
            21004, // Placeholder GumpID
            9300,  // Placeholder SoundID
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        public BlessedRevival(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BlessedRevival m_Owner;

            public InternalTarget(BlessedRevival owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (target == null || target.Alive)
                    {
                        from.SendLocalizedMessage(1060508); // Target must be a dead player.
                    }
                    else if (m_Owner.CheckBSequence(target))
                    {
                        SpellHelper.Turn(from, target);

                        // Play revival sound and effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 10, 1, 1153, 4);
                        Effects.PlaySound(target.Location, target.Map, 0x214);

                        // Revive target
                        target.Resurrect();

                        // Restore health and mana
                        target.Hits = (int)(target.HitsMax * 0.5); // Restore 50% health
                        target.Mana = (int)(target.ManaMax * 0.3); // Restore 30% mana

                        from.SendMessage("You have revived your ally with Blessed Revival!");
                        target.SendMessage("You have been revived with Blessed Revival!");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
