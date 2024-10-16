using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class TransmutationElixir : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Transmutation Elixir", "Transmute!",
            //SpellCircle.Fourth,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        public TransmutationElixir(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TransmutationElixir m_Owner;

            public InternalTarget(TransmutationElixir owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseIngot ingot)
                {
                    if (ingot is IronIngot)
                    {
                        m_Owner.TransmuteIngot(ingot);
                    }
                    else
                    {
                        from.SendMessage("You can only transmute iron ingots into gold.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid item for transmutation.");
                }
            }
        }

        private void TransmuteIngot(BaseIngot ingot)
        {
            if (!Caster.CanSee(ingot))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                if (Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, ingot);

                // Play sound and visual effects
                Effects.PlaySound(ingot.Location, Caster.Map, 0x1F4); // Transmutation sound
                Effects.SendLocationParticles(EffectItem.Create(ingot.Location, ingot.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008, 0, 0, 0); // Transmutation effect

                // Transform the ingot
                Item newIngot = new GoldIngot(ingot.Amount);
                newIngot.MoveToWorld(ingot.Location, ingot.Map);
                ingot.Delete();

                Caster.SendMessage("You have successfully transmuted the iron ingots into gold!");
            }

            FinishSequence();
        }
    }
}
