using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a lurking fear corpse")]
    public class DrolaticBoss : Drolatic
    {
        [Constructable]
        public DrolaticBoss()
            : base()
        {
            Name = "Drolatic the Supreme Fear";
            Title = "the Overlord of Fear";
            
            // Update stats to match or exceed the boss-tier stats
            SetStr(1200); // Higher strength than before
            SetDex(255);  // Max dexterity
            SetInt(250);  // Max intelligence

            SetHits(12000); // High health for a boss
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 120;

            Tamable = false; // Boss creatures are not tamable
            ControlSlots = 0; // Not tamable or controllable by players
            MinTameSkill = 0;

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

        public DrolaticBoss(Serial serial)
            : base(serial)
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
