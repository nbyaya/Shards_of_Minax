using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class IllusionaryDecoy : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Illusionary Decoy", "Illusor Orbis",
            21005,
            9412,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public IllusionaryDecoy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                // Convert IPoint3D to Point3D
                Point3D point = new Point3D(p);

                // Play visual and sound effects for illusion creation
                Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(point, Caster.Map, 0x1E0);

                // Summon the illusionary duplicate
                IllusionaryDecoyCreature illusion = new IllusionaryDecoyCreature(Caster);
                illusion.MoveToWorld(point, Caster.Map);

                // Add temporary effects to reduce the likelihood of being targeted
                illusion.FixedParticles(0x375A, 10, 15, 5036, EffectLayer.Head);
                illusion.PlaySound(0x1F3);

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private IllusionaryDecoy m_Owner;

            public InternalTarget(IllusionaryDecoy owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class IllusionaryDecoyCreature : BaseCreature
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public IllusionaryDecoyCreature(Mobile caster) : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
            {
                m_Caster = caster;
                Name = "Illusionary Decoy";
                Body = caster.Body;
                Hue = 0x4001; // A ghostly hue

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2.0), new TimerCallback(OnEffect));
                m_Timer = new DecoyTimer(this);
                m_Timer.Start();

                SetStr(10);
                SetDex(10);
                SetInt(10);

                Hits = 10;
                Mana = 0;
                Stam = 10;
            }

            public override void OnThink()
            {
                if (Utility.RandomDouble() < 0.3) // 30% chance to play decoy effects periodically
                {
                    FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist);
                    PlaySound(0x1F3);
                }
            }

            private void OnEffect()
            {
                if (Deleted || !Alive)
                    return;

                // Reduce likelihood of being targeted by nearby enemies
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m is BaseCreature && m.Combatant == m_Caster)
                    {
                        BaseCreature bc = (BaseCreature)m;
                        if (Utility.RandomDouble() < 0.5) // 50% chance to redirect target
                        {
                            bc.Combatant = this;
                            bc.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head);
                        }
                    }
                }
            }

            public override bool OnBeforeDeath()
            {
                Effects.PlaySound(Location, Map, 0x1FE); // Sound effect on death
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042);
                return base.OnBeforeDeath();
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class DecoyTimer : Timer
            {
                private IllusionaryDecoyCreature m_Creature;

                public DecoyTimer(IllusionaryDecoyCreature creature) : base(TimeSpan.FromSeconds(30.0))
                {
                    m_Creature = creature;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (!m_Creature.Deleted)
                        m_Creature.Delete();
                }
            }

            public IllusionaryDecoyCreature(Serial serial) : base(serial)
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
}
