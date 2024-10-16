using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a luminous arachnid corpse")]
    public class LuminousArachnid : BaseCreature
    {
        private DateTime m_NextLightBurst;
        private DateTime m_NextWebSnare;

        [Constructable]
        public LuminousArachnid()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a luminous arachnid";
            Body = 48; // Scorpion body
            BaseSoundID = 397;
            Hue = 1195; // Bright cyan hue

            SetStr(100, 140);
            SetDex(90, 110);
            SetInt(100, 130);

            SetHits(80, 100);
            SetMana(50, 70);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 80.0, 90.0);
            SetSkill(SkillName.Magery, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 65.0, 75.0);
            SetSkill(SkillName.Tactics, 70.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 70.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextLightBurst = DateTime.UtcNow;
            m_NextWebSnare = DateTime.UtcNow;
        }

        public LuminousArachnid(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextLightBurst)
                {
                    LightBurst();
                }

                if (DateTime.UtcNow >= m_NextWebSnare)
                {
                    WebSnare();
                }
            }
        }

        private void LightBurst()
        {
            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = targets[i];

                DoHarmful(m);

                m.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Waist);
                m.PlaySound(0x201);

                int damage = Utility.RandomMinMax(20, 30);
                m.Damage(damage, this);

                if (m.Player)
                {
                    m.SendLocalizedMessage(1111675); // The burst of light blinds you!
                    m.SendLocalizedMessage(1111676); // You are dazed by the bright light!
                }
            }

            m_NextLightBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void WebSnare()
        {
            if (Combatant is Mobile)
            {
                Mobile m = (Mobile)Combatant;

                if (m.Frozen)
                    return;

                if (CanBeHarmful(m))
                {
                    DoHarmful(m);

                    m.Frozen = true;
                    m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    m.PlaySound(0x204);

                    if (m.Player)
                        m.SendLocalizedMessage(1042555); // You are ensnared in a spider web!

                    Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerStateCallback(ReleaseWebSnare), m);
                }
            }

            m_NextWebSnare = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ReleaseWebSnare(object state)
        {
            if (state is Mobile)
            {
                Mobile m = (Mobile)state;

                m.Frozen = false;
                m.FixedParticles(0x376A, 9, 32, 5042, EffectLayer.Waist);

                if (m.Player)
                    m.SendLocalizedMessage(1042532); // You free yourself from the web!
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

            m_NextLightBurst = DateTime.UtcNow;
            m_NextWebSnare = DateTime.UtcNow;
        }
    }
}
