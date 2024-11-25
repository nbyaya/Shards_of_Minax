using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the storm overlord")]
    public class StormConjurerBoss : StormConjurer
    {
        [Constructable]
        public StormConjurerBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Supreme Conjurer";

            // Update stats to match or exceed Barracoon-like levels
            SetStr(700); // Higher strength for the boss version
            SetDex(200); // Higher dexterity for faster response
            SetInt(400); // Higher intelligence for better spellcasting

            SetHits(12000); // High health
            SetDamage(20, 35); // Higher damage range to reflect boss status

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 80, 100);

            SetSkill(SkillName.EvalInt, 110.0, 120.0); // Improved skill levels
            SetSkill(SkillName.Magery, 115.0, 120.0); 
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0); 
            SetSkill(SkillName.Tactics, 80.0, 100.0); 
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Negative karma, for a villainous boss

            VirtualArmor = 60; // Higher armor for a boss-tier NPC

            // Attach the random ability
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
            // Additional boss logic or more frequent storm summons could be added here
        }

        public StormConjurerBoss(Serial serial) : base(serial)
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
