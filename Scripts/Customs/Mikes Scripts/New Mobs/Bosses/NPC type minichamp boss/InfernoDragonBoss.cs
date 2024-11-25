using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    public class InfernoDragonBoss : InfernoDragon
    {
        [Constructable]
        public InfernoDragonBoss() : base()
        {
            Name = "Inferno Dragon King";
            Title = "the Flamebringer";

            // Enhance stats to match or exceed the desired boss level
            SetStr(1200); // Enhanced strength
            SetDex(200);  // Enhanced dexterity
            SetInt(600);  // Enhanced intelligence

            SetHits(15000); // Enhanced health for a boss-level challenge
            SetStam(500);   // Enhanced stamina
            SetMana(1000);  // Enhanced mana

            SetDamage(40, 60); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 120.0); // Enhanced skill
            SetSkill(SkillName.Tactics, 130.0);    // Enhanced skill
            SetSkill(SkillName.Wrestling, 130.0);  // Enhanced skill
            SetSkill(SkillName.Meditation, 100.0); // Enhanced skill for mana regen

            Fame = 30000;  // Enhanced fame
            Karma = -30000; // Enhanced karma

            VirtualArmor = 90;  // Enhanced virtual armor

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

        public InfernoDragonBoss(Serial serial) : base(serial) { }

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
