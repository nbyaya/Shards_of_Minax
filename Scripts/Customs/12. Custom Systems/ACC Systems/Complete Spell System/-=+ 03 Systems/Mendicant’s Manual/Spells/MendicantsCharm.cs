using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class MendicantsCharm : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mendicants Charm", "Beg for Food",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 14; } }

        public MendicantsCharm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MendicantsCharm m_Owner;

            public InternalTarget(MendicantsCharm owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile mobile && IsNpc(mobile))
                {
                    m_Owner.Beg(mobile);
                }
                else
                {
                    from.SendMessage("You can only beg from NPCs.");
                    m_Owner.FinishSequence();
                }
            }

            private bool IsNpc(Mobile mobile)
            {
                // Check if the Mobile is a BaseCreature and not a player
                return mobile is BaseCreature && !mobile.Player;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Beg(Mobile npc)
        {
            if (npc == null || npc.Deleted || !Caster.InRange(npc, 10))
            {
                Caster.SendMessage("That NPC is not close enough.");
                return;
            }

            if (!npc.Alive || !Caster.CanSee(npc))
            {
                Caster.SendMessage("You cannot beg from this NPC.");
                return;
            }

            if (CheckSequence())
            {
                Caster.Animate(34, 5, 1, true, false, 0);
                Caster.PlaySound(0x2E); // Begging sound

                // Chance to gain food item
                if (Utility.RandomDouble() < 0.3 + (Caster.Skills[SkillName.Begging].Value / 100.0))
                {
                    Item foodItem = GetRandomFoodItem();
                    Caster.AddToBackpack(foodItem);
                    Caster.SendMessage("The NPC feels pity and gives you some food.");
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 1, 1153, 3); // Visual effect
                }
                else
                {
                    Caster.SendMessage("The NPC ignores your begging.");
                }
            }

            FinishSequence();
        }

        private Item GetRandomFoodItem()
        {
            Type[] foodTypes = new Type[]
            {
                typeof(BreadLoaf),
                typeof(CheeseWheel),
                typeof(CookedBird),
                typeof(Apple),
                typeof(Peach)
            };

            return (Item)Activator.CreateInstance(foodTypes[Utility.Random(foodTypes.Length)]);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
