using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class SummoningTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Summoning Trap", "Summon Guard",
                                                        21001,
                                                        9200
                                                       );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public SummoningTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
				Caster.SendLocalizedMessage(500237); // Target cannot be seen.
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				if (this.Scroll != null)
					Scroll.Consume();

				SpellHelper.Turn(Caster, p);
				SpellHelper.GetSurfaceTop(ref p);

				// Convert IPoint3D to Point3D
				Point3D point = new Point3D(p.X, p.Y, p.Z);

				Effects.PlaySound(point, Caster.Map, 0x2F4); // Play trap setting sound
				Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052); // Trap animation

				SummoningTrapItem trap = new SummoningTrapItem(Caster, point, Caster.Map);
				trap.MoveToWorld(point, Caster.Map);

				FinishSequence();
			}
		}


        private class InternalTarget : Target
        {
            private SummoningTrap m_Owner;

            public InternalTarget(SummoningTrap owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public class SummoningTrapItem : Item
    {
        private Mobile m_Caster;
        private Timer m_Timer;

        [Constructable]
        public SummoningTrapItem(Mobile caster, Point3D loc, Map map) : base(0x1B72) // Invisible trap item ID
        {
            Movable = false;
            Visible = false;
            m_Caster = caster;

            MoveToWorld(loc, map);
            m_Timer = new TrapTimer(this);
            m_Timer.Start();
        }

        public SummoningTrapItem(Serial serial) : base(serial)
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

        public override bool OnMoveOver(Mobile m)
        {
            if (m is BaseCreature || m is PlayerMobile)
            {
                TriggerTrap(m);
                return false;
            }

            return base.OnMoveOver(m);
        }

        private void TriggerTrap(Mobile m)
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052); // Trap trigger animation
            Effects.PlaySound(Location, Map, 0x208); // Trap trigger sound

            if (m_Caster != null && !m.Deleted)
            {
                BaseCreature summoned = new SummonedCreature(); // Replace with desired creature type
                SpellHelper.Summon(summoned, m_Caster, 0x215, TimeSpan.FromMinutes(2.0), false, false);
                summoned.MoveToWorld(Location, Map);
            }

            Delete();
        }

        private class TrapTimer : Timer
        {
            private SummoningTrapItem m_Trap;

            public TrapTimer(SummoningTrapItem trap) : base(TimeSpan.FromMinutes(1.0))
            {
                m_Trap = trap;
            }

            protected override void OnTick()
            {
                if (m_Trap != null && !m_Trap.Deleted)
                    m_Trap.Delete();
            }
        }
    }

    public class SummonedCreature : BaseCreature
    {
        public SummonedCreature() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4) // Changed FightMode.All to FightMode.Closest
        {
            Name = "Summoned Guardian";
            Body = 0x190; // Replace with desired creature body type
            BaseSoundID = 0x45A; // Creature sound ID

            SetStr(100);
            SetDex(90);
            SetInt(30);

            SetHits(200);
            SetDamage(15, 20);

            SetSkill(SkillName.Wrestling, 70.0);
            SetSkill(SkillName.Tactics, 70.0);

            ControlSlots = 1;
            // BardImmune = true; // BardImmune is read-only, so this line is removed
            Tamable = false;
        }

        public SummonedCreature(Serial serial) : base(serial)
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
