using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the enlightened hoplite")]
    public class HippieHopliteBoss : HippieHoplite
    {
        [Constructable]
        public HippieHopliteBoss() : base()
        {
            Name = "Enlightened Hoplite";
            Title = "the Advocate of Harmony";

            // Update stats to match or exceed Barracoon's boss-level stats
            SetStr(1200); // High strength for a boss-level challenge
            SetDex(255); // High dexterity for agility and combat effectiveness
            SetInt(250); // Enhanced intelligence for better magic resistance

            SetHits(10000); // Higher health for a boss encounter

            SetDamage(25, 40); // Increased damage for a tougher challenge

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 100); // Maintain high poison resistance
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance for a boss fight
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 70; // Increased armor for the boss

            // Attach the XmlRandomAbility to give the boss a random special ability
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

        public HippieHopliteBoss(Serial serial) : base(serial)
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
