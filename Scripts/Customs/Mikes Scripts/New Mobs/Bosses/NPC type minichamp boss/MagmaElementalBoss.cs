using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the magma overlord")]
    public class MagmaElementalBoss : MagmaElemental
    {
        [Constructable]
        public MagmaElementalBoss() : base()
        {
            Name = "Magma Overlord";
            Title = "the Infernal Fury";
            Body = 16; // The same as the original MagmaElemental

            Hue = 1995; // Maintain the fiery hue

            // Enhanced stats to make the boss more formidable
            SetStr(425); // Enhanced Strength
            SetDex(150); // Enhanced Dexterity
            SetInt(750); // Enhanced Intelligence

            SetHits(12000); // High health for a boss

            SetDamage(30, 45); // Increased damage range to make it stronger

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500;   // High fame for a boss-level creature
            Karma = -22500; // Negative karma for a villainous boss

            VirtualArmor = 80; // Increased armor for better defense

            // Attach a random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to the normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop apples with a flame strike effect, just like the original
            int applesToDrop = 10;

            for (int i = 0; i < applesToDrop; i++)
            {
                Point3D appleLocation = new Point3D(this.X + Utility.RandomMinMax(-2, 2), this.Y + Utility.RandomMinMax(-2, 2), this.Z);
                HotLavaTile droppedApple = new HotLavaTile();
                droppedApple.MoveToWorld(appleLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(appleLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 0, 0, 2023, 0);
            }
        }

        public MagmaElementalBoss(Serial serial) : base(serial)
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
