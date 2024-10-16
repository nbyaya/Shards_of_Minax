using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class EtherealMelody : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ethereal Melody", "Canto Spectralis",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public EtherealMelody(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x29); // Musical sound effect
                
                // Convert IPoint3D to Point3D
                Point3D location = new Point3D(p);
                Effects.SendLocationParticles(EffectItem.Create(location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044); // Spectral light effect

                Mobile entity = new SpectralEntity(Caster);
                entity.MoveToWorld(location, Caster.Map);
                entity.Combatant = Caster.Combatant;

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 10, () =>
                {
                    if (entity == null || entity.Deleted || !entity.Alive || entity.Combatant == null || entity.Combatant.Deleted || !entity.Combatant.Alive)
                        return;

                    Effects.SendTargetParticles(entity.Combatant, 0x374A, 10, 30, 5052, EffectLayer.Waist); // Energy blast effect
                    entity.Combatant.Damage(Utility.RandomMinMax(5, 10), entity);
                });

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    if (entity != null && !entity.Deleted)
                        entity.Delete();
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private EtherealMelody m_Owner;

            public InternalTarget(EtherealMelody owner) : base(12, true, TargetFlags.Harmful)
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

        private class SpectralEntity : BaseCreature
        {
            private Mobile m_Caster;

            public SpectralEntity(Mobile caster) : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
            {
                m_Caster = caster;
                Name = "Spectral Entity";
                Body = 0x9;
                BaseSoundID = 0x1F2;

                SetStr(50);
                SetDex(50);
                SetInt(50);

                SetHits(100);
                SetStam(100);
                SetMana(0);

                SetDamage(5, 10);

                SetSkill(SkillName.MagicResist, 50.0);
                SetSkill(SkillName.Tactics, 50.0);
                SetSkill(SkillName.Wrestling, 50.0);

                ControlSlots = 1;
                ControlOrder = OrderType.Attack;
            }

            public SpectralEntity(Serial serial) : base(serial)
            {
            }

            public override void OnThink()
            {
                base.OnThink();

                if (Combatant != null && !Deleted && Combatant.Alive && !Combatant.Deleted && InRange(Combatant, 12))
                {
                    Effects.PlaySound(Location, Map, 0x29); // Repeated musical sound effect
                }
                else
                {
                    Delete();
                }
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
