using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class CommandBeast : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Command Beast", "Domini Bestia",
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public CommandBeast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CommandBeast m_Owner;

            public InternalTarget(CommandBeast owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseCreature creature && creature.Controlled == false && creature.IsEnemy(from))
                {
                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, creature);
                        creature.ControlMaster = from;
                        creature.Controlled = true;
                        creature.ControlOrder = OrderType.Follow;
                        creature.ControlTarget = from;

                        // Visual and sound effects
                        creature.FixedParticles(0x373A, 1, 15, 9909, EffectLayer.Waist);
                        creature.PlaySound(0x206);

                        Timer.DelayCall(TimeSpan.FromSeconds(15.0), () =>
                        {
                            // Revert to original state
                            if (creature != null && !creature.Deleted)
                            {
                                creature.Controlled = false;
                                creature.ControlMaster = null;
                                creature.Aggressors.Clear();
                                creature.Aggressed.Clear();
                                creature.Combatant = from;
                                creature.Warmode = true;
                                creature.Say("*Roars in anger!*");
                                creature.FixedParticles(0x36BD, 1, 20, 9535, 37, 0, EffectLayer.Head);
                                creature.PlaySound(0x5A);
                            }
                        });
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be controlled.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
