using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frost drakon overlord corpse")]
    public class FrostDrakonBoss : FrostDrakon
    {
        private static readonly string[] BossAbilityMessages = new string[]
        {
            "The Frost Drakon breathes an overwhelming freezing breath!",
            "An unbreakable ice barrier surrounds the Frost Drakon!",
            "Glacial wards shatter the very air around the Frost Drakon!"
        };

        [Constructable]
        public FrostDrakonBoss() : base()
        {
            Name = "Frost Drakon Overlord";
            Title = "the Supreme Drakon";

            // Enhance stats to match or exceed Barracoon-like levels
            SetStr(1200, 1500); // Higher strength for a boss-tier creature
            SetDex(255, 350); // Max out dexterity for superior speed and defense
            SetInt(250, 350); // High intelligence for better spellcasting and abilities

            SetHits(15000); // Increased health for a tougher fight

            SetDamage(35, 50); // Increased damage for a more challenging fight

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for better combat
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill

            Fame = 30000; // Increase fame for a boss-tier monster
            Karma = -30000; // Negative karma for a malevolent creature

            VirtualArmor = 100; // High virtual armor for superior defense

            Tamable = false; // It cannot be tamed, as it's a boss
            ControlSlots = 0; // Not tamable or controllable

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // You can also add extra loot if needed (e.g., a rare item or artifact)
            // AddLoot(LootPack.Average); // You can uncomment to add more loot
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional logic for the boss NPC can go here
            // For example, custom behavior based on combat conditions or more frequent use of abilities
        }

        public FrostDrakonBoss(Serial serial) : base(serial)
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
