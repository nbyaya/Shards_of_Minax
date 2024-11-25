using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a venomous ivy corpse")]
    public class VenomousIvyBoss : VenomousIvy
    {
        [Constructable]
        public VenomousIvyBoss() : base("Venomous Ivy Boss")
        {
            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Increase strength
            SetDex(255); // Increase dexterity
            SetInt(250); // Increase intelligence

            SetHits(12000); // Set high health
            SetDamage(35, 45); // Increase damage range

            // Increase resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // Set high virtual armor

            Tamable = false;
            ControlSlots = 0; // Bosses can't be tamed

            // Attach the XmlRandomAbility for additional features
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
            // Additional boss logic could be added here if needed
        }

        public VenomousIvyBoss(Serial serial) : base(serial)
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
