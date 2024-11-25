using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the punk rock paladin overlord")]
    public class PunkRockPaladinBoss : PunkRockPaladin
    {
        [Constructable]
        public PunkRockPaladinBoss() : base()
        {
            Name = "Punk Rock Paladin Overlord";
            Title = "the Supreme Punk";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Upper range of strength for a boss
            SetDex(255); // Maximum dexterity
            SetInt(250); // Upper range of intelligence for a boss

            SetHits(12000); // Boss level health
            SetDamage(30, 50); // Higher damage range for a boss

            SetResistance(ResistanceType.Physical, 75, 90); // Increased resistances for a boss
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Full resistance to poison
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Increased fame for a boss-level creature
            Karma = -22500; // Evil karma

            VirtualArmor = 80; // Higher virtual armor for a tougher boss

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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if desired
        }

        public PunkRockPaladinBoss(Serial serial) : base(serial)
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
