using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ascetic master")]
    public class AsceticHermitBoss : AsceticHermit
    {
        [Constructable]
        public AsceticHermitBoss() : base()
        {
            Name = "Ascetic Master";
            Title = "the Enlightened One";

            // Update stats to match or exceed Barracoon-like levels
            SetStr(600); // Increase strength
            SetDex(150); // Max out dexterity
            SetInt(400); // Max out intelligence

            SetHits(12000); // Match Barracoon's high health

            SetDamage(15, 30); // Increase damage

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            SetResistance(ResistanceType.Physical, 75, 85); // Boost physical resistance
            SetResistance(ResistanceType.Fire, 70, 85); // Boost fire resistance
            SetResistance(ResistanceType.Cold, 70, 85); // Boost cold resistance
            SetResistance(ResistanceType.Poison, 75, 85); // Boost poison resistance
            SetResistance(ResistanceType.Energy, 80, 90); // Boost energy resistance

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 15000; // Increased fame for a boss-tier creature
            Karma = 15000; // Adjust karma to match the higher fame

            VirtualArmor = 80; // Increase virtual armor

            m_NextBuffTime = DateTime.Now + TimeSpan.FromSeconds(15.0); // Faster buff interval
            m_NextAreaDenialTime = DateTime.Now + TimeSpan.FromSeconds(20.0); // Faster area denial interval

            // Attach random ability
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

        public AsceticHermitBoss(Serial serial) : base(serial)
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
