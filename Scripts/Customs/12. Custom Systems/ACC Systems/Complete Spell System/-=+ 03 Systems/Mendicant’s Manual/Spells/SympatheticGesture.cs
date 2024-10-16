using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class SympatheticGesture : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Sympathetic Gesture", "Please sir a shirt?",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 14; } }

        public SympatheticGesture(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (m is BaseCreature && m.Alive && m.Body.IsHuman && !(m is PlayerMobile))
            {
                // Apply visual and sound effects
                Caster.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Waist);
                Caster.PlaySound(0x212);

                if (CheckSequence())
                {
                    // Random chance to receive clothing
                    int chance = Utility.Random(100);
                    if (chance < 50) // 50% chance to receive random clothing
                    {
                        Item clothing = GetRandomClothing();
                        if (clothing != null)
                        {
                            Caster.AddToBackpack(clothing);
                            Caster.SendMessage("You receive a piece of clothing from your sympathetic gesture!");
                        }
                        else
                        {
                            Caster.SendMessage("Your sympathetic gesture didn't yield any clothing this time.");
                        }
                    }
                    else
                    {
                        Caster.SendMessage("Your sympathetic gesture didn't yield any clothing this time.");
                    }
                }
            }
            else
            {
                Caster.SendMessage("You can only use this ability on a suitable NPC.");
            }

            FinishSequence();
        }

        private Item GetRandomClothing()
        {
            // List of possible clothing items
            Type[] clothingTypes = new Type[]
            {
                typeof(FancyShirt),
                typeof(Robe),
                typeof(Cloak),
                typeof(Skirt),
                typeof(BodySash),
                typeof(Doublet)
            };

            try
            {
                Type selectedType = clothingTypes[Utility.Random(clothingTypes.Length)];
                return (Item)Activator.CreateInstance(selectedType);
            }
            catch
            {
                return null;
            }
        }

        private class InternalTarget : Target
        {
            private SympatheticGesture m_Owner;

            public InternalTarget(SympatheticGesture owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendMessage("You must target an NPC.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
