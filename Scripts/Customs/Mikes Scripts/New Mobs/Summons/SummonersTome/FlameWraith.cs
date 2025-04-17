using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a smoldering corpse")]
    public class FlameWraith : BaseCreature
    {
        private Mobile m_Summoner;

        [Constructable]
        public FlameWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flame wraith";
            Body = 26;
            Hue = 1359; // Fiery orange-red hue
            BaseSoundID = 0x482;



            double spiritSpeak = 0;
            int bonus = (int)(spiritSpeak * 0.5); // Bonus scaling: 50% of SpiritSpeak

            SetStr(90 + bonus, 110 + bonus);
            SetDex(80 + bonus / 2, 100 + bonus / 2);
            SetInt(60 + bonus, 80 + bonus);

            SetHits(100 + bonus, 150 + bonus);
            SetDamage(10 + bonus / 10, 14 + bonus / 10);

            SetDamageType(ResistanceType.Fire, 100);

            SetResistance(ResistanceType.Physical, 25 + bonus / 5, 35 + bonus / 5);
            SetResistance(ResistanceType.Fire, 50 + bonus / 5, 60 + bonus / 5);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 80.0 + bonus / 2);
            SetSkill(SkillName.Magery, 80.0 + bonus / 2);
            SetSkill(SkillName.MagicResist, 75.0 + bonus / 2);
            SetSkill(SkillName.Tactics, 70.0 + bonus / 2);
            SetSkill(SkillName.Wrestling, 60.0 + bonus / 2);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40 + bonus / 4;

            PackReg(5);
        }

        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override bool BardImmune => true;
        public override bool AutoDispel => true;
        public override bool DeleteOnRelease => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow > m_NextFlameNova)
            {
                FlameNova();
                m_NextFlameNova = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        private DateTime m_NextFlameNova;

        /// <summary>
        /// Unique ability: Flame Nova – AoE fire explosion around the Wraith
        /// </summary>
        private void FlameNova()
        {
            this.PlaySound(0x208); // Explosion sound
            this.FixedParticles(0x3709, 10, 30, 5052, Hue, 0, EffectLayer.CenterFeet);

            var map = this.Map;
            if (map == null)
                return;

            IPooledEnumerable eable = map.GetMobilesInRange(this.Location, 4);

            foreach (Mobile m in eable)
            {
                if (m != this && m != m_Summoner && m.Alive && this.CanBeHarmful(m))
                {
                    this.DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 100, 0, 0, 0); // Pure fire damage
                    m.SendMessage("The flame wraith scorches you with searing heat!");
                }
            }

            eable.Free();
        }

        public FlameWraith(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Summoner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Summoner = reader.ReadMobile();
        }
    }
}
