using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss spiderling broodmother corpse")]
    public class BossSpiderlingMinionBroodmother : SpiderlingMinionBroodmother
    {
        [Constructable]
        public BossSpiderlingMinionBroodmother()
            : base()
        {
            Name = "Broodmother";
            Title = " Matriarch";
            Hue = 1786; // Unique hue for a boss
            Body = 28; // Same as original Spiderling Minion Broodmother

            SetStr(1400, 1600); // Increased strength to match boss-level stats
            SetDex(250, 300);
            SetInt(350, 450);

            SetHits(12000, 15000); // Increased health for a boss

            SetDamage(50, 65); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000; // Increased fame for boss-level
            Karma = -25000;

            VirtualArmor = 110; // Enhanced armor

            Tamable = false; // Bosses typically not tamable
            ControlSlots = 5; // Increased control slots for difficulty

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Loot adjustments: Add 5 MaxxiaScrolls when defeated
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("My brood shall overwhelm you!");
            PackGold(1000, 2000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More iron ingots
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for broodling spawning, webs, etc.
        }

        public BossSpiderlingMinionBroodmother(Serial serial)
            : base(serial)
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
