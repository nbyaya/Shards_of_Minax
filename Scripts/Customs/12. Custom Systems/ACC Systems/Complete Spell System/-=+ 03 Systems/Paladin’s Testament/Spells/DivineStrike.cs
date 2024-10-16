using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class DivineStrike : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Divine Strike", "Divinum Impetus",
            21001,
            9300,
            false,
            Reagent.Garlic
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public DivineStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DivineStrike m_Owner;

            public InternalTarget(DivineStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Base damage of the attack
                        double damage = Utility.RandomMinMax(15, 25);

                        // Check if the target is undead or evil
                        if (target is BaseCreature creature && (IsEvil(creature) || IsUndead(creature)))
                        {
                            // Additional damage to undead or evil creatures
                            damage *= 1.5;
                            Effects.SendTargetEffect(target, 0x376A, 10, 16); // Holy light effect
                        }

                        // Play sound and visual effect
                        Effects.PlaySound(target.Location, target.Map, 0x208); // Holy strike sound
                        target.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Holy aura effect

                        // Apply the damage
                        AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                        // Potential knockback effect (flashy visual)
                        if (Utility.RandomDouble() < 0.3) // 30% chance to knockback
                        {
                            int distance = Utility.Random(2, 3);
                            int offsetX = 0, offsetY = 0;

                            // Calculate offset based on the direction
                            switch (from.Direction)
                            {
                                case Direction.North:
                                    offsetY = -distance;
                                    break;
                                case Direction.South:
                                    offsetY = distance;
                                    break;
                                case Direction.East:
                                    offsetX = distance;
                                    break;
                                case Direction.West:
                                    offsetX = -distance;
                                    break;
                                case Direction.Up:
                                    offsetX = -distance;
                                    offsetY = -distance;
                                    break;
                                case Direction.Down:
                                    offsetX = distance;
                                    offsetY = distance;
                                    break;
                                case Direction.Left:
                                    offsetX = -distance;
                                    offsetY = distance;
                                    break;
                                case Direction.Right:
                                    offsetX = distance;
                                    offsetY = -distance;
                                    break;
                            }

                            Point3D dest = new Point3D(target.X + offsetX, target.Y + offsetY, target.Z);
                            
                            if (target.Map.CanFit(dest, 16, false, false))
                            {
                                target.Location = dest;
                                target.ProcessDelta();
                                target.Combatant = from; // Ensure the target fights back
                                Effects.SendLocationEffect(dest, target.Map, 0x3728, 10); // Flashy explosion effect
                            }
                        }
                    }
                }
                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private static bool IsEvil(BaseCreature creature)
        {
            // Replace with actual logic for determining if a creature is evil
            return false; // Placeholder
        }

        private static bool IsUndead(BaseCreature creature)
        {
            // Replace with actual logic for determining if a creature is undead
            return false; // Placeholder
        }
    }
}
