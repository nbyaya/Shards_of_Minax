using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the scorpomancer overlord")]
    public class ScorpomancerBoss : Scorpomancer
    {
        [Constructable]
        public ScorpomancerBoss() : base()
        {
            Name = "Scorpomancer Overlord";
            Title = "the Supreme Scorpomancer";

            // Update stats to match or exceed Barracoon's stats for boss difficulty
            SetStr(1200);  // Higher strength for a boss-tier creature
            SetDex(255);   // Max dexterity for quicker actions
            SetInt(250);   // Increased intelligence for stronger magic

            SetHits(12000); // High health like a boss
            SetDamage(29, 38); // Matching Barracoon's damage range, making it more dangerous

            SetResistance(ResistanceType.Physical, 75, 80); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Full poison immunity
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 120.0); // Higher EvalInt for more powerful magic
            SetSkill(SkillName.Magery, 120.0);  // More powerful spellcasting
            SetSkill(SkillName.Meditation, 50.0); 
            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance for a tough boss
            SetSkill(SkillName.Tactics, 100.0); // Higher tactics for better combat
            SetSkill(SkillName.Wrestling, 100.0); // Better wrestling skill for close combat

            Fame = 22500; // Increase Fame for a boss
            Karma = -22500; // Negative Karma for evil boss

            VirtualArmor = 70; // Higher armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Set team to a unique value to prevent confusion with normal Scorpomancer

        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add richer loot for a boss NPC
            PackGold(500, 700); // More gold
            AddLoot(LootPack.UltraRich); // Ultra-rich loot pack to make it more rewarding
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be implemented here if needed
        }

        public ScorpomancerBoss(Serial serial) : base(serial)
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
