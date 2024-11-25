using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a phantom vines corpse")]
    public class PhantomVinesBoss : PhantomVines
    {
        [Constructable]
        public PhantomVinesBoss()
            : base("Phantom Vines Overlord")
        {
            // Enhanced stats based on original PhantomVines and Barracoon
            SetStr(1200); // Enhanced strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence
            
            SetHits(12000); // Increased health for boss
            SetDamage(40, 50); // Increased damage for boss

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 110.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 30000;  // Increased fame for boss-tier NPC
            Karma = -30000; // Increased negative karma

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false;  // Boss should not be tamable
            ControlSlots = 3;  // Just like the original, not tamable for players
            MinTameSkill = 93.9;

            // Attach the XmlRandomAbility
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
            
            // Additional logic could be added for the boss (special phase changes, etc.)
        }

        public PhantomVinesBoss(Serial serial) : base(serial)
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
