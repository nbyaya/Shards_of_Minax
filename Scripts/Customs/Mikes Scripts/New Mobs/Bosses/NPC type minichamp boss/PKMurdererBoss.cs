using System;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ruthless pk overlord")]
    public class PKMurdererBoss : PKMurderer
    {
        [Constructable]
        public PKMurdererBoss() : base()
        {
            Name = "Ruthless PK Overlord";
            Title = "the Ultimate Troll";

            // Update stats to match or exceed Barracoon's or better
            SetStr(1200); // Increased strength to be more powerful
            SetDex(255); // Maximized dexterity
            SetInt(250); // Increased intelligence for enhanced spellcasting

            SetHits(10000); // Increased health to a boss level
            SetDamage(25, 35); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Archery, 100.0); // Enhanced ranged skills for diversity
            SetSkill(SkillName.Swords, 100.0);
            SetSkill(SkillName.Fencing, 100.0);

            Fame = 30000; // Higher fame for a boss-tier NPC
            Karma = -30000; // Negative karma for a murderer

            VirtualArmor = 100; // Increased armor for survivability

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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be placed here, like unique speech or other mechanics
        }

        public PKMurdererBoss(Serial serial) : base(serial)
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
