using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a pool of sizzling acid")]
    public class SummonedAcidElemental : BaseCreature, IAcidCreature
    {
        private Timer m_LifetimeTimer;
        private Mobile m_Summoner;
        private double m_SpiritSpeakSkill;

        public override bool DeleteOnRelease => true;
        public override bool Commandable => false;
        public override bool AlwaysMurderer => true;

        [Constructable]
        public SummonedAcidElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a summoned acid elemental";
            Body = 158;
            BaseSoundID = 263;
            Hue = 0x9C4; // Acid green


            m_SpiritSpeakSkill = 0;

            double scale = m_SpiritSpeakSkill / 120.0;

            SetStr((int)(200 + (scale * 200)));
            SetDex((int)(60 + (scale * 40)));
            SetInt((int)(150 + (scale * 100)));

            SetHits((int)(150 + (scale * 300)));
            SetDamage(10 + (int)(scale * 10), 20 + (int)(scale * 10));

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 90);

            SetResistance(ResistanceType.Physical, 50 + (int)(scale * 20));
            SetResistance(ResistanceType.Fire, 20 + (int)(scale * 20));
            SetResistance(ResistanceType.Cold, 20 + (int)(scale * 20));
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 30 + (int)(scale * 20));

            SetSkill(SkillName.Magery, 80.0 + (scale * 20));
            SetSkill(SkillName.EvalInt, 80.0 + (scale * 20));
            SetSkill(SkillName.MagicResist, 60.0 + (scale * 25));
            SetSkill(SkillName.Tactics, 80.0 + (scale * 10));
            SetSkill(SkillName.Wrestling, 70.0 + (scale * 20));

            VirtualArmor = 60 + (int)(scale * 20);

            Fame = 0;
            Karma = 0;

            ControlSlots = 2;

            m_LifetimeTimer = Timer.DelayCall(TimeSpan.FromMinutes(3.0), Delete); // 3 minute lifespan
            StartCorrosionAura();
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override double HitPoisonChance => 0.75;
        public override bool AutoDispel => false;

        public override void OnDelete()
        {
            if (m_LifetimeTimer != null)
                m_LifetimeTimer.Stop();

            base.OnDelete();
        }

        public override void OnThink()
        {
            base.OnThink();

            // Slowly regenerate health if close to summoner
            if (m_Summoner != null && !Deleted && m_Summoner.Map == Map && Utility.InRange(Location, m_Summoner.Location, 10))
            {
                Hits = Math.Min(HitsMax, Hits + 1); // 1 HP per think cycle
            }
        }

        private void StartCorrosionAura()
        {
            Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), () =>
            {
                if (Deleted || !Alive)
                    return;

                IPooledEnumerable eable = GetMobilesInRange(2);
                foreach (Mobile m in eable)
                {
                    if (m != this && m.Alive && CanBeHarmful(m) && m != m_Summoner && m.AccessLevel == AccessLevel.Player)
                    {
                        AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100); // Pure poison
                        m.PlaySound(0x231);
                        m.FixedEffect(0x3728, 10, 15, 0x481, 3);
                        m.SendMessage("The acid burns your flesh and weakens your defenses!");
                        // Optional: Resistance debuffs or other effects could be added here.
                    }
                }
                eable.Free();
            });
        }

        public SummonedAcidElemental(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
