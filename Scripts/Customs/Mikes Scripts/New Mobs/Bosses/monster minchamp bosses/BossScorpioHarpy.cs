using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss scorpio harpy corpse")]
    public class BossScorpioHarpy : ScorpioHarpy
    {
        [Constructable]
        public BossScorpioHarpy()
        {
            Name = "Scorpio Harpy";
            Title = "the Overlord";
            Hue = 2069; // Keep the same hue or modify as needed to represent boss status
            Body = 30; // Harpy body, remains the same
            BaseSoundID = 402; // Harpy sound

            // Enhanced stats
            SetStr(1200, 1500); // Higher strength
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 50); // Increased damage

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills
            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 24000; // Keep the same fame
            Karma = -24000; // Keep the same karma

            VirtualArmor = 120; // Enhanced armor

            Tamable = false; // Not tamable for a boss
            ControlSlots = 0; // No control slots
            MinTameSkill = 0; // Not tamable


            // Attach random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You will bow before me, mortal!");
            PackGold(2000, 3000); // Enhanced loot
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More iron ingots for the boss
            // Add 5 MaxxiaScrolls to the loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossScorpioHarpy(Serial serial) : base(serial)
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
