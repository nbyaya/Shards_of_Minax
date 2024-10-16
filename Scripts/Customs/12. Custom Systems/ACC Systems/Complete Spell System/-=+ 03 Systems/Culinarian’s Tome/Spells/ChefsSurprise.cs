using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class ChefsSurprise : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Chef's Surprise", "Gustatio Magica",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public ChefsSurprise(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play casting sound and visual effect
                Caster.PlaySound(0x1FA);
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);

                // Create the consumable item
                ChefsSurpriseItem item = new ChefsSurpriseItem();
                Caster.AddToBackpack(item);

                Caster.SendMessage("You summon a Chef's Surprise!");

                // Play success sound
                Effects.PlaySound(Caster.Location, Caster.Map, 0x5C0);
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

    public class ChefsSurpriseItem : Item
    {
        [Constructable]
        public ChefsSurpriseItem() : base(0x9B9)
        {
            Name = "Chef's Surprise";
            Hue = 1161; // Unique color for the item
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            // Randomly decide the type of bonus
            int randomEffect = Utility.Random(3);
            int bonus = Utility.RandomMinMax(5, 10); // Random bonus amount

            // Apply the bonus effect
            switch (randomEffect)
            {
                case 0:
                    from.SendMessage("You feel a surge of strength!");
                    from.AddStatMod(new StatMod(StatType.Str, "ChefSurpriseStr", bonus, TimeSpan.FromMinutes(5)));
                    from.FixedParticles(0x375A, 1, 15, 5021, EffectLayer.Waist);
                    from.PlaySound(0x44D);
                    break;
                case 1:
                    from.SendMessage("You feel a surge of dexterity!");
                    from.AddStatMod(new StatMod(StatType.Dex, "ChefSurpriseDex", bonus, TimeSpan.FromMinutes(5)));
                    from.FixedParticles(0x375A, 1, 15, 5019, EffectLayer.Waist);
                    from.PlaySound(0x44C);
                    break;
                case 2:
                    from.SendMessage("You feel a surge of intelligence!");
                    from.AddStatMod(new StatMod(StatType.Int, "ChefSurpriseInt", bonus, TimeSpan.FromMinutes(5)));
                    from.FixedParticles(0x375A, 1, 15, 5020, EffectLayer.Waist);
                    from.PlaySound(0x44E);
                    break;
            }

            // Delete the item after use
            this.Delete();
        }

        public ChefsSurpriseItem(Serial serial) : base(serial)
        {
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
    }
}
