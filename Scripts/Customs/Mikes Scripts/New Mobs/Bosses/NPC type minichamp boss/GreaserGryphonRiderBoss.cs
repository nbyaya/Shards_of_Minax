using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the soaring overlord")]
    public class GreaserGryphonRiderBoss : GreaserGryphonRider
    {
        [Constructable]
        public GreaserGryphonRiderBoss() : base()
        {
            Name = "Soaring Overlord";
            Title = "the Sky Tyrant";

            // Enhance stats to match or exceed the best values (Baracoon-like)
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Maximum intelligence

            SetHits(10000); // Boss-tier health
            SetStam(300);   // Boss-tier stamina
            SetMana(750);   // Boss-tier mana

            SetDamage(40, 50); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Archery, 120.0); // Enhanced skill

            Fame = 22500;  // Adjusted fame
            Karma = -22500; // Adjusted karma

            VirtualArmor = 80; // Boss armor value

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

        public GreaserGryphonRiderBoss(Serial serial) : base(serial)
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
