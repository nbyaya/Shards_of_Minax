using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class Wildcraft : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wildcraft", "Nature's Bounty",
            21005, // Sound ID
            9400,  // Effect ID
            false, // Allow buff icon
            new Type[] { typeof(RawRibs) } // Required reagent
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 25;

        public Wildcraft(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!HasReagent(Caster, typeof(RawRibs)))
            {
                Caster.SendMessage("You must have RawRibs in your inventory to cast this spell.");
                return;
            }

            Caster.Target = new InternalTarget(this);
        }

        private bool HasReagent(Mobile caster, Type reagentType)
        {
            return caster.Backpack != null && caster.Backpack.FindItemByType(reagentType) != null;
        }

        private class InternalTarget : Target
        {
            private Wildcraft m_Owner;

            public InternalTarget(Wildcraft owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (m_Owner.CheckSequence())
                {
                    if (m_Owner.Caster.Backpack != null && m_Owner.Caster.Backpack.ConsumeTotal(typeof(RawRibs), 1))
                    {
                        // Play casting effects
                        m_Owner.Caster.PlaySound(0x5C3); // Custom sound
                        m_Owner.Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head);

                        // Randomly generate a resource item
                        Item resource = CreateRandomResource();
                        if (resource != null)
                        {
                            m_Owner.Caster.Backpack.DropItem(resource);
                            m_Owner.Caster.SendMessage("You have created a " + resource.Name + " from your Wildcrafting skill!");
                        }
                        else
                        {
                            m_Owner.Caster.SendMessage("The spell fizzled and nothing happened.");
                        }
                    }
                    else
                    {
                        m_Owner.Caster.SendMessage("You must have RawRibs in your inventory to cast this spell.");
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }

            private Item CreateRandomResource()
            {
                Type[] resourceTypes = new Type[]
                {
                    typeof(Board),
                    typeof(IronOre),
                    typeof(Leather),
                    typeof(Feather),
                    typeof(Cloth),
                    typeof(Bone)
                };

                Type resourceType = resourceTypes[Utility.Random(resourceTypes.Length)];
                return Activator.CreateInstance(resourceType) as Item;
            }
        }
    }
}
