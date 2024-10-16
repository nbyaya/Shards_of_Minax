using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingMeditation : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Meditation", "Heal Us All",
            21004, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public HealingMeditation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HealingMeditation m_Owner;

            public InternalTarget(HealingMeditation owner) : base(12, true, TargetFlags.None)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x5A2); // Meditation sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 1153, 7, 9502, 0); // Glowing effect

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 10, HealNearbyAllies, new object[] { Caster });

                Caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Meditation effect
                Caster.PlaySound(0x5C9); // Sound effect for healing

                FinishSequence();
            }
        }

        private void HealNearbyAllies(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster == null || caster.Deleted || !caster.Alive)
                return;

            ArrayList allies = new ArrayList();

            foreach (Mobile m in caster.GetMobilesInRange(5))
            {
                if (m.Alive && !IsEnemy(m, caster) && m != caster)
                    allies.Add(m);
            }

            foreach (Mobile ally in allies)
            {
                ally.Heal(Utility.RandomMinMax(10, 20)); // Heal 10-20 health points
                ally.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Healing effect
                ally.PlaySound(0x1F2); // Sound effect for being healed
            }

            // Heal the caster as well
            caster.Heal(Utility.RandomMinMax(10, 20));
        }

        private bool IsEnemy(Mobile target, Mobile caster)
        {
            // Implement your criteria for determining if the target is an enemy
            // For simplicity, let's assume that anyone who is not on the same team can be considered an enemy
            return !CanBeHealedBy(caster, target);
        }

        private bool CanBeHealedBy(Mobile healer, Mobile target)
        {
            // Custom logic to determine if the target can be healed by the healer
            // Example logic: Check if the healer and target are in the same faction or guild, etc.
            // For this example, let's assume everyone can be healed by everyone
            return true;
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
