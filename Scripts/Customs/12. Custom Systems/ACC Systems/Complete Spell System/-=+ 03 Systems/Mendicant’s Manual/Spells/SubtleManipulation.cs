using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class SubtleManipulation : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Subtle Manipulation", "Can you spare a dime?",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 18; } }

        public SubtleManipulation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SubtleManipulation m_Owner;

            public InternalTarget(SubtleManipulation owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target is BaseCreature && ((BaseCreature)target).ControlMaster == null)
                {
                    if (from.InRange(target, 2) && from.CanSee(target))
                    {
                        if (m_Owner.CheckSequence())
                        {
                            m_Owner.TryBeg(from, target);
                        }
                    }
                    else
                    {
                        from.SendMessage("You are too far away to beg from that target.");
                    }
                }
                else
                {
                    from.SendMessage("You can only beg from NPCs.");
                }

                m_Owner.FinishSequence();
            }
        }

        public void TryBeg(Mobile beggar, Mobile target)
        {
            if (beggar.Skills.Begging.Value < 20.0)
            {
                beggar.SendMessage("You are not skilled enough in the art of begging.");
                return;
            }

            double successChance = 0.2 + (beggar.Skills.Begging.Value / 500); // Base 20% chance plus skill modifier

            if (successChance > Utility.RandomDouble())
            {
                Item reward = GetRandomReward();
                beggar.AddToBackpack(reward);
                beggar.SendMessage("The NPC reluctantly gives you something.");
                PlayEffects(beggar, target);
            }
            else
            {
                beggar.SendMessage("The NPC refuses to give you anything.");
            }
        }

        private Item GetRandomReward()
        {
            List<Type> rewards = new List<Type>
            {
                typeof(Dagger),
                typeof(Broadsword),
                typeof(LeatherChest),
                typeof(ChainChest)
            };

            Type rewardType = rewards[Utility.Random(rewards.Count)];
            return (Item)Activator.CreateInstance(rewardType);
        }

        private void PlayEffects(Mobile beggar, Mobile target)
        {
            // Play sound and visual effects
            beggar.PlaySound(0x3E6); // Begging sound effect
            beggar.FixedParticles(0x373A, 1, 15, 5016, EffectLayer.Waist); // Magic sparkle effect
            target.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*looks reluctantly*");
        }
    }
}
