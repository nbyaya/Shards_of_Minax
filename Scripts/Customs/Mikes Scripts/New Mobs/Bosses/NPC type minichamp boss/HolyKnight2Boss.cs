using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the holy knight overlord")]
    public class HolyKnight2Boss : HolyKnight2
    {
        [Constructable]
        public HolyKnight2Boss() : base()
        {
            Name = "Holy Knight Overlord";
            Title = "the Divine Avenger";

            // Enhance stats to match or exceed Barracoon (as needed)
            SetStr(1200); // Improved strength
            SetDex(255);  // Improved dexterity
            SetInt(250);  // Improved intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 70); // Enhanced physical damage resistance
            SetDamageType(ResistanceType.Fire, 30); // Fire damage
            SetDamageType(ResistanceType.Energy, 30); // Energy damage

            SetResistance(ResistanceType.Physical, 75); // Boss-level resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100); // Immune to poison
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resist skill
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Swords, 120.0); // Boosted sword skill
            SetSkill(SkillName.Chivalry, 120.0); // Boosted chivalry skill

            Fame = 22500;  // Boss-level fame
            Karma = 22500; // Positive karma

            VirtualArmor = 80; // Increased virtual armor for tougher defense

            // Attach the random ability XML
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Modify the loot to add 5 MaxxiaScrolls
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
            PackItem(new MaxxiaScroll());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Additional boss loot could be added here if necessary, but the main change is the MaxxiaScroll drop
        }

        public HolyKnight2Boss(Serial serial) : base(serial)
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
