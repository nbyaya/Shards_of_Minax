using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a tarantula warrior boss corpse")]
    public class TarantulaWorriorBoss : TarantulaWarrior
    {
        [Constructable]
        public TarantulaWorriorBoss() : base()
        {
            Name = "Tarantula Warrior";
            Title = "the Supreme";
            Hue = 1153; // Unique hue for the boss version
            BaseSoundID = 0x388; // Same sound as original TarantulaWarrior

            SetStr(1200, 1500); // Enhanced strength for the boss
            SetDex(250, 300);   // Enhanced dexterity
            SetInt(350, 450);   // Enhanced intelligence

            SetHits(12000); // Increased health for boss level
            SetDamage(40, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 75, 85);     // Increased fire resistance
            SetResistance(ResistanceType.Cold, 65, 75);     // Increased cold resistance
            SetResistance(ResistanceType.Poison, 80, 90);   // Increased poison resistance
            SetResistance(ResistanceType.Energy, 50, 60);   // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0);  // Boss-level resist
            SetSkill(SkillName.Tactics, 130.0);      // Enhanced tactics
            SetSkill(SkillName.Wrestling, 130.0);    // Enhanced wrestling skill
            SetSkill(SkillName.Meditation, 50.0);    // Some meditation skill

            Fame = 30000; // Increased fame for boss
            Karma = -30000; // Negative karma for a boss mob

            VirtualArmor = 100; // Boss-level virtual armor

            Tamable = false; // Not tamable
            ControlSlots = 0; // Not tamable, so no control slots

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot of 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Inherit base loot generation

            // Add additional loot for the boss
            PackGold(2000, 3000); // High gold reward
            AddLoot(LootPack.FilthyRich, 3); // More rich loot
            AddLoot(LootPack.Rich); // Standard rich loot
            AddLoot(LootPack.Gems, 10); // More gems

            this.Say("You dare challenge the Tarantula Warrior?");
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the original behavior
        }

        public TarantulaWorriorBoss(Serial serial) : base(serial)
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
