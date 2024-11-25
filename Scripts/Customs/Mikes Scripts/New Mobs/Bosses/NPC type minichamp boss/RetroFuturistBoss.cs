using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the retrofuturistic overlord")]
    public class RetroFuturistBoss : RetroFuturist
    {
        [Constructable]
        public RetroFuturistBoss() : base()
        {
            Name = "Retrofuturistic Overlord";
            Title = "the Visionary";

            // Update stats to match or exceed Barracoon's level of strength
            SetStr(1200); // Increased to a higher value than before
            SetDex(255); // Maximum dexterity for higher speed
            SetInt(250); // Higher intelligence for increased spellcasting

            SetHits(12000); // High health to make the boss formidable
            SetDamage(25, 40); // Increase damage range for greater threat

            // Enhanced resistance values
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Update skills for a more dangerous boss-tier NPC
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            // Enhanced fame and karma
            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80; // Increased virtual armor for added protection

            // Attach the XmlRandomAbility for random powerups
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

            // Add additional loot fitting for a boss-tier NPC
            PackGold(500, 1000);
            AddLoot(LootPack.Rich);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss behavior could be added here, like casting powerful spells
        }

        public RetroFuturistBoss(Serial serial) : base(serial)
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
