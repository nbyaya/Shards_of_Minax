using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the psychedelic overlord")]
    public class PsychedelicShamanBoss : PsychedelicShaman
    {
        [Constructable]
        public PsychedelicShamanBoss() : base()
        {
            Name = "Psychedelic Overlord";
            Title = "the Cosmic Sage";

            // Update stats to match or exceed Barracoon (for a more boss-like creature)
            SetStr(1200); // Upper strength for a boss-tier character
            SetDex(255); // Maximum dexterity for agility
            SetInt(250); // High intelligence for magical power

            SetHits(12000); // Match Barracoon's health
            SetDamage(29, 38); // Use a high damage range suitable for a boss

            // Resistance enhancements
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 25000; // Boss-level fame
            Karma = -25000; // Negative karma for a villainous boss

            VirtualArmor = 80; // Enhanced armor for a boss-tier creature

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
            
            // Add additional items or loot for the boss-tier creature
            PackGem();
            PackGold(500, 600);
            AddLoot(LootPack.Rich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(20, 40)));
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can go here, such as unique speech or enhanced behavior
        }

        public PsychedelicShamanBoss(Serial serial) : base(serial)
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
