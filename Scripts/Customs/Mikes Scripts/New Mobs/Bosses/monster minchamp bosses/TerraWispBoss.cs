using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a terra wisp boss corpse")]
    public class TerraWispBoss : TerraWisp
    {
        private DateTime m_NextNatureWrath;
        private DateTime m_NextEarthCloak;
        private bool m_IsCloaked;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TerraWispBoss() : base()
        {
            Name = "Terra Wisp Overlord";
            Title = "the Earthshaker";

            // Update stats to match or exceed Barracoon-level values
            SetStr(300, 400); // Much stronger than the regular Terra Wisp
            SetDex(150, 180);
            SetInt(200, 300);

            SetHits(2000, 3000); // Significantly more health
            SetDamage(25, 40); // Stronger damage range

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            VirtualArmor = 50; // Slightly more armor for a tankier boss

            Fame = 15000; // Higher fame value
            Karma = -15000; // Higher negative karma for the boss

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add the 5 MaxxiaScrolls to loot
            this.GenerateLoot();
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional loot or drops can be added here
        }

        public TerraWispBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsCloaked);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsCloaked = reader.ReadBool();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
