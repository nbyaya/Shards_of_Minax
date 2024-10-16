using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class BonfireAura : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bonfire Aura", "Ignis Aura",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public BonfireAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Item item)
        {
            if (item is Campfire)
            {
                if (CheckSequence())
                {
                    if (this.Scroll != null)
                        Scroll.Consume();

                    Campfire campfire = (Campfire)item;
                    campfire.Movable = false;
                    campfire.Name = "Aura-Enhanced Campfire";

                    Effects.PlaySound(campfire.Location, Caster.Map, 0x208);
                    Effects.SendLocationParticles(EffectItem.Create(campfire.Location, campfire.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

                    AuraEffect(campfire, Caster);
                }
                else
                {
                    Caster.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }

            FinishSequence();
        }

        private void AuraEffect(Campfire campfire, Mobile caster)
        {
            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(5), () =>
            {
                if (campfire.Deleted)
                    return;

                var spellcasters = campfire.GetMobilesInRange(5).OfType<PlayerMobile>().Where(m => m.Alive && m.Mana < m.ManaMax).ToList();

                foreach (var mobile in spellcasters)
                {
                    mobile.Mana += 2; // Increase mana regeneration
                    mobile.FixedEffect(0x375A, 10, 15, 1153, 3); // Visual effect for the players
                }
            });

            Timer.DelayCall(TimeSpan.FromMinutes(1), () =>
            {
                if (!campfire.Deleted)
                {
                    campfire.Movable = true;
                    campfire.Name = "Campfire";
                }
            });
        }

        private class InternalTarget : Target
        {
            private BonfireAura m_Owner;

            public InternalTarget(BonfireAura owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                    m_Owner.Target(item);
                else
                    from.SendLocalizedMessage(500237); // Target can not be seen.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }

        // Removed Cooldown property
    }
}
