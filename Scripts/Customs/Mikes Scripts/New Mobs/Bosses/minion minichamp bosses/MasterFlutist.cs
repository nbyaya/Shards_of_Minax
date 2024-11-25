using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master flutist")]
    public class MasterFlutist : Flutist
    {
        [Constructable]
        public MasterFlutist() : base()
        {
            Name = "Master Flutist";
            Title = "the Supreme Melody";

            // Update stats to match or exceed Barracoon for a boss tier NPC
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(200); // Enhanced dexterity
            SetInt(350); // Enhanced intelligence

            SetHits(12000); // Much higher health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Musicianship, 120.0); // Maximum musicianship skill
            SetSkill(SkillName.Provocation, 120.0);  // Enhanced provocation skill
            SetSkill(SkillName.Discordance, 120.0);  // Enhanced discordance skill
            SetSkill(SkillName.Peacemaking, 120.0);  // Enhanced peacemaking skill

            Fame = 22500;  // Higher fame for a boss
            Karma = -22500; // Negative karma for a boss NPC

            VirtualArmor = 70; // Higher armor value

            // Attach random ability
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
            
            // You can modify or add additional behavior for the boss here if desired
        }

        public MasterFlutist(Serial serial) : base(serial)
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
