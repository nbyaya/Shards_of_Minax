using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class EaglesFeast : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Eagle's Feast", "Call of the Eagle",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 20; } }

        public EaglesFeast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You summon an eagle's feast!");
                Caster.PlaySound(0x1FA); // Play sound effect
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head); // Create visual effect

                // Create the consumable item
                EagleFeastItem feast = new EagleFeastItem();
                feast.MoveToWorld(Caster.Location, Caster.Map);

                Caster.SendMessage("An Eagle's Feast appears before you.");
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        
        public class EagleFeastItem : Item
        {
            private Timer m_Timer;
			
			[Constructable]
            public EagleFeastItem() : base(0x09B7) // Use a different item ID for a unique look
            {
                Name = "Eagle's Feast";
                Hue = 0x48E; // Set color to make it visually distinct

                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }

            public EagleFeastItem(Serial serial) : base(serial)
            {
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (!Movable)
                    return;

                if (from == null || !from.InRange(this.GetWorldLocation(), 2))
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
                    return;
                }

                from.SendMessage("You consume the Eagle's Feast and feel your agility surge!");
                from.PlaySound(0x1E7); // Sound effect on consumption
                from.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist); // Visual effect on consumption

                // Apply dexterity buff
                StatMod dexBuff = new StatMod(StatType.Dex, "EaglesFeastDexBuff", (int)(from.Dex * 0.2), TimeSpan.FromMinutes(5));
                from.AddStatMod(dexBuff);

                this.Delete(); // Remove the item after use
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }

            private class InternalTimer : Timer
            {
                private Item m_Item;

                public InternalTimer(Item item) : base(TimeSpan.FromMinutes(1.0))
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    if (m_Item != null && !m_Item.Deleted)
                        m_Item.Delete();
                }
            }
        }
    }
}
