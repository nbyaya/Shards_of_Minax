using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the swingin' sorceress overlord")]
    public class SwinginSorceressBoss : SwinginSorceress
    {
        [Constructable]
        public SwinginSorceressBoss() : base()
        {
            Name = "Swingin' Sorceress Overlord";
            Title = "the Jazzy Sorceress Supreme";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Enhanced strength, higher than the original
            SetDex(255); // Maxed dexterity
            SetInt(250); // High intelligence for a boss tier

            SetHits(10000); // Higher health for a boss tier
            SetDamage(25, 35); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced Tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced Wrestling
            SetSkill(SkillName.EvalInt, 120.0); // Enhanced EvalInt
            SetSkill(SkillName.Magery, 120.0); // Enhanced Magery

            Fame = 22500; // Increased Fame
            Karma = -22500; // Increased Karma (still evil)

            VirtualArmor = 80; // Higher Virtual Armor

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
            // Additional boss logic can go here, such as special behaviors or spells
        }

        public SwinginSorceressBoss(Serial serial) : base(serial)
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
