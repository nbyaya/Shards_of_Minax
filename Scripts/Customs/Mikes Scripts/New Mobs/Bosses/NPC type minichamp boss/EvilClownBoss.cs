using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the evil clown overlord")]
    public class EvilClownBoss : EvilClown
    {
        [Constructable]
        public EvilClownBoss() : base()
        {
            Name = "Evil Clown Overlord";
            Title = "the Terrifying";

            // Update stats to match or exceed Barracoon-like levels
            SetStr(1200); // Matching or surpassing the high strength
            SetDex(255); // Max dexterity for high speed
            SetInt(250); // High intelligence

            SetHits(12000); // High health similar to Barracoon
            SetDamage(29, 38); // Adjust damage to match high-tier bosses

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80); // Increased resistance for survival
            SetResistance(ResistanceType.Fire, 80, 90);    // Increased resistance to fire
            SetResistance(ResistanceType.Cold, 60, 80);    // Better resistance to cold
            SetResistance(ResistanceType.Poison, 100);    // Full poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Improved resistance to energy

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased Magic Resist for boss-level NPC
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 20000; // Increased fame to reflect its boss status
            Karma = -20000; // Negative karma for an evil boss

            VirtualArmor = 80; // Increased armor to withstand more damage

            // Attach the XmlRandomAbility for dynamic abilities
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

        public EvilClownBoss(Serial serial) : base(serial)
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
