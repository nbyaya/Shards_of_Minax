using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapOffensive : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Offensive", "Trappus Offensivus",
            21001, // Icon
            9200 // Animation
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public TrapOffensive(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapOffensive m_Owner;

            public InternalTarget(TrapOffensive owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseTrap trap && !trap.Deleted && trap.Movable)
                {
                    m_Owner.TransformTrap(trap);
                }
                else
                {
                    from.SendMessage("You must target a movable trap.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void TransformTrap(BaseTrap trap)
        {
            if (!CheckSequence())
                return;

            // Set trap as offensive
            trap.Movable = false;
            trap.PublicOverheadMessage(MessageType.Regular, 0x22, false, "*The trap becomes offensively enchanted*");

            Effects.SendLocationParticles(
                EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 
                0x3728, 10, 30, 5052
            );
            Effects.PlaySound(trap.Location, trap.Map, 0x208);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => TriggerOffensiveTrap(trap));
        }

        private void TriggerOffensiveTrap(BaseTrap trap)
        {
            // Offensive trap logic here
            ArrayList list = new ArrayList();

            foreach (Mobile m in trap.GetMobilesInRange(3))
            {
                if (m.Player && m.Alive && m.AccessLevel == AccessLevel.Player)
                    list.Add(m);
            }

            for (int i = 0; i < list.Count; ++i)
            {
                Mobile m = (Mobile)list[i];

                // Trigger damage or debuff
                if (Utility.RandomBool())
                {
                    m.Damage(Utility.RandomMinMax(10, 20), Caster);
                    m.SendMessage("You are hit by an offensive trap!");
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x307);
                }
                else
                {
                    m.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5)));
                    m.SendMessage("You are paralyzed by an offensive trap!");
                    m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                    m.PlaySound(0x204);
                }
            }

            trap.Delete(); // Trap is consumed after triggering
            FinishSequence();
        }
    }
}
