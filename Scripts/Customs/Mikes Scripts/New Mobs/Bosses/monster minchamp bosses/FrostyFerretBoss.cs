using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frosty ferret boss corpse")]
    public class FrostyFerretBoss : FrostyFerret
    {
        [Constructable]
        public FrostyFerretBoss() : base()
        {
            Name = "Frosty Ferret, the Frost Lord";
            Title = "the Frozen Overlord";
            Hue = 1575; // Slightly darker blue for the boss version
            BaseSoundID = 0xCF;

            // Update stats to match or exceed the original NPC's performance, plus enhancing them
            SetStr(1200, 1500); // Higher strength than original
            SetDex(255, 300);   // Higher dexterity for faster movements and attacks
            SetInt(250, 350);   // More intelligence for better magical defense and attacks
            
            SetHits(12000); // Higher health for a boss-tier NPC
            SetDamage(35, 50); // Increased damage for more challenge

            SetResistance(ResistanceType.Physical, 80, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 105.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame to match its boss tier status
            Karma = -30000;

            VirtualArmor = 100;

            Tamable = false; // Boss creatures should not be tamable
            ControlSlots = 0; // Not tamable
            MinTameSkill = 100.0; // Not tamable anyway


            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Extra boss logic could be added here if needed
        }

        public FrostyFerretBoss(Serial serial) : base(serial)
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
