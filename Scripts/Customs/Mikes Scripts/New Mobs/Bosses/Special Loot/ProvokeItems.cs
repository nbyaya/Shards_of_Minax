using System;
using Server;

namespace Server.Items
{
    public class PipesOfPan : Item
    {
        [Constructable]
        public PipesOfPan() : base(0x2805) // You can change the item ID to a suitable one
        {
            Name = "Pipes of Pan";
            Hue = 0x8A5; // Set a suitable hue
        }

        public PipesOfPan(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.Provocation].Value >= 100.0)
            {
                from.SendMessage("You feel your provocation abilities enhanced.");
                from.AddSkillMod(new DefaultSkillMod(SkillName.Provocation, true, 20.0)); // Increases Provocation skill by 20
            }
            else
            {
                from.SendMessage("You lack the skill to use these pipes.");
            }
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

    public class WildCloak : BaseCloak
    {
        [Constructable]
        public WildCloak() : base(0x1515) // Cloak item ID
        {
            Name = "Wild Cloak";
            Hue = 0x497; // Set a suitable hue
            Attributes.Luck = 50; // Example attribute, you can add more
            Resistances.Physical = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 5;
        }

        public WildCloak(Serial serial) : base(serial)
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

    public class MusicalSheet : Item
    {
        [Constructable]
        public MusicalSheet() : base(0x2259) // You can change the item ID to a suitable one
        {
            Name = "Musical Sheet";
            Hue = 0x47E; // Set a suitable hue
        }

        public MusicalSheet(Serial serial) : base(serial)
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

    public class WoodlandStatue : Item
    {
        [Constructable]
        public WoodlandStatue() : base(0x139A) // You can change the item ID to a suitable one
        {
            Name = "Woodland Statue";
            Hue = 0x483; // Set a suitable hue
        }

        public WoodlandStatue(Serial serial) : base(serial)
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
