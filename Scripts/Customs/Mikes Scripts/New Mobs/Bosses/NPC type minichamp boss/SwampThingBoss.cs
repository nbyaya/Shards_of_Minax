using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the swamp overlord")]
    public class SwampThingBoss : SwampThing
    {
        [Constructable]
        public SwampThingBoss() : base()
        {
            Name = "Swamp Overlord";
            Title = "the Swamp Lord";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increase strength for boss-tier challenge
            SetDex(255); // Max dexterity for agility
            SetInt(250); // Intelligence for spellcasting

            SetHits(10000); // Boss-tier health
            SetDamage(25, 40); // Increase damage range for boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Higher combat tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.Magery, 120.0); // Boss-tier magery skill

            Fame = 15000; // Increased fame
            Karma = -15000; // Increased negative karma

            VirtualArmor = 85; // Increased virtual armor

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

            // Additional boss logic can be added here if necessary
        }

        public SwampThingBoss(Serial serial) : base(serial)
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
