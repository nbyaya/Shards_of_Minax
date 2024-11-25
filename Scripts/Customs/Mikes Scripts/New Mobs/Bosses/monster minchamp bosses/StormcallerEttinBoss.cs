using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a stormcaller ettin corpse")]
    public class StormcallerEttinBoss : StormcallerEttin
    {
        [Constructable]
        public StormcallerEttinBoss() : base()
        {
            Name = "Stormcaller Ettin";
            Title = "the Storm King";

            // Update stats to match or exceed Barracoon (higher values can be adjusted for the boss-tier)
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300);   // Increased dexterity
            SetInt(250, 350);   // Increased intelligence

            SetHits(15000);     // Increased health
            SetDamage(35, 50);  // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 105.0, 120.0);  // Enhanced spell casting skills
            SetSkill(SkillName.MagicResist, 150.0);    // Higher magic resistance
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 120.0, 140.0); // Increased wrestling skill

            Fame = 30000;   // Increased fame
            Karma = -30000; // Increased karma for a more menacing boss

            VirtualArmor = 100; // Increased armor for a tanky boss

            Tamable = false;   // Bosses are not tamable
            ControlSlots = 3;  // Same control slots as original

            // Attach the random ability
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
            // You can add additional logic or custom behavior here for the boss-tier version
        }

        public StormcallerEttinBoss(Serial serial) : base(serial)
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
