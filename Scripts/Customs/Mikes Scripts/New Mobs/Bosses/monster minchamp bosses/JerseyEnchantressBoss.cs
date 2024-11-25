using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a jersey enchantress corpse")]
    public class JerseyEnchantressBoss : JerseyEnchantress
    {
        [Constructable]
        public JerseyEnchantressBoss()
            : base()
        {
            Name = "Jersey Enchantress Overlord";
            Title = "the Supreme Enchantress";
            
            // Update stats to match or exceed Barracoon's (or better) stats
            SetStr(1200); // Upper strength from Barracoon
            SetDex(255); // Upper dexterity from Barracoon
            SetInt(750); // Upper intelligence from Barracoon
            
            SetHits(12000); // Boss health
            SetDamage(38, 48); // Higher damage than the original

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher karma

            VirtualArmor = 100; // Enhanced armor

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 100.0;

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

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public JerseyEnchantressBoss(Serial serial)
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
