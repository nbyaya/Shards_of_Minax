using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the earth clan samurai lord")]
    public class EarthClanSamuraiBoss : EarthClanSamurai
    {
        [Constructable]
        public EarthClanSamuraiBoss() : base()
        {
            Name = "Earth Clan Samurai Lord";
            Title = "the Earth Clan Protector Supreme";

            // Update stats to match or exceed the previous boss (Barracoon)
            SetStr(800, 1000); // Increased Strength to match or exceed Barracoon
            SetDex(450, 500); // Increased Dexterity to match Barracoon's stats
            SetInt(400, 600); // Increased Intelligence for more resistance and spellcasting

            SetHits(12000); // Matching Barracoon's health
            SetDamage(120, 150); // Increased damage range

            SetSkill(SkillName.Bushido, 150.0); // Mastery level
            SetSkill(SkillName.Anatomy, 100.0, 150.0);
            SetSkill(SkillName.Fencing, 120.0, 150.0);
            SetSkill(SkillName.Macing, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased Magic Resistance
            SetSkill(SkillName.Swords, 150.0, 200.0); // Increased Sword skill
            SetSkill(SkillName.Tactics, 150.0); // Mastery level for tactics
            SetSkill(SkillName.Wrestling, 150.0); // Increased Wrestling skill

            Fame = 22500; // Increased Fame to match the boss-tier nature
            Karma = 22500; // Increased Karma to reflect their strength

            VirtualArmor = 75; // Improved Armor value for additional toughness

            // Attach a random ability from XmlRandomAbility
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to include 5 MaxxiaScrolls in addition to regular loot
        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Potential additional loot (could be customized further)
            PackGold(1500, 2500); // Increased gold drop
            AddLoot(LootPack.Average); // Adds a variety of average loot
        }

        // Override OnThink for potential additional boss behavior (if needed)
        public override void OnThink()
        {
            base.OnThink();
            // Additional custom behavior for the boss can be added here
        }

        public EarthClanSamuraiBoss(Serial serial) : base(serial)
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
