using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the fencing master overlord")]
    public class FencingMasterBoss : FencingMaster
    {
        [Constructable]
        public FencingMasterBoss() : base()
        {
            Name = "Fencing Master Overlord";
            Title = "the Supreme Duelist";

            // Update stats to match or exceed Barracoon as a powerful boss
            SetStr(900); // Matching the upper end of Barracoon's strength
            SetDex(250); // Matching Barracoon's upper dexterity
            SetInt(150); // Keeping this stat similar to the original FencingMaster

            SetHits(12000); // Much higher health for boss-tier
            SetDamage(29, 38); // Same as Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75); // Higher physical resistance for durability
            SetResistance(ResistanceType.Fire, 70); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 65); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 70); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 75); // Higher energy resistance

            SetSkill(SkillName.Fencing, 150.0); // Significantly improved fencing skill
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics
            SetSkill(SkillName.MagicResist, 100.0); // Stronger resistance to magic
            SetSkill(SkillName.Anatomy, 110.0); // Increased anatomy skill

            Fame = 22500; // High fame for a boss
            Karma = -22500; // High negative karma

            VirtualArmor = 80; // Increased virtual armor for added protection

            // Attach the XmlRandomAbility to give it an additional random special power
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

        public FencingMasterBoss(Serial serial) : base(serial)
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
