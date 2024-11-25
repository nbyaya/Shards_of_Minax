using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frost warden ettin corpse")]
    public class FrostWardenEttinBoss : FrostWardenEttin
    {
        [Constructable]
        public FrostWardenEttinBoss() : base()
        {
            Name = "The Frost Warden Ettin";
            Title = "the Overlord of Frost";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Enhanced dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Higher resist skill
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // Higher armor

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
            // You can add additional logic or custom behavior here for the boss-tier version
        }

        public FrostWardenEttinBoss(Serial serial) : base(serial)
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
