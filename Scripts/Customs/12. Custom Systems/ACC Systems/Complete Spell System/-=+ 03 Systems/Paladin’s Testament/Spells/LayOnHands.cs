using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items; // Add this line to include the namespace where Corpse is defined

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class LayOnHands : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lay on Hands", "In Bas Maxi",
            21009,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 35; } }

        public LayOnHands(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private LayOnHands m_Owner;

            public InternalTarget(LayOnHands owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile m = (Mobile)target;

                    if (m.Alive)
                    {
                        if (m.Hits < m.HitsMax)
                        {
                            int healAmount = (int)(from.Skills[SkillName.Healing].Value * 0.5 + Utility.Random(15, 25));
                            m.Hits += healAmount;
                            m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                            m.PlaySound(0x202);
                            from.SendMessage("You lay your hands and heal the target for {0} health.", healAmount);
                            m.SendMessage("You feel a warm light healing your wounds.");
                        }
                        else
                        {
                            from.SendMessage("That target is already at full health.");
                        }
                    }
                    else if (m is PlayerMobile && m.Corpse != null && m.Corpse is Corpse corpse && !corpse.Deleted && corpse.TimeOfDeath + TimeSpan.FromSeconds(10) > DateTime.UtcNow)
                    {
                        // 20% chance to revive a recently deceased player
                        if (Utility.RandomDouble() < 0.2)
                        {
                            m.Resurrect();
                            m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head);
                            m.PlaySound(0x214);
                            from.SendMessage("You miraculously bring {0} back to life!", m.Name);
                            m.SendMessage("You have been resurrected by a miraculous power!");
                        }
                        else
                        {
                            from.SendMessage("The resurrection attempt failed.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You cannot target that.");
                    }
                }
                else
                {
                    from.SendMessage("You must target a living or recently deceased being.");
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
