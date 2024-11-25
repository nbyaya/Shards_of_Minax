using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the cabaret kraken overlord")]
    public class CabaretKrakenBoss : CabaretKrakenGirl
    {
        [Constructable]
        public CabaretKrakenBoss() : base()
        {
            Name = "Kraken Overlord";
            Title = "the Abyssal Performer";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Set higher strength than original
            SetDex(255); // Maxed dexterity for higher mobility
            SetInt(750); // High intelligence for magical abilities

            SetHits(12000); // Much higher health than original
            SetDamage(29, 38); // Set high damage range for a boss-tier encounter

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Increase resistances to make it more durable
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics for better combat awareness
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill
            SetSkill(SkillName.Magery, 110.0); // Stronger magery skill for greater offensive capabilities

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased armor for better protection

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

            // Ensure the boss still speaks upon death
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The abyss will remember my performance..."); break;
                case 1: this.Say(true, "The song ends... but not for long!"); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public CabaretKrakenBoss(Serial serial) : base(serial)
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
