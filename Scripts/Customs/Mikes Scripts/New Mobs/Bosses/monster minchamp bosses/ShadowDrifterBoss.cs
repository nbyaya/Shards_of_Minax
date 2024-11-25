using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow drifter boss corpse")]
    public class ShadowDrifterBoss : ShadowDrifter
    {
        [Constructable]
        public ShadowDrifterBoss() : base()
        {
            Name = "Shadow Drifter Overlord";
            Title = "the Eternal Drifter";

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Upper limit of Barracoon's strength
            SetDex(255);  // Upper limit of Barracoon's dexterity
            SetInt(750);  // Upper limit of Barracoon's intelligence

            SetHits(12000); // Boss-level health
            SetDamage(35, 45); // Higher damage range for a boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma loss

            VirtualArmor = 100;

            // Attach the XmlRandomAbility
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
            // Additional boss logic can be added here (if needed)
        }

        public ShadowDrifterBoss(Serial serial) : base(serial)
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
