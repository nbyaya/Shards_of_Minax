using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the slime mage overlord")]
    public class SlimeMageBoss : SlimeMage
    {
        [Constructable]
        public SlimeMageBoss() : base()
        {
            Name = "Slime Mage Overlord";
            Title = "the Supreme Sorcerer";

            // Update stats to match or exceed Barracoon (or better if required)
            SetStr(1200); // Max strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Max health
            SetDamage(25, 40); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Meditation, 60.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 75;

            // Attach the XmlRandomAbility for dynamic special abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can go here, such as special behaviors or spells
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

        public SlimeMageBoss(Serial serial) : base(serial)
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
