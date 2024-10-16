using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class MasterCarpentersSaw : Item
    {
        [Constructable]
        public MasterCarpentersSaw() : base(0x1034) // Saw item ID
        {
            Weight = 5.0;
            Name = "Master Carpenter's Saw";
            Hue = 0x47E; // Custom hue
        }

        public MasterCarpentersSaw(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Speeds up crafting by 25%");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.Carpentry].Value >= 100.0)
            {
                from.SendMessage("You feel the power of the Master Carpenter's Saw enhance your crafting speed.");
                // Logic to increase crafting speed by 25%
            }
            else
            {
                from.SendMessage("You lack the skill to use this item.");
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

    public class WoodPuppet : Item
    {
        [Constructable]
        public WoodPuppet() : base(0x20DA) // Wooden puppet item ID
        {
            Weight = 1.0;
            Name = "Wooden Puppet";
            Hue = 0x47E; // Custom hue
        }

        public WoodPuppet(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Summons a wooden golem to fight for you.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500486); // That is too far away.
                return;
            }

            if (from.Skills[SkillName.Carpentry].Value >= 100.0)
            {
                from.SendMessage("You summon a wooden golem to fight for you.");
                WoodenGolem golem = new WoodenGolem();
                golem.MoveToWorld(from.Location, from.Map);
                golem.Combatant = from.Combatant;
                this.Consume(); // The item is consumed after use
            }
            else
            {
                from.SendMessage("You lack the skill to use this item.");
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

    public class WoodenGolem : BaseCreature
    {
        [Constructable]
        public WoodenGolem() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wooden golem";
            Body = 0x33;
            Hue = 0x47E; // Custom hue

            SetStr(250, 300);
            SetDex(50, 75);
            SetInt(50, 75);

            SetHits(1000);
            SetMana(0);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 70.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 50;
        }

        public WoodenGolem(Serial serial) : base(serial)
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
