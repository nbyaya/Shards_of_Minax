using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class ThievingSwipe : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Thieving Swipe", "Swift as Shadow",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } } // Fast attack
        public override double RequiredSkill { get { return 30.0; } } // Lower skill requirement for a fast attack
        public override int RequiredMana { get { return 10; } }

        public ThievingSwipe(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ThievingSwipe m_Owner;

            public InternalTarget(ThievingSwipe owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckHSequence(target))
                    {
                        // Play attack animation and sound
                        from.RevealingAction();
                        from.PlaySound(0x1F7); // Sound of a swift attack
                        from.Animate(31, 5, 1, true, false, 0); // Quick attack animation

                        // Check for gold and steal a small amount
                        if (target.Backpack != null)
                        {
                            int goldStolen = Utility.RandomMinMax(5, 15); // Randomly steal between 5 to 15 gold

                            if (target.Backpack.ConsumeTotal(typeof(Gold), goldStolen))
                            {
                                from.SendMessage("You swiftly swipe some gold from your target!");
                                target.SendMessage("You've been robbed of some gold!");

                                // Add gold to the caster's backpack
                                from.Backpack.DropItem(new Gold(goldStolen));

                                // Display visual effect on target to show gold stealing
                                target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            }
                            else
                            {
                                from.SendMessage("Your target has no gold to steal.");
                            }
                        }

                        // Deal a small amount of damage
                        int damage = Utility.RandomMinMax(10, 20);
                        AOS.Damage(target, from, damage, 100, 0, 0, 0, 0); // Pure physical damage

                        // Visual effect on successful hit
                        target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head);
                        target.PlaySound(0x3B1); // Sound of a successful hit
                    }
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
