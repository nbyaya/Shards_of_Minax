using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Twin Terror Ettin")]
    public class TwinTerrorEttinBoss : TwinTerrorEttin
    {
        [Constructable]
        public TwinTerrorEttinBoss() : base()
        {
            Name = "Twin Terror Ettin";
            Title = "the Earthshaker";

            // Update stats to match or exceed Barracoon's stats (using enhanced values)
            SetStr(1200); // Upper range for strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Upper intelligence

            SetHits(12000); // Increased health for the boss
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Improved resistance
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Increased skill levels for a harder fight
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame for a boss-tier encounter
            Karma = -30000; // Boss-tier karma

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // Not tamable as a boss
            ControlSlots = 0; // No control slots required

            // Attach a random ability for more dynamic behavior
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

            // Additional boss logic could go here if desired
        }

        public TwinTerrorEttinBoss(Serial serial) : base(serial)
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
