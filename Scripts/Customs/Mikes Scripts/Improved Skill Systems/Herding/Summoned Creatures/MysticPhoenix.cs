using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic phoenix corpse")]
    public class MysticPhoenix : BaseCreature
    {
        private DateTime m_NextRebirthFlame;

        [Constructable]
        public MysticPhoenix()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic phoenix";
            Body = 5; // Phoenix body
            BaseSoundID = 0x8F;
            Hue = 1359; // Unique hue for Mystic Phoenix

            this.SetStr(300);
            this.SetDex(150);
            this.SetInt(200);

            this.SetHits(200);
            this.SetMana(100);

            this.SetDamage(15, 25);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Fire, 50);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 70, 80);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextRebirthFlame = DateTime.UtcNow;
        }

        public MysticPhoenix(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Feathers { get { return 50; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextRebirthFlame)
                {
                    RebirthFlame();
                }
            }
        }

        private void RebirthFlame()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int healAmount = Utility.RandomMinMax(20, 30);
                Hits = Math.Min(Hits + healAmount, HitsMax);

                Effects.PlaySound(Location, Map, 0x208);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);

                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m != target && CanBeHarmful(m))
                    {
                        m.Damage(Utility.RandomMinMax(10, 20), this);
                    }
                }

                m_NextRebirthFlame = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
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

            m_NextRebirthFlame = DateTime.UtcNow;
        }
    }
}
