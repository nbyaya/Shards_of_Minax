using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sinister alchemist overlord")]
    public class EvilAlchemistBoss : EvilAlchemist
    {
        [Constructable]
        public EvilAlchemistBoss() : base()
        {
            Name = "Evil Alchemist Overlord";
            Title = "the Supreme Alchemist";

            // Update stats to match or exceed Barracoon for a boss-tier version
            SetStr(1200);  // Stronger than the original
            SetDex(255);   // Maximum dexterity
            SetInt(250);   // Keep intelligence on par with original

            SetHits(12000); // Much higher health than the original
            SetDamage(25, 35); // Slightly enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // Enhanced Magery

            Fame = 22500;  // Increased Fame for a boss
            Karma = -22500; // Increased negative Karma for a stronger boss

            VirtualArmor = 80; // Better armor to match a boss-tier creature

            // Attach a random ability to the boss
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
            // Additional boss logic or speech could be added here
        }

        public EvilAlchemistBoss(Serial serial) : base(serial)
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
