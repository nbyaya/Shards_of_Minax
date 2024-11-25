using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the fallen queen")]
    public class HostilePrincessBoss : HostilePrincess
    {
        [Constructable]
        public HostilePrincessBoss() : base()
        {
            Name = "Queen of Hostility";
            Title = "the Supreme Princess";

            // Update stats to match or exceed Barracoon (or equivalent)
            SetStr(1200); // Increased strength for a boss-tier character
            SetDex(255); // Max dexterity
            SetInt(250); // Increase intelligence for higher magical potential

            SetHits(10000); // Much higher health for boss status
            SetDamage(25, 45); // Increased damage range to match a boss

            SetResistance(ResistanceType.Physical, 80, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60, 80); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maintains full poison resistance
            SetResistance(ResistanceType.Energy, 60, 80); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Max magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics skill for better combat
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill for higher physical combat prowess

            Fame = 22500; // Increased fame for boss-tier
            Karma = -22500; // Negative karma for evil character

            VirtualArmor = 70; // Increased virtual armor for better defense

            // Attach the XmlRandomAbility for added boss complexity
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

            // Additional loot for a boss
            PackGold(500, 1000); // Larger gold drop
            PackItem(new Diamond(Utility.RandomMinMax(5, 10))); // More valuable item drops
            AddLoot(LootPack.UltraRich); // Richer loot pack

            // Special phrases after being defeated
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "You might have defeated me, but the crown is mine to claim!"); break;
                case 1: this.Say(true, "This is not my end... you shall regret this!"); break;
            }
        }

        public HostilePrincessBoss(Serial serial) : base(serial)
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
