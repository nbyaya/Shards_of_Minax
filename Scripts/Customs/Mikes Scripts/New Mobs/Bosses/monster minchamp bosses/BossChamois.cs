using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss chamois corpse")]
    public class BossChamois : Chamois
    {
        [Constructable]
        public BossChamois()
        {
            Name = "Chamois";
            Title = "the Mountain King";
            Hue = 1152; // Unique hue for the boss
            Body = 0xD1; // Goat body
            BaseSoundID = 0x99; // Standard sound

            // Enhanced stats, based on Barracoon's level but more focused on agility and resilience
            SetStr(1600, 2000);
            SetDex(300, 400);
            SetInt(400, 600);

            SetHits(15000); // Boss-level health
            SetDamage(45, 60); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 130.0);
            SetSkill(SkillName.Anatomy, 90.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 90.0);
            SetSkill(SkillName.Throwing, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Negative karma for a boss

            VirtualArmor = 120; // High virtual armor to make it tough

            Tamable = false; // Boss-level creature is untamable
            ControlSlots = 0;
            MinTameSkill = 0; // Not tamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add additional loot drops
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drop 5 MaxxiaScrolls on defeat
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You will never conquer these mountains!");
            PackGold(1500, 2000); // Enhanced gold drops for boss
            AddLoot(LootPack.FilthyRich, 3); // More loot
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 12); // More gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities

            // Custom abilities for the boss go here (same as original)
        }

        public BossChamois(Serial serial) : base(serial)
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
