using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme drone master")]
    public class MechanicBoss : Mechanic
    {
        [Constructable]
        public MechanicBoss() : base()
        {
            Name = "Supreme Drone Master";
            Title = "the Overlord of Drones";

            // Update stats to match or exceed Barracoon
            SetStr(800, 1000); // Higher strength for a boss
            SetDex(250, 300); // Increased dexterity for better agility
            SetInt(400, 600); // Higher intelligence for stronger magic abilities

            SetHits(12000); // Boss-tier health
            SetDamage(30, 45); // Stronger damage range for the boss

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100; // Boss-level virtual armor

            // Attach a random ability
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
            // Any additional boss logic can be added here
        }

        public MechanicBoss(Serial serial) : base(serial)
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
