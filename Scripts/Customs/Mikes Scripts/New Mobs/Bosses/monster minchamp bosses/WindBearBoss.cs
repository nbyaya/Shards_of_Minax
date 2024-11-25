using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a wind overlord corpse")]
    public class WindBearBoss : WindBear
    {
        [Constructable]
        public WindBearBoss() : base()
        {
            Name = "Wind Overlord";
            Title = "the Gale King";

            // Update stats to match or exceed Barracoon-like boss stats
            SetStr(1200); // Stronger than the original WindBear's max strength
            SetDex(255); // Max dexterity, making it agile
            SetInt(250); // Increase intelligence for better spellcasting

            SetHits(12000); // Increased health for a boss-tier NPC

            SetDamage(35, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 110.0);

            Fame = 30000; // Increased fame to reflect the boss tier
            Karma = -30000; // Negative karma for a more fearsome appearance

            VirtualArmor = 100; // Increase armor to make it more difficult to damage

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

        public WindBearBoss(Serial serial) : base(serial)
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
