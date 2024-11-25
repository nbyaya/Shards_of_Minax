using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme space pirate")]
    public class PirateOfTheStarsBoss : PirateOfTheStars
    {
        [Constructable]
        public PirateOfTheStarsBoss() : base()
        {
            Name = "Supreme Pirate Of The Stars";
            Title = "The Cosmic Tyrant";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Higher than the original, matching the upper limits of Barracoon
            SetDex(255); // Highest dexterity
            SetInt(250); // Upper intelligence as desired

            SetHits(12000); // A powerful amount of health for the boss-tier
            SetDamage(29, 38); // Same as Barracoon or better for an enhanced boss-tier experience

            SetResistance(ResistanceType.Physical, 75); // Higher resistances to make the boss more challenging
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance makes sense for a pirate
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance for survivability
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill for better combat strategies
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill to be more formidable in melee combat

            Fame = 22500; // Increased fame for a boss
            Karma = -22500; // Negative karma for the pirate villain

            VirtualArmor = 80; // Boosted armor value for higher defense

            // Attach the XmlRandomAbility to the boss
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

        public PirateOfTheStarsBoss(Serial serial) : base(serial)
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
