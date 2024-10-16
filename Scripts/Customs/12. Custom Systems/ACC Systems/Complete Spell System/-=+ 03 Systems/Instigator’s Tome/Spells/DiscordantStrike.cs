using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class DiscordantStrike : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Discordant Strike", "Confusum Malum",
            21005, 9400, false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public DiscordantStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeHarmful(target) && CheckSequence())
            {
                Caster.DoHarmful(target);

                // Apply confusion effect
                if (Utility.RandomDouble() < 0.5)
                {
                    // Target attacks its allies
                    target.Say("*is confused and attacks allies*");

                    // Temporarily force the target to attack a nearby ally
                    Mobile newTarget = FindRandomAlly(target);
                    if (newTarget != null)
                    {
                        target.Combatant = newTarget;
                        newTarget.Combatant = target;
                    }
                }
                else
                {
                    // Target becomes less effective in combat
                    target.Say("*is weakened and less effective in combat*");

                    if (target is BaseCreature baseCreature)
                    {
                        // Assume we have some way to adjust damage, e.g., modify weapon stats
                        baseCreature.BeginAction(typeof(DiscordantStrike)); // Example action to simulate damage reduction
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => RestoreDamage(baseCreature));
                    }
                }

                // Visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(target.Location, target.Map, 0x1FE);

                // End the casting sequence
                FinishSequence();
            }
        }

        private Mobile FindRandomAlly(Mobile target)
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in target.GetMobilesInRange(10))
            {
                if (m != target && m is BaseCreature creature && creature.ControlMaster == (target as BaseCreature)?.ControlMaster)
                    list.Add(m);
            }

            if (list.Count > 0)
                return (Mobile)list[Utility.Random(list.Count)];

            return null;
        }

        private void RestoreDamage(BaseCreature target)
        {
            if (target != null && !target.Deleted)
            {
                target.EndAction(typeof(DiscordantStrike)); // Example action to simulate damage restoration
            }
        }

        private class InternalTarget : Target
        {
            private DiscordantStrike m_Owner;

            public InternalTarget(DiscordantStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
