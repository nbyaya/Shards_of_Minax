using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an ethereal guardian corpse")]
    public class EtherealGuardian : BaseCreature
    {
        private Mobile m_Caster;

        [Constructable]
        public EtherealGuardian()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an ethereal guardian";
            Body = 58;
            Hue = 1150; // Glowing ghostly blue
            BaseSoundID = 466;
            Team = 4;


            double spiritSpeak = 0;

            int bonus = (int)(spiritSpeak / 5); // Scaling factor for stats

            SetStr(200 + bonus, 225 + bonus);
            SetDex(200 + bonus, 225 + bonus);
            SetInt(200 + bonus, 225 + bonus);

            SetHits(150 + bonus * 2, 200 + bonus * 2);
            SetDamage(18 + bonus / 10, 24 + bonus / 10);

            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 40 + bonus / 10, 50 + bonus / 10);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 15, 30);
            SetResistance(ResistanceType.Energy, 60 + bonus / 10, 80 + bonus / 10);

            SetSkill(SkillName.Magery, 100.0 + bonus / 2);
            SetSkill(SkillName.EvalInt, 100.0 + bonus / 2);
            SetSkill(SkillName.MagicResist, 90.0 + bonus / 2);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 90.0);

            Fame = 5000;
            Karma = 5000;
            VirtualArmor = 50 + bonus / 2;

            AddItem(new LightSource());

            // Custom aura timer
            new EtherealAura(this).Start();
        }

        public EtherealGuardian(Serial serial) : base(serial)
        {
        }

        public override bool AlwaysMurderer => false;
        public override bool BardImmune => true;
        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override bool Uncalmable => true;
        public override bool DisallowAllMoves => false;

        public override void OnThink()
        {
            base.OnThink();

            // Defensive support — reflect damage
            if (Combatant != null && Utility.RandomDouble() < 0.10)
            {
                Combatant.Damage(10, this);
                Combatant.FixedParticles(0x37B9, 10, 5, 5023, EffectLayer.Head);
                Say("*The guardian retaliates with ethereal energy*");
            }
        }

        public override void GenerateLoot()
        {
            // No loot for summoned guardians
        }

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

        private class EtherealAura : Timer
        {
            private EtherealGuardian m_Guardian;

            public EtherealAura(EtherealGuardian guardian)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(5.0))
            {
                m_Guardian = guardian;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Guardian.Deleted || !m_Guardian.Alive)
                {
                    Stop();
                    return;
                }

                // Aura Heal for allies and damage undead/hostile near the guardian
                IPooledEnumerable nearby = m_Guardian.GetMobilesInRange(4);
                foreach (Mobile m in nearby)
                {
                    if (m == m_Guardian) continue;

                    if (m.Guild == m_Guardian.ControlMaster?.Guild || m == m_Guardian.ControlMaster)
                    {
                        m.Heal(5 + Utility.Random(6));
                        m.FixedParticles(0x375A, 10, 15, 5032, EffectLayer.Waist);
                    }
                    else if (m.Kills > 0 || (m is BaseCreature bc && bc.OppositionGroup == OppositionGroup.FeyAndUndead))
                    {
                        m.Damage(5 + Utility.Random(10), m_Guardian);
                        m.FixedParticles(0x3709, 10, 15, 5040, EffectLayer.Head);
                    }
                }

                nearby.Free();
            }
        }
    }
}
