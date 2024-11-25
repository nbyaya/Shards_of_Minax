using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the mushroom witch overlord")]
    public class MushroomWitchBoss : MushroomWitch
    {
        [Constructable]
        public MushroomWitchBoss() : base()
        {
            Name = "Mushroom Witch Overlord";
            Title = "the Supreme Sorceress";

            // Update stats to match or exceed Barracoon's tier
            SetStr(1200); // Increased strength for boss
            SetDex(255); // Maximized dexterity
            SetInt(250); // Maximized intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80); // Increased resistance
            SetResistance(ResistanceType.Fire, 80, 90); 
            SetResistance(ResistanceType.Cold, 60, 75); 
            SetResistance(ResistanceType.Poison, 100); // Maintained max poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); 

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced Magic Resist
            SetSkill(SkillName.EvalInt, 110.0); // Increased EvalInt for better damage
            SetSkill(SkillName.Magery, 120.0); // Increased magery
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.Tactics, 110.0); // Increased tactics for better combat
            SetSkill(SkillName.Wrestling, 110.0); // Increased wrestling for defense

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased negative karma for a boss tier creature

            VirtualArmor = 75; // Improved armor for boss tier

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
            // Additional boss logic or special speech/abilities can be added here if desired
        }

        public MushroomWitchBoss(Serial serial) : base(serial)
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
