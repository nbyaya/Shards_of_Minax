using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blood ape corpse")]
    public class BloodApe : BaseCreature
    {
        private DateTime m_NextHealthDrain;
        private DateTime m_NextBloodFrenzy;
        private DateTime m_BloodFrenzyEnd;

        [Constructable]
        public BloodApe()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blood ape";
            Body = 0x1D;
            BaseSoundID = 0x9E;
            Hue = 1157; // Red hue

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

            m_NextHealthDrain = DateTime.UtcNow;
            m_NextBloodFrenzy = DateTime.UtcNow;
        }

        public BloodApe(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextHealthDrain)
                {
                    DrainHealth();
                }

                if (DateTime.UtcNow >= m_NextBloodFrenzy && DateTime.UtcNow >= m_BloodFrenzyEnd)
                {
                    ActivateBloodFrenzy();
                }
            }

            if (DateTime.UtcNow >= m_BloodFrenzyEnd && m_BloodFrenzyEnd != DateTime.MinValue)
            {
                DeactivateBloodFrenzy();
            }
        }

        private void DrainHealth()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(5, 10);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + damage, HitsMax);
                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);
                m_NextHealthDrain = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        private void ActivateBloodFrenzy()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blood Frenzy! *");
            PlaySound(0x165);
            FixedEffect(0x37C4, 10, 36);

            SetStr(Str + 30);
            SetDex(Dex + 20);
            SetInt(Int + 10);

            m_BloodFrenzyEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextBloodFrenzy = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void DeactivateBloodFrenzy()
        {
            SetStr(Str - 30);
            SetDex(Dex - 20);
            SetInt(Int - 10);

            m_BloodFrenzyEnd = DateTime.MinValue;
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

            m_NextHealthDrain = DateTime.UtcNow;
            m_NextBloodFrenzy = DateTime.UtcNow;
            m_BloodFrenzyEnd = DateTime.MinValue;
        }
    }
}