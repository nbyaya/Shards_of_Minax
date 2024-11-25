using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the insane robotic overlord")]
    public class InsaneRoboticistBoss : InsaneRoboticist
    {
        [Constructable]
        public InsaneRoboticistBoss() : base()
        {
            Name = "Insane Roboticist Overlord";
            Title = "the Supreme Creator";

            // Update stats to match or exceed the boss level
            SetStr(1200); // Matching the upper end of Barracoon's strength range
            SetDex(255); // Max dexterity for agility and combat performance
            SetInt(250); // High intelligence for powerful magic and tactics

            SetHits(12000); // Set the health comparable to Barracoon's strength
            SetDamage(25, 40); // Higher damage for the boss-level challenge

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            // Enhanced resistances to make the boss more formidable
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            // Increased skill values
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 22500; // Boss fame, much higher than a standard mob
            Karma = -22500; // Villainous reputation

            VirtualArmor = 70; // Stronger armor for better defense

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
            
            // Additional loot can be specified here, like rare crafting materials
            PackItem(new IronIngot(10)); // More scrap materials
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic for summons or other behaviors could go here
        }

        public InsaneRoboticistBoss(Serial serial) : base(serial)
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
