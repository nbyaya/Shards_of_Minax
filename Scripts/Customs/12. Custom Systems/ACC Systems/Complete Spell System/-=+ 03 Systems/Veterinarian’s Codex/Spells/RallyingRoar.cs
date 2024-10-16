using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class RallyingRoar : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rallying Roar", "Roar of Courage",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RallyingRoar(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);
                Effects.PlaySound(target.Location, target.Map, 0x16A); // Sound effect for roar

                // Display visual effect on target
                target.FixedParticles(0x3728, 1, 13, 9910, 1153, 2, EffectLayer.Head);

                // Boost morale and attack power for allies in range
                List<Mobile> allies = new List<Mobile>();
                foreach (Mobile m in target.GetMobilesInRange(5))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == Caster)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    if (ally.Alive && ally != target)
                    {
                        ally.SendMessage("You feel a surge of courage from the rallying roar!");
                        // Increase attack power temporarily
                        ally.PlaySound(0x5A1); // Sound effect for buff
                        ally.AddStatMod(new StatMod(StatType.Str, "RallyingRoarBuff", 10, TimeSpan.FromSeconds(30))); // Increase strength by 10 for 30 seconds
                    }
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private RallyingRoar m_Owner;

            public InternalTarget(RallyingRoar owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
