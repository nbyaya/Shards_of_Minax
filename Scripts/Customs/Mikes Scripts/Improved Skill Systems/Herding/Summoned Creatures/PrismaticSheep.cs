using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a prismatic sheep corpse")]
    public class PrismaticSheep : BaseCreature
    {
        private DateTime m_NextColorChange;
        private DateTime m_NextResistanceGrant;

        [Constructable]
        public PrismaticSheep()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a prismatic sheep";
            Body = 0xCF;
            BaseSoundID = 0xD6;
            Hue = Utility.RandomMinMax(1150, 1175); // Random prismatic hue

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextColorChange = DateTime.UtcNow;
            m_NextResistanceGrant = DateTime.UtcNow;
        }

        public PrismaticSheep(Serial serial)
            : base(serial)
        {
        }

        public override int Wool { get { return 3; } }
        public override int Meat { get { return 2; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextColorChange)
            {
                ChangeColor();
            }

            if (DateTime.UtcNow >= m_NextResistanceGrant)
            {
                GrantResistance();
            }
        }

        private void ChangeColor()
        {
            Hue = Utility.RandomMinMax(1150, 1175); // Random prismatic hue
            FixedEffect(0x373A, 10, 30);
            PlaySound(0x1FA);
            m_NextColorChange = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void GrantResistance()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && CanBeHarmful(m, false))
                {
                    ResistanceType type = (ResistanceType)Utility.Random(5);
                    int amount = Utility.RandomMinMax(10, 20);
                    TimeSpan duration = TimeSpan.FromSeconds(30);

                    ResistanceMod mod = new ResistanceMod(type, amount);
                    m.AddResistanceMod(mod);

                    Timer.DelayCall(duration, delegate
                    {
                        m.RemoveResistanceMod(mod);
                    });

                    m.FixedEffect(0x373A, 10, 30);
                    m.PlaySound(0x1E9);
                }
            }

            m_NextResistanceGrant = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextColorChange = DateTime.UtcNow;
            m_NextResistanceGrant = DateTime.UtcNow;
        }
    }
}