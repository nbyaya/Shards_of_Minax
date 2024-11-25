using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the 50s menace")]
    public class PinupPandemoniumBoss : PinupPandemonium
    {
        [Constructable]
        public PinupPandemoniumBoss() : base()
        {
            Name = "Pinup Pandemonium";
            Title = "the 50s Menace - Boss";

            // Update stats to match or exceed Barracoon's or better stats
            SetStr(1200); // Increase strength to match Barracoon's upper value
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(10000); // Increased health to be on par with Barracoon
            SetDamage(29, 38); // Higher damage range to reflect a boss-level challenge

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Enhanced resistances for a boss-tier creature
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            // Enhanced skills, especially in MagicResist, Tactics, and Wrestling
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased Fame for a boss
            Karma = -30000; // Negative Karma for boss

            VirtualArmor = 80; // Increased armor for better protection

            // Attach a random ability to the boss
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

            // Custom loot
            PackGold(500, 700);
            AddLoot(LootPack.FilthyRich);

            this.Say(true, "This is the end of the line, darling.");
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
        }

        public PinupPandemoniumBoss(Serial serial) : base(serial)
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
