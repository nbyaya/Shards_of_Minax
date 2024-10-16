using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a moon rabbit corpse")]
    public class MoonRabbit : BaseCreature
    {
        private DateTime m_NextLunarBlessing;
        private DateTime m_LunarBlessingEnd;

        [Constructable]
        public MoonRabbit()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a moon rabbit";
            Body = 205;
            Hue = 1153; // Light blue hue representing lunar essence

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

            m_NextLunarBlessing = DateTime.UtcNow;
        }

        public MoonRabbit(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextLunarBlessing && DateTime.UtcNow >= m_LunarBlessingEnd)
                {
                    ActivateLunarBlessing();
                }
            }

            if (DateTime.UtcNow >= m_LunarBlessingEnd && m_LunarBlessingEnd != DateTime.MinValue)
            {
                DeactivateLunarBlessing();
            }
        }

        private void ActivateLunarBlessing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lunar Blessing! *");
            PlaySound(0x20E); // Mystic sound
            FixedEffect(0x373A, 10, 30);

            SetStr(Str + 10);
            SetDex(Dex + 10);
            SetInt(Int + 10);

            m_LunarBlessingEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextLunarBlessing = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void DeactivateLunarBlessing()
        {
            SetStr(Str - 10);
            SetDex(Dex - 10);
            SetInt(Int - 10);

            m_LunarBlessingEnd = DateTime.MinValue;
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

            m_NextLunarBlessing = DateTime.UtcNow;
            m_LunarBlessingEnd = DateTime.MinValue;
        }
    }
}
