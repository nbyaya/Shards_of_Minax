using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class TinderboxIgnite : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Tinderbox Ignite", "In Vas Flam",
                                                        //SpellCircle.First,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public TinderboxIgnite(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TinderboxIgnite m_Owner;

            public InternalTarget(TinderboxIgnite owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = (Item)targeted;

                    if (item is BaseLight)
                    {
                        BaseLight light = (BaseLight)item;

                        if (!light.Burning)
                        {
                            light.Ignite();

                            from.SendMessage("You ignite the {0} with a burst of flame!", item.Name ?? "item");

                            // Add visual effects and sound
                            Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                            Effects.PlaySound(item.Location, item.Map, 0x208);

                            m_Owner.FinishSequence();
                        }
                        else
                        {
                            from.SendMessage("The {0} is already burning.", item.Name ?? "item");
                        }
                    }
                    else
                    {
                        from.SendMessage("You can only ignite torches or campfires.");
                    }
                }
                else
                {
                    from.SendMessage("You must target an item to ignite.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
