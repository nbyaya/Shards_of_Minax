using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frost hen overlord corpse")]
    public class FrostHenBoss : FrostHen
    {
        [Constructable]
        public FrostHenBoss() : base()
        {
            Name = "Frost Hen Overlord";
            Title = "the Ice Tyrant";
            Hue = 1378; // Icy blue hue

            // Update stats to match or exceed Barracoon's (or superior where needed)
            SetStr(1200); // Increased strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Increased health
            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80); // Increased resistance
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0); // Boosted EvalInt for a more powerful magic attack
            SetSkill(SkillName.Magery, 100.0); // Boosted Magery
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0); // Increased resist magic skill
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0); // Boosted wrestling for better combat

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false; // No longer tamable for the boss version
            ControlSlots = 3;


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
            // Optionally, you could add more advanced logic here for the boss's AI
        }
		
        public FrostHenBoss(Serial serial)
            : base(serial)
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
