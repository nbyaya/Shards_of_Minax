using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a cannibal boss's corpse")]
    public class CannibalBoss : Cannibal
    {
        [Constructable]
        public CannibalBoss() : base()
        {
            Name = "Cannibal Boss";
            Title = "the Cannibal Overlord";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increase strength for boss-tier
            SetDex(255);  // Max dexterity for fast actions
            SetInt(250);  // Upper range of intelligence

            SetHits(10000); // Boss-tier health
            SetDamage(25, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Boss-level magic resist
            SetSkill(SkillName.Tactics, 120.0); // Increase tactics for better combat
            SetSkill(SkillName.Wrestling, 120.0); // High wrestling for close combat
            SetSkill(SkillName.Magery, 100.0);  // A bit of magery for flair

            Fame = 22500; // Higher fame for the boss
            Karma = -22500; // Negative karma for being evil

            VirtualArmor = 70; // Boss-tier armor

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
            // Additional boss logic can be placed here if necessary
        }

        public CannibalBoss(Serial serial) : base(serial)
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
