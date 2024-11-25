using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the musical overlord")]
    public class JazzAgeJuggernautBoss : JazzAgeJuggernaut
    {
        [Constructable]
        public JazzAgeJuggernautBoss() : base()
        {
            Name = "Musical Overlord";
            Title = "the Jazz Master";

            // Enhanced stats to match or exceed a boss's capabilities
            SetStr(1200); // Higher strength than before
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Boss-level health
            SetDamage(50, 70); // Boss-level damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Stronger magic resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 25000; // Higher fame for a boss-level creature
            Karma = -25000; // Negative karma for a boss enemy

            VirtualArmor = 75; // Higher virtual armor for extra protection

            // Attach a random ability to enhance the boss further
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
            // Additional boss logic for summons or other behaviors could go here
        }

        public JazzAgeJuggernautBoss(Serial serial) : base(serial)
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
