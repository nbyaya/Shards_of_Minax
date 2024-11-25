using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the poetic overlord")]
    public class DuelistPoetBoss : DuelistPoet
    {
        [Constructable]
        public DuelistPoetBoss() : base()
        {
            Name = "Poetic Overlord";
            Title = "the Supreme Duelist";

            // Update stats to match or exceed Barracoon (as a benchmark)
            SetStr(1200); // Matching or better than Barracoon's strength
            SetDex(255); // Upper dexterity range
            SetInt(250); // Upper intelligence range

            SetHits(12000); // Higher health for a boss-tier creature
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 150.0); // High Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma for a boss

            VirtualArmor = 70; // Higher virtual armor for more durability

            // Attach a random ability for extra challenges
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
            // Additional boss logic (such as special abilities or attacks) could be added here
        }

        public DuelistPoetBoss(Serial serial) : base(serial)
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
