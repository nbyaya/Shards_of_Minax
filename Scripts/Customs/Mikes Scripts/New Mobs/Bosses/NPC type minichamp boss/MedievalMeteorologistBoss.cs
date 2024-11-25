using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the storm overlord")]
    public class MedievalMeteorologistBoss : MedievalMeteorologist
    {
        [Constructable]
        public MedievalMeteorologistBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Tempest Caller";

            // Update stats to match or exceed Barracoon's values
            SetStr( 1200 ); // Exceeds the original, now on par with a boss
            SetDex( 255 ); // Max dexterity to increase agility
            SetInt( 250 ); // Higher intelligence to reflect its mage-like nature

            SetHits( 12000 ); // High health to match a boss-tier character
            SetDamage( 25, 35 ); // Increased damage range

            // Update resistances to match a high-level boss NPC
            SetResistance( ResistanceType.Physical, 75, 85 );
            SetResistance( ResistanceType.Fire, 80, 90 );
            SetResistance( ResistanceType.Cold, 60, 70 );
            SetResistance( ResistanceType.Poison, 100 );
            SetResistance( ResistanceType.Energy, 60, 70 );

            SetSkill( SkillName.MagicResist, 150.0 );
            SetSkill( SkillName.Tactics, 100.0 );
            SetSkill( SkillName.Wrestling, 100.0 );
            SetSkill( SkillName.Magery, 110.0 );
            SetSkill( SkillName.EvalInt, 100.0 );

            Fame = 20000;  // Boss-tier fame
            Karma = -20000;  // Negative karma for a boss
            VirtualArmor = 80;  // Higher virtual armor for better defense

            // Attach a random ability using the XmlRandomAbility
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to standard loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Add custom boss logic here if needed (e.g., custom weather effects or unique behavior)
        }

        public MedievalMeteorologistBoss(Serial serial) : base(serial)
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
