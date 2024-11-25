using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the fire clan overlord")]
    public class FireClanSamuraiBoss : FireClanSamurai
    {
        [Constructable]
        public FireClanSamuraiBoss() : base()
        {
            Name = "Fire Clan Overlord";
            Title = "the Supreme Warrior";

            // Update stats to match or exceed Barracoon's or better stats
            SetStr(850); // Matching or exceeding Barracoon's strength
            SetDex(600); // Increased dexterity for more agility in battle
            SetInt(600); // Increased intelligence to make the boss more formidable

            SetHits(1400); // Match the higher end of FireClanSamurai's health
            SetDamage(120, 140); // Increased damage to make the boss tougher

            SetResistance(ResistanceType.Physical, 75); // Improved resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 75);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.Bushido, 150.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Fencing, 150.0);
            SetSkill(SkillName.Macing, 150.0);
            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Swords, 150.0);
            SetSkill(SkillName.Tactics, 150.0);
            SetSkill(SkillName.Wrestling, 150.0);

            Fame = 25000; // Increased fame for the boss-tier version
            Karma = -25000; // Negative karma for being a more fearsome foe

            VirtualArmor = 90; // Increased armor for extra durability

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override GenerateLoot to add 5 MaxxiaScrolls
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
            // Additional boss logic could be added here, if desired
        }

        public FireClanSamuraiBoss(Serial serial) : base(serial)
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
