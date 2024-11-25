using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the disco overlord")]
    public class DiscoDruidBoss : DiscoDruid
    {
        [Constructable]
        public DiscoDruidBoss() : base()
        {
            Name = "Disco Overlord";
            Title = "the Supreme Groovy Druid";

            // Update stats to match or exceed Barracoon's
            SetStr(425); // Strength increased to match the boss level
            SetDex(255); // Dexterity increased to match the boss level
            SetInt(250); // Intelligence increased to match the boss level

            SetHits(12000); // Increased health to match the boss tier
            SetDamage(29, 38); // Increased damage to match the boss tier

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 100.0, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Enhanced skill values
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 22500; // Adjusted fame to be a high-tier boss
            Karma = -22500; // Adjusted karma for a villainous boss

            VirtualArmor = 70; // Increased armor value

            // Attach a random ability to the boss
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

        public DiscoDruidBoss(Serial serial) : base(serial)
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
