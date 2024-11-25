using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("boss ember axis corpse")]
    public class BossEmberAxis : EmberAxis
    {
        [Constructable]
        public BossEmberAxis() : base()
        {
            Name = "Ember Axis";
            Title = "the Infernal Warden"; // Title for the boss
            Hue = 0x497; // Unique hue for the boss
            Body = 0xEA; // Use the same body as Ember Axis

            // Enhanced stats to match or exceed Barracoon's stats
            SetStr(1200); // Higher strength
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(15000); // Boss-level health
            SetDamage(35, 50); // Higher damage range

            SetResistance(ResistanceType.Physical, 75); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80); // Strong fire resistance
            SetResistance(ResistanceType.Cold, 60); // Slightly higher cold resistance
            SetResistance(ResistanceType.Poison, 80); // Strong poison resistance
            SetResistance(ResistanceType.Energy, 60); // Strong energy resistance

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // Boss can't be tamed
            ControlSlots = 0; // No control slots needed for bosses

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain base loot
            this.Say("I will burn you all!");
            PackGold(2000, 3000); // Enhanced gold drop
            AddLoot(LootPack.FilthyRich, 2); // Rich loot pack
            AddLoot(LootPack.Rich); // Rich loot pack
            AddLoot(LootPack.Gems, 8); // Enhanced gems drop
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original ability logic (FlameTrail, InfernoCharge, EmberBurst)
        }

        public BossEmberAxis(Serial serial) : base(serial)
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
