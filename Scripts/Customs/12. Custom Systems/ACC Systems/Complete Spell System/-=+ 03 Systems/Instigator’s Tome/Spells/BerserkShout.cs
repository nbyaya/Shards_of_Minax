using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class BerserkShout : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Berserk Shout", "Berex Kran",
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public BerserkShout(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BerserkShout m_Owner;

            public InternalTarget(BerserkShout owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target))
                    {
                        from.DoHarmful(target);
                        m_Owner.Effect(target);
                    }
                }
                else
                {
                    from.SendMessage("You can only target a living creature.");
                }

                m_Owner.FinishSequence();
            }
        }

        public void Effect(Mobile target)
        {
            if (CheckSequence())
            {
                // Apply frenzy effect: increase damage but reduce defense
                target.SendMessage("You feel a surge of uncontrollable rage!");
                Effects.PlaySound(target.Location, target.Map, 0x1F5); // Play a loud shout sound effect
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Apply a visual effect on the target

                // Buff and Debuff: Increase damage, reduce defenses
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Berserk, 1152125, TimeSpan.FromSeconds(10), target, "Increased damage output, but reduced defenses!"));

                // Temporary modification using custom effects
                target.SendMessage("You are now berserk, gaining damage but losing defense!");

                // Increase damage and decrease physical resistance
                ApplyBerserkEffects(target, 20, -15);

                // Set a timer to end the effect after 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    RemoveBerserkEffects(target, 20, 15);
                    BuffInfo.RemoveBuff(target, BuffIcon.Berserk);
                    target.SendMessage("The frenzy fades away.");
                });
            }
        }

        private void ApplyBerserkEffects(Mobile target, int damageIncrease, int resistanceDecrease)
        {
            if (target is BaseCreature creature)
            {
                // Assuming 'BaseCreature' has methods to modify damage and resistance
                creature.DamageMin += damageIncrease; // Increase minimum damage
                creature.DamageMax += damageIncrease; // Increase maximum damage

                // Simulate reduced resistance using a custom debuff or multiplier
                creature.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, resistanceDecrease));
            }
        }

        private void RemoveBerserkEffects(Mobile target, int damageIncrease, int resistanceDecrease)
        {
            if (target is BaseCreature creature)
            {
                // Assuming 'BaseCreature' has methods to modify damage and resistance
                creature.DamageMin -= damageIncrease; // Restore minimum damage
                creature.DamageMax -= damageIncrease; // Restore maximum damage

                // Remove the custom resistance modification
                creature.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, resistanceDecrease));
            }
        }
    }
}
