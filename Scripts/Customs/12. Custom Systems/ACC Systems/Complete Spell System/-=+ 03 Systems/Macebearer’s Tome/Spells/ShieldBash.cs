using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class ShieldBash : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shield Bash", "Impactus Clipes",
            //SpellCircle.Fourth,
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public ShieldBash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (SpellHelper.CheckTown(target, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Visual effect for the shield bash
                Effects.SendMovingEffect(Caster, target, 0x1FDD, 10, 0, false, false, 0, 0);
                Effects.PlaySound(target.Location, target.Map, 0x52D);

                if (Utility.RandomDouble() < 0.4) // 40% chance to disarm
                {
                    // Try to disarm the shield
                    BaseShield shield = target.FindItemOnLayer(Layer.TwoHanded) as BaseShield;
                    if (shield != null)
                    {
                        target.SendLocalizedMessage(1070754); // You have been disarmed!
                        shield.MoveToWorld(target.Location, target.Map); // Drop the shield on the ground
                    }
                }
                else
                {
                    // Reduce block chance temporarily
                    target.SendMessage("Your block chance has been reduced!");
                    // Assuming you have a way to track and modify block chance
                    // Example: target.BlockChance -= 10; 
                    // Note: You'll need to implement block chance tracking in your Mobile class or another system
                    
                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => 
                    {
                        target.SendMessage("Your block chance has been restored.");
                        // Example: target.BlockChance += 10; 
                        // Note: Ensure the block chance tracking system properly handles restoration
                    });
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ShieldBash m_Owner;

            public InternalTarget(ShieldBash owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
