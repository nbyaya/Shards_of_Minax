using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Starfleet commander")]
    public class StarfleetCommander : StarfleetOfficer
    {
        [Constructable]
        public StarfleetCommander() : base()
        {
            Name = "Starfleet Commander";
            Title = "the Supreme Officer";

            // Update stats to match or exceed Barracoon's level
            SetStr(1200); // Boss-level strength
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(10000); // Increased health for boss-tier NPC
            SetStam(400); // Increased stamina
            SetMana(750); // Increased mana

            SetDamage(50, 70); // Increased damage for boss-level power

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Stronger magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Master tactics
            SetSkill(SkillName.Wrestling, 120.0); // Master wrestling skill

            Fame = 25000; // Higher fame for boss
            Karma = -25000; // Negative karma for the boss

            VirtualArmor = 80; // Increased armor for more defense

            // Attach the random ability feature
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
            // Additional boss logic (e.g., enhanced speech, more attacks)
        }

        public StarfleetCommander(Serial serial) : base(serial)
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
