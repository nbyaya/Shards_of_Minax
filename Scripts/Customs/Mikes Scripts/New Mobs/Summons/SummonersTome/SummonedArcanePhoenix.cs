using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an arcane phoenix corpse")]
    public class SummonedArcanePhoenix : BaseCreature
    {
        private Timer m_LifetimeTimer;

        public override bool DeleteOnRelease => true;
        public override bool Commandable => false;
        public override bool BardImmune => true;

        private Mobile m_Caster;
        private double m_SpiritSpeak;

        [Constructable]
        public SummonedArcanePhoenix()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an arcane phoenix";
            Body = 0x340;
            Hue = 0x489; // Arcane bluish-purple fire hue
            BaseSoundID = 0x8F;


            int ssBonus = 0;

            SetStr(500 + ssBonus / 4);
            SetDex(200 + ssBonus / 5);
            SetInt(500 + ssBonus / 2);

            SetHits(400 + ssBonus);
            SetDamage(20 + ssBonus / 20, 25 + ssBonus / 20);

            SetDamageType(ResistanceType.Energy, 40);
            SetDamageType(ResistanceType.Fire, 60);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Energy, 50);
            SetResistance(ResistanceType.Poison, 30);

            SetSkill(SkillName.Magery, 100 + ssBonus / 10);
            SetSkill(SkillName.EvalInt, 100 + ssBonus / 10);
            SetSkill(SkillName.MagicResist, 100 + ssBonus / 10);
            SetSkill(SkillName.Tactics, 90);
            SetSkill(SkillName.Wrestling, 90);

            VirtualArmor = 70;

            Fame = 0;
            Karma = 0;

            ControlSlots = 4;
            Tamable = false;



            // Passive spell-disruption aura
            new DispelAura(this).Start();

            // Lifespan (e.g., 2 minutes)
            m_LifetimeTimer = Timer.DelayCall(TimeSpan.FromMinutes(2), () =>
            {
                this.PlaySound(0x307); // Rebirth burst
                Effects.SendLocationEffect(this.Location, this.Map, 0x3709, 30, 10);
                AOEArcaneExplosion();
                this.Delete();
            });
        }

        public override void OnDelete()
        {
            if (m_LifetimeTimer != null)
                m_LifetimeTimer.Stop();

            base.OnDelete();
        }

        // Arcane explosion on death
		public override bool OnBeforeDeath()
		{
			AOEArcaneExplosion();
			return true;
		}


        private void AOEArcaneExplosion()
        {
            IPooledEnumerable e = GetMobilesInRange(3);
            foreach (Mobile m in e)
            {
                if (m == null || m == this || !m.Alive || !CanBeHarmful(m)) continue;

                DoHarmful(m);
                int damage = Utility.RandomMinMax(30, 50);
                m.Damage(damage);
                m.FixedParticles(0x3709, 10, 30, 5052, 0x47D, 0, EffectLayer.CenterFeet);
                m.PlaySound(0x208);
            }
            e.Free();
        }

        public SummonedArcanePhoenix(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private class DispelAura : Timer
        {
            private readonly BaseCreature m_Creature;

            public DispelAura(BaseCreature creature) : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
            {
                m_Creature = creature;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Creature == null || m_Creature.Deleted)
                {
                    Stop();
                    return;
                }

                var range = m_Creature.GetMobilesInRange(3);
                foreach (Mobile m in range)
                {
                    if (m == null || m.AccessLevel > AccessLevel.Player || !m.Alive)
                        continue;

                    if (m is BaseCreature bc && bc.Controlled && bc.ControlMaster != m_Creature)
                    {
                        if (Utility.RandomDouble() < 0.2)
                        {
                            m_Creature.Say("*The arcane aura disrupts a summon!*");
                            Effects.SendLocationEffect(bc.Location, bc.Map, 0x374A, 16, 1);
                            bc.Delete();
                        }
                    }
                }
            }
        }
    }
}
