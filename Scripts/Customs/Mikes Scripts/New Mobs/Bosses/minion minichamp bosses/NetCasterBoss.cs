using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the net overlord")]
    public class NetCasterBoss : NetCaster
    {
        [Constructable]
        public NetCasterBoss() : base()
        {
            Name = "Net Overlord";
            Title = "the Supreme Caster";

            // Enhance stats to match or exceed Barracoon
            SetStr(800); // Increase strength
            SetDex(150); // Maximize dexterity
            SetInt(250); // Increase intelligence

            SetHits(12000); // Match Barracoon's health
            SetDamage(29, 38); // Use Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75); // Boost resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Match Barracoon's fame
            Karma = -22500; // Negative karma, like Barracoon

            VirtualArmor = 70; // Increase virtual armor

            // Attach a random ability (using XmlRandomAbility)
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

        public NetCasterBoss(Serial serial) : base(serial)
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
