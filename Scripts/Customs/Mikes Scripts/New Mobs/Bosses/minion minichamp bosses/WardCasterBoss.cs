using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ward overlord")]
    public class WardCasterBoss : WardCaster
    {
        [Constructable]
        public WardCasterBoss() : base()
        {
            Name = "Ward Overlord";
            Title = "the Supreme Ward Caster";

            // Update stats to match or exceed Barracoon (or better where applicable)
            SetStr(700); // Upper range of Barracoon's strength or higher
            SetDex(150); // Upper dexterity from Barracoon
            SetInt(750); // Upper intelligence from Barracoon

            SetHits(12000); // Set high health (matching Barracoon's health)
            SetDamage(29, 38); // Matching Barracoon's damage range

            // Resists enhanced for a tougher fight
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 150.0); // Match or exceed Barracoon's resist
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Enhanced armor value for better defense

            // Attach the XmlRandomAbility to add dynamic behavior
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            
            // Add shield or buff logic if needed for the boss (can be customized)
            if (DateTime.Now >= m_NextShieldTime)
            {
                this.Say(true, "Feel the might of my magic!");
                BuffSelf();
            }
        }

        private void BuffSelf()
        {
            // Implement any special self-buffing abilities (like shielding, speed, etc.)
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

        public WardCasterBoss(Serial serial) : base(serial)
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
