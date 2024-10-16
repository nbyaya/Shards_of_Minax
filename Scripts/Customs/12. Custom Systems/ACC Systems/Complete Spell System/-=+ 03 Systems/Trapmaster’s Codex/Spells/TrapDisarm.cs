using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapDisarm : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Disarm", "Disarm it Safely!",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public TrapDisarm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapDisarm m_Owner;

            public InternalTarget(TrapDisarm owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    m_Owner.Target((IPoint3D)targeted);
                }
                else
                {
                    from.SendMessage("You must target a trap.");
                    m_Owner.FinishSequence();
                }
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
                SpellHelper.GetSurfaceTop(ref p);

                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x36B0, 30, 10, 0, 0);
                Effects.PlaySound(p, Caster.Map, 0x1F0);

                TrapEffect(p);
            }

            FinishSequence();
        }

        private void TrapEffect(IPoint3D p)
        {
            // Check if the targeted location has a trap
            foreach (Item item in Caster.Map.GetItemsInRange(new Point3D(p), 0))
            {
                if (item is BaseTrap trap)
                {
                    if (trap.IsLockedDown)
                    {
                        Caster.SendMessage("You cannot disarm a secured trap.");
                        return;
                    }

                    if (Caster.CheckSkill(SkillName.Tracking, 0.0, 100.0)) // Adjust the skill name as necessary
                    {
                        // Custom disarm logic here
                        // For example, removing the trap item or similar
                        Caster.SendMessage("You successfully disarmed the trap!");

                        // Flashy Effect and Sound
                        Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x374A, 10, 15, 5023);
                        Effects.PlaySound(trap.Location, trap.Map, 0x3E3);

                        trap.Delete(); // Adjust according to your disarm logic
                    }
                    else
                    {
                        Caster.SendMessage("You failed to disarm the trap.");
                        // Trigger method does not exist; handle failure appropriately
                    }
                }
                else
                {
                    Caster.SendMessage("There is no trap here.");
                }
            }
        }
    }
}
