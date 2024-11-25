using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss Virgo Harpy corpse")]
    public class BossVirgoHarpy : VirgoHarpy
    {
        [Constructable]
        public BossVirgoHarpy() : base()
        {
            Name = "Virgo Harpy";
            Title = "the Celestial Fury";
            Hue = 2066; // Elegant, pristine white feathers (same as original, can change if desired)
            Body = 30; // Harpy body
            BaseSoundID = 402; // Harpy sound

            // Enhance stats for boss
            SetStr(1200, 1500);
            SetDex(255, 300);
            SetInt(250, 350);

            SetHits(15000); // Increased health for boss
            SetDamage(40, 50); // Increased damage for boss

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Negative karma for a boss

            VirtualArmor = 100; // Increased armor

            // Attach random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("Feel the wrath of the celestial harpy!");
            PackGold(1500, 2000); // Enhanced loot
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities and random intervals for powers
        }

        public BossVirgoHarpy(Serial serial) : base(serial)
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
