using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the harpist overlord")]
    public class HarpistBoss : Harpist
    {
        [Constructable]
        public HarpistBoss() : base()
        {
            Name = "Harpist Overlord";
            Title = "the Supreme Harpist";

            // Update stats to match or exceed Barracoon (as a baseline for boss power)
            SetStr(600, 800); // Enhance strength
            SetDex(250, 350); // Enhance dexterity
            SetInt(500, 700); // Enhance intelligence

            SetHits(5000, 8000); // Much higher hit points for a boss
            SetDamage(15, 25); // Increased damage range for the boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80); // Stronger resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Musicianship, 120.0); // Increased skills for the boss
            SetSkill(SkillName.Provocation, 120.0);
            SetSkill(SkillName.Peacemaking, 120.0);
            SetSkill(SkillName.Discordance, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Magery, 100.0);

            Fame = 12000; // Increased fame for the boss
            Karma = -12000; // Negative karma for the boss

            VirtualArmor = 70; // Enhanced armor

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
            // Additional boss logic could be added here
        }

        public HarpistBoss(Serial serial) : base(serial)
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
