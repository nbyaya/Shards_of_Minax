using System;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of Dog the Bounty Hunter - Boss")]
    public class DogtheBountyHunterBoss : DogtheBountyHunter
    {
        [Constructable]
        public DogtheBountyHunterBoss() : base()
        {
            Name = "Dog the Bounty Hunter - Boss";
            Title = "the Ultimate Hunter";

            // Enhance stats to match or exceed Barracoon
            SetStr(1200); // Higher strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Keep the intelligence at a high level

            SetHits(10000); // Increased health for the boss tier

            SetDamage(25, 40); // Increased damage range

            // Enhance resistances to make the boss more resilient
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Keeping poison resistance high
            SetResistance(ResistanceType.Energy, 60, 80);

            // Increase skills to make it more challenging
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 25000; // Higher fame
            Karma = -25000; // Negative karma

            VirtualArmor = 80; // Increased virtual armor for higher damage mitigation

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls along with regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic could be placed here (e.g., summoning more allies)
        }

        public DogtheBountyHunterBoss(Serial serial) : base(serial)
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
