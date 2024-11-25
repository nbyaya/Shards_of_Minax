using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the gothic overlord")]
    public class GothicNovelistBoss : GothicNovelist
    {
        [Constructable]
        public GothicNovelistBoss() : base()
        {
            Name = "Gothic Overlord";
            Title = "the Dark Writer";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health
            SetDamage(25, 40); // Increased damage range

            SetResistance(ResistanceType.Physical, 75); // Increased resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Increased Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Increased Tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased Wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased armor

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
            // Additional boss logic could be added here
        }

        public GothicNovelistBoss(Serial serial) : base(serial)
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
