using System;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the alien overlord")]
    public class AlienWarriorBoss : AlienWarrior
    {
        [Constructable]
        public AlienWarriorBoss() : base()
        {
            Name = "Alien Overlord";
            Title = "the Supreme Warrior";

            // Enhance stats to boss level (comparable to Barracoon's)
            SetStr(500, 800); // Increase strength to boss level
            SetDex(500, 800); // Increase dexterity to boss level
            SetInt(500, 800); // Increase intelligence to boss level

            SetHits(12000); // Increase health to boss level (like Barracoon)
            SetDamage(100, 150); // Keeping damage range as it is, but this could be adjusted based on needs

            // Set higher resistances (also matching Barracoon's resistances)
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            // Set skill levels comparable to Barracoon's
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 150.0, 200.0);
            SetSkill(SkillName.Wrestling, 150.0, 200.0);
            SetSkill(SkillName.Fencing, 150.0, 200.0);
            SetSkill(SkillName.Macing, 150.0, 200.0);
            SetSkill(SkillName.Swords, 150.0, 200.0);

            Fame = 22500; // Match Barracoon's fame
            Karma = -22500; // Negative karma as per the original NPC

            // Add superior armor for the boss
            int armorHue = Utility.Random(2000, 1001);
            AddItem(new PlateChest() { Hue = armorHue, Name = "Alien Overlord Space Armor" });
            AddItem(new PlateLegs() { Hue = armorHue, Name = "Alien Overlord Space Armor" });
            AddItem(new PlateArms() { Hue = armorHue, Name = "Alien Overlord Space Armor" });
            AddItem(new PlateGloves() { Hue = armorHue, Name = "Alien Overlord Space Armor" });
            AddItem(new PlateHelm() { Hue = armorHue, Name = "Alien Overlord Space Armor" });

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls in addition to the usual loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add ultra-rich loot for the boss
            AddLoot(LootPack.UltraRich);
        }

        public AlienWarriorBoss(Serial serial) : base(serial)
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
