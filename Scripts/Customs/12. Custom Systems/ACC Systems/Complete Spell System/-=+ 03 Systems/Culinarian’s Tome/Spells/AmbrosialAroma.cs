using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class AmbrosialAroma : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Ambrosial Aroma", "Cookus Flux",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public AmbrosialAroma(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Summon the consumable item
                Caster.SendMessage("You summon an Ambrosial Aroma.");
                AmbrosialAromaItem aromaItem = new AmbrosialAromaItem(Caster);
                Caster.AddToBackpack(aromaItem);
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Caster.PlaySound(0x1F2);
            }

            FinishSequence();
        }
    }

    public class AmbrosialAromaItem : Item
    {
        private Mobile m_Caster;

        [Constructable]
        public AmbrosialAromaItem(Mobile caster) : base(0x0993) // Using an appropriate item ID for the consumable
        {
            m_Caster = caster;
            Name = "Ambrosial Aroma";
            Hue = 1150; // A unique hue to distinguish the item
            Weight = 1.0;
        }

        public AmbrosialAromaItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == m_Caster)
            {
                from.SendMessage("Select a monster to feed the Ambrosial Aroma.");
                from.Target = new AmbrosialAromaTarget(this);
            }
            else
            {
                from.SendMessage("This is not your Ambrosial Aroma!");
            }
        }

        private class AmbrosialAromaTarget : Target
        {
            private Item m_Item;

            public AmbrosialAromaTarget(Item item) : base(12, false, TargetFlags.Harmful)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && target is BaseCreature && from.CanSee(target))
                {
                    BaseCreature creature = (BaseCreature)target;
                    from.SendMessage("You feed the Ambrosial Aroma to the creature!");

                    // Apply paralyze effect
                    creature.Paralyze(TimeSpan.FromSeconds(5.0 + (from.Skills[SkillName.Magery].Value / 20.0)));
                    creature.PlaySound(0x214);
                    Effects.SendTargetParticles(creature, 0x374A, 10, 15, 5021, EffectLayer.Waist);

                    // Consume the item
                    m_Item.Delete();
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Mobile)m_Caster);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Caster = reader.ReadMobile();
        }
    }
}
