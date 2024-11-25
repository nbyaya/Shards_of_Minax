using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sidhe corpse")]
    public class SidheBoss : Sidhe
    {
        private DateTime m_NextRoyalAura;
        private DateTime m_NextTimeStop;
        private DateTime m_NextMirrorImage;

        [Constructable]
        public SidheBoss() : base()
        {
            Name = "Sidhe Overlord";
            Title = "the Supreme Sidhe";
            Hue = 1581; // Slightly different hue to mark it as a boss-tier NPC

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80); // Enhanced resistance
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Increased Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Increased Tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Increased Wrestling skill

            Fame = 30000; // Increased Fame
            Karma = -30000; // Increased Karma (negative for the boss-tier nature)

            VirtualArmor = 100; // Enhanced virtual armor

            Tamable = false; // The Sidhe boss cannot be tamed
            ControlSlots = 0; // Not tamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Add 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here
        }

        public SidheBoss(Serial serial) : base(serial)
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
