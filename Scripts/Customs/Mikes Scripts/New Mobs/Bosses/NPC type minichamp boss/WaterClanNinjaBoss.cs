using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the water clan boss ninja")]
    public class WaterClanNinjaBoss : WaterClanNinja
    {
        [Constructable]
        public WaterClanNinjaBoss() : base()
        {
            Name = "Water Clan Boss Ninja";
            Title = "the Tsunami's Wrath";

            // Update stats to match or exceed the original ninja, adjusting for a boss fight
            SetStr(750);  // Higher Strength for a more powerful boss
            SetDex(650);  // Higher Dexterity to be more agile
            SetInt(700);  // Higher Intelligence for more spellcasting potential
            SetHits(15000);  // Higher health for a boss fight

            SetDamage(125, 175);  // Higher damage output for a stronger boss

            SetResistance(ResistanceType.Physical, 75, 85);  // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 75, 90);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);  // Higher skill levels
            SetSkill(SkillName.Fencing, 180.0, 200.0);
            SetSkill(SkillName.Macing, 180.0, 200.0);
            SetSkill(SkillName.MagicResist, 180.0, 200.0);
            SetSkill(SkillName.Swords, 180.0, 200.0);
            SetSkill(SkillName.Tactics, 180.0, 200.0);
            SetSkill(SkillName.Wrestling, 180.0, 200.0);
            SetSkill(SkillName.Ninjitsu, 180.0, 200.0);
            SetSkill(SkillName.Stealth, 180.0, 200.0);
            SetSkill(SkillName.Hiding, 180.0, 200.0);

            Fame = 25000;  // Higher fame to signify its boss status
            Karma = -25000;  // Negative karma for being a boss

            VirtualArmor = 100;  // Higher armor for greater protection

            // Attach a random ability for extra challenge
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

        public WaterClanNinjaBoss(Serial serial) : base(serial)
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
