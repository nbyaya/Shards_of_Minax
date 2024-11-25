using System;
using Server;
using Server.Items;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the saboteur overlord")]
    public class SaboteurBoss : Saboteur
    {
        [Constructable]
        public SaboteurBoss() : base()
        {
            Name = "Saboteur Overlord";
            Title = "the Explosive Master";

            // Update stats to match or exceed Barracoon's and make it a true boss
            SetStr(700, 900); // Enhanced strength for the boss
            SetDex(300, 400); // Enhanced dexterity for the boss
            SetInt(200, 300); // Enhanced intelligence for the boss

            SetHits(10000, 12000); // Increase health significantly
            SetDamage(35, 50); // Increased damage for the boss

            SetResistance(ResistanceType.Physical, 75, 85); // Increase resistances for the boss
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increase skill levels
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500; // Increased fame for the boss
            Karma = -22500; // Increased karma for the boss

            VirtualArmor = 70; // Increased virtual armor for the boss

            // Attach a random ability for added boss complexity
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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if desired
        }

        public SaboteurBoss(Serial serial) : base(serial)
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
