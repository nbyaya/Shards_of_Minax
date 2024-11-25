using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Turkish Angora Enchanter Boss corpse")]
    public class TurkishAngoraEnchanterBoss : TurkishAngoraEnchanter
    {
        [Constructable]
        public TurkishAngoraEnchanterBoss() : base()
        {
            Name = "Turkish Angora Enchanter Boss";
            Title = "the Supreme Enchanter";

            // Update stats to match or exceed Barracoon's or better stats
            SetStr(1200); // Higher strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 75); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 50); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance skill
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

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

            // Additional boss logic could be added here if needed
        }

        public TurkishAngoraEnchanterBoss(Serial serial) : base(serial)
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
