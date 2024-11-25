using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of Lord British, the Overlord")]
    public class LordBritishSummonerBoss : LordBritishSummoner
    {
        [Constructable]
        public LordBritishSummonerBoss() : base()
        {
            Name = "Lord British, the Overlord";
            Title = "the Supreme Summoner";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching Barracoon's upper strength or better
            SetDex(255); // Higher dexterity for faster actions
            SetInt(250); // Increased intelligence for spellcasting

            SetHits(12000); // Matching Barracoon's health
            SetDamage(25, 40); // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // Higher physical damage resistance
            SetDamageType(ResistanceType.Fire, 30); // Fire damage resistance
            SetDamageType(ResistanceType.Energy, 30); // Energy damage resistance

            SetResistance(ResistanceType.Physical, 75); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60); // Moderate cold resistance
            SetResistance(ResistanceType.Poison, 100); // Poison resistance
            SetResistance(ResistanceType.Energy, 50); // Energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Improved magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics for better combat
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.EvalInt, 120.0); // Enhanced intelligence evaluation skill
            SetSkill(SkillName.Magery, 120.0); // Higher magery skill for spellcasting

            Fame = 22500;
            Karma = -22500; // Negative karma for an evil boss character

            VirtualArmor = 80; // Stronger virtual armor for better survivability

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic can be added here
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional loot can be added here if needed, or custom items
            PackGold(1000, 1500);
        }

        public LordBritishSummonerBoss(Serial serial) : base(serial)
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
