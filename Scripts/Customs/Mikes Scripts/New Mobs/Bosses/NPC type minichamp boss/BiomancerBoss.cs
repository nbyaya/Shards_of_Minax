using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the biomancer overlord")]
    public class BiomancerBoss : Biomancer
    {
        [Constructable]
        public BiomancerBoss() : base()
        {
            Name = "Biomancer Overlord";
            Title = "the Grand Priest";

            // Enhanced Stats (based on or exceeding Biomancer's base stats)
            SetStr(1200); // Increase strength significantly
            SetDex(255); // Maximum dexterity
            SetInt(750); // Higher intelligence for more spellcasting potential

            SetHits(12000); // Increased health to match a boss-tier NPC
            SetDamage(30, 45); // Increased damage output

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100); // Keep poison resistance high
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // Improve casting capabilities
            SetSkill(SkillName.Meditation, 80.0); // Improve meditation for mana regeneration

            Fame = 25000; // Higher fame for a boss-tier creature
            Karma = -25000; // High negative karma for a villainous boss

            VirtualArmor = 80; // Increased virtual armor to make the boss more durable

            // Attach a random ability using XMLSpawner
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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could be added here if needed
        }

        public BiomancerBoss(Serial serial) : base(serial)
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
