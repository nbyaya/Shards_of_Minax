using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a chaneque boss corpse")]
    public class ChanequeBoss : Chaneque
    {
        [Constructable]
        public ChanequeBoss()
            : base()
        {
            Name = "Chaneque Boss";
            Title = "the Forest Overlord";

            // Enhance stats to match or exceed Barracoon's boss stats
            SetStr(1200); // Increased strength for a boss-tier NPC
            SetDex(255); // Max dexterity to ensure high agility
            SetInt(250); // Set intelligence at the higher end for more powerful spells

            SetHits(12000); // Boss-level health
            SetDamage(35, 50); // Increased damage for difficulty

            SetResistance(ResistanceType.Physical, 75, 85); // Improved resistances for the boss
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Max magic resistance for a boss
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for more effective combat
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill
            SetSkill(SkillName.EvalInt, 100.0); // Increased eval int for stronger spells
            SetSkill(SkillName.Magery, 120.0); // Higher magery skill for powerful magical attacks
            SetSkill(SkillName.Meditation, 60.0); // Meditation skill for faster mana regeneration

            Fame = 32000; // Increased fame for a more prestigious boss
            Karma = -32000; // Increased negative karma for a villainous character

            VirtualArmor = 100; // Enhanced armor to make the boss tougher

            Tamable = false; // The boss can't be tamed
            ControlSlots = 0; // No slots required

            // Attach random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls to the boss loot
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

        public ChanequeBoss(Serial serial)
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
