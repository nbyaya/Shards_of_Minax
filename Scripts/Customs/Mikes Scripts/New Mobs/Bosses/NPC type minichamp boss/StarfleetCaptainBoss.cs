using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the starfleet overlord")]
    public class StarfleetCaptainBoss : StarfleetCaptain
    {
        [Constructable]
        public StarfleetCaptainBoss() : base()
        {
            Name = "Starfleet Overlord";
            Title = "the Supreme Commander";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Stronger strength than original
            SetDex(255); // Max dexterity to be more agile
            SetInt(250); // Keep intelligence high but consistent with original

            SetHits(10000); // Boss-level health
            SetDamage(25, 40); // Stronger damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance remains high
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher resist skill
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.Magery, 100.0); // Stronger spell casting

            Fame = 10000;
            Karma = -10000; // Make it more infamous

            VirtualArmor = 80; // Stronger armor

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
            // Additional boss logic could be added here, if needed
        }

        public StarfleetCaptainBoss(Serial serial) : base(serial)
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
