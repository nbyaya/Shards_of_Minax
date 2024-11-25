using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme wrestler")]
    public class GrecoRomanWrestlerBoss : GrecoRomanWrestler
    {
        [Constructable]
        public GrecoRomanWrestlerBoss() : base()
        {
            Name = "Supreme Wrestler";
            Title = "the Ultimate Grappler";

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Enhanced strength, matching or better than Barracoon
            SetDex(250); // Enhanced dexterity
            SetInt(200); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Increased damage for boss

            // Update resistances to be more robust for a boss fight
            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 75;

            // Attach a random ability for dynamic gameplay
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

            // Optionally, you can add more custom loot or items here
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here (like custom combat behavior)
        }

        public GrecoRomanWrestlerBoss(Serial serial) : base(serial)
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
