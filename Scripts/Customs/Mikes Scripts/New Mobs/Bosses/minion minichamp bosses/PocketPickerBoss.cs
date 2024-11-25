using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master pocket picker")]
    public class PocketPickerBoss : PocketPicker
    {
        [Constructable]
        public PocketPickerBoss() : base()
        {
            Name = "Master Pocket Picker";
            Title = "the Supreme Thief";

            // Update stats to match or exceed Barracoon-like levels
            SetStr(500); // Stronger strength
            SetDex(700); // Elite dexterity for quick and evasive movements
            SetInt(200); // High intelligence for greater awareness

            SetHits(1500); // Much higher health than the original
            SetDamage(20, 35); // Increased damage range

            SetResistance(ResistanceType.Physical, 50, 70); // Increased resistances
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Stealing, 100.0); // Perfect skill for a boss-level thief
            SetSkill(SkillName.Hiding, 100.0);
            SetSkill(SkillName.Stealth, 100.0);
            SetSkill(SkillName.Fencing, 90.0);
            SetSkill(SkillName.Tactics, 90.0);

            Fame = 10000; // Elevated fame for a boss NPC
            Karma = -10000; // Negative karma for a villainous boss

            VirtualArmor = 50; // Increased armor for survivability

            // Attach a random ability for more unpredictability
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

        public PocketPickerBoss(Serial serial) : base(serial)
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
