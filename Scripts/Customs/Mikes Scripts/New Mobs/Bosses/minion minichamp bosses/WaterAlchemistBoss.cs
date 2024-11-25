using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the tidal alchemist")]
    public class WaterAlchemistBoss : WaterAlchemist
    {
        [Constructable]
        public WaterAlchemistBoss() : base()
        {
            Name = "Tidal Alchemist";
            Title = "the Supreme Alchemist";

            // Update stats to make it a boss-tier creature
            SetStr(300, 400); // Higher strength than original
            SetDex(100, 150); // Higher dexterity
            SetInt(250, 350); // Higher intelligence
            SetHits(12000); // Set to a much higher health than original

            SetDamage(20, 40); // Enhanced damage range to make it more dangerous

            // Enhance resistances to be more formidable
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Alchemy, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Boss-level karma

            VirtualArmor = 60; // Increased armor to match boss difficulty

            // Attach the XmlRandomAbility to provide random abilities to this boss
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
            // Add more custom behaviors if needed
            this.Say(true, "Feel the surge of the ocean's fury!");
        }

        public WaterAlchemistBoss(Serial serial) : base(serial)
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
