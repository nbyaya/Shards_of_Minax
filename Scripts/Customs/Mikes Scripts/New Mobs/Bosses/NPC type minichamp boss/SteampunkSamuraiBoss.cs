using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the clockwork overlord")]
    public class SteampunkSamuraiBoss : SteampunkSamurai
    {
        [Constructable]
        public SteampunkSamuraiBoss() : base()
        {
            Name = "Clockwork Overlord";
            Title = "the Steam Emperor";

            // Update stats to match or exceed Barracoon (or the original samurai's stats)
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Matching Barracoon's upper dexterity
            SetInt(250); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Use an increased stamina value
            SetMana(750); // Use an increased mana value

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75); // Increased resistance
            SetResistance(ResistanceType.Fire, 80); // Increased resistance
            SetResistance(ResistanceType.Cold, 65); // Increased resistance
            SetResistance(ResistanceType.Poison, 100); // Same resistance
            SetResistance(ResistanceType.Energy, 60); // Increased resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma

            VirtualArmor = 70; // Enhanced virtual armor for more defense

            // Attach a random ability for more unpredictability
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

            // Additional boss-specific logic could be added here
        }

        public SteampunkSamuraiBoss(Serial serial) : base(serial)
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
