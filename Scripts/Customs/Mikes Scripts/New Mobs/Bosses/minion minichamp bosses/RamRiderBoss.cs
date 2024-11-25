using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ram overlord")]
    public class RamRiderBoss : RamRider
    {
        [Constructable]
        public RamRiderBoss() : base()
        {
            Name = "Ram Overlord";
            Title = "the Supreme Rider";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Upper strength, better than original
            SetDex(200); // Upper dexterity, better than original
            SetInt(75); // Higher intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Boss-level damage

            SetResistance(ResistanceType.Physical, 85); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 70); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 70); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 60); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced skill
            SetSkill(SkillName.Tactics, 120.0); // Enhanced skill
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Enhanced magic resist
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Enhanced wrestling skill

            Fame = 22500; // Higher fame
            Karma = -22500; // Higher karma

            VirtualArmor = 75; // Enhanced armor

            // Attach the XmlRandomAbility to add randomness to the NPC's abilities
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

        public RamRiderBoss(Serial serial) : base(serial)
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
