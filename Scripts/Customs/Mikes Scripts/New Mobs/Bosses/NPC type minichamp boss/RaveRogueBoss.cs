using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the techno overlord")]
    public class RaveRogueBoss : RaveRogue
    {
        [Constructable]
        public RaveRogueBoss() : base()
        {
            Name = "Techno Overlord";
            Title = "the Supreme Rogue";

            // Update stats to make it a boss
            SetStr(1200); // Max strength for a tough boss
            SetDex(255);  // Max dexterity for agility and combat effectiveness
            SetInt(250);  // High intelligence to be more challenging

            SetHits(12000); // High health to make the boss more resilient

            SetDamage(40, 60); // High damage for a boss fight

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance for added challenge
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Fencing, 110.0); // Higher skills for a more dangerous boss
            SetSkill(SkillName.Archery, 110.0);

            Fame = 25000; // High fame for a boss-tier character
            Karma = -25000; // Negative karma for a villainous boss

            VirtualArmor = 80; // Higher virtual armor for increased survivability

            // Attach a random ability for added unpredictability
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
            // Add any extra logic for the boss behavior here (e.g., special moves or strategies)
        }

        public RaveRogueBoss(Serial serial) : base(serial)
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
