using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a huntsman spider corpse")]
    public class HuntsmanSpiderBoss : HuntsmanSpider
    {
        [Constructable]
        public HuntsmanSpiderBoss()
            : base()
        {
            Name = "Huntsman Spider Overlord";
            Title = "the Venomous Tyrant";

            // Enhanced stats to match or exceed the original Huntsman Spider
            SetStr(1200); // Max strength for the boss tier
            SetDex(255); // Max dexterity for the boss tier
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health to match a boss tier
            SetDamage(40, 50); // Increased damage to be more challenging

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 35000; // Boss-level fame
            Karma = -35000; // Negative karma for the boss

            VirtualArmor = 100; // Enhanced armor for higher survivability

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
            // Additional boss logic could be added here
        }

        public HuntsmanSpiderBoss(Serial serial)
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
