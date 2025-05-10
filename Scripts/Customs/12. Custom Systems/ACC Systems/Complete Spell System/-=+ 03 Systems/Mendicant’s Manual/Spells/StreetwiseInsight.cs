using System;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using Server.Spells;
using Server.Items; // Add this line

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class StreetwiseInsight : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Streetwise Insight", "Reveal Wealth",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 18; } }

        public StreetwiseInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private StreetwiseInsight m_Owner;

            public InternalTarget(StreetwiseInsight owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanSee(target) && target is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)target;
                        from.SendMessage($"You gain insight into {creature.Name}'s wealth.");

                        // Show flashy visual effects and sounds
                        Effects.SendLocationEffect(creature.Location, creature.Map, 0x376A, 10, 1, 1153, 4);
                        creature.FixedParticles(0x373A, 10, 30, 5052, EffectLayer.Waist);
                        creature.PlaySound(0x1F7);

                        // Create and display the gump with NPC stats
                        from.SendGump(new StreetwiseInsightGump(from, creature));
                    }
                    else
                    {
                        from.SendMessage("You cannot gain insight into that.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Owner.FinishSequence();
            }
        }

        private class StreetwiseInsightGump : Gump
        {
            private Mobile m_From;
            private BaseCreature m_Target;

            public StreetwiseInsightGump(Mobile from, BaseCreature target) : base(50, 50)
            {
                m_From = from;
                m_Target = target;

                AddPage(0);
                AddBackground(0, 0, 300, 400, 5054);
                AddLabel(20, 20, 1153, $"Insight on {m_Target.Name}");

                AddLabel(20, 50, 1152, $"Wealth: {GetWealthStatus(m_Target)}");
                AddLabel(20, 80, 1152, $"Hits: {m_Target.Hits}/{m_Target.HitsMax}");
                AddLabel(20, 110, 1152, $"Mana: {m_Target.Mana}/{m_Target.ManaMax}");
                AddLabel(20, 140, 1152, $"Stamina: {m_Target.Stam}/{m_Target.StamMax}");
                AddLabel(20, 170, 1152, $"Karma: {m_Target.Karma}");
                AddLabel(20, 200, 1152, $"Fame: {m_Target.Fame}");

                // Additional stats can be added here as needed.
            }

            private string GetWealthStatus(BaseCreature target)
            {
                if (target == null)
                    return "Unknown";

                // Calculate total amount of gold in the creature's backpack
                int gold = 0;
                foreach (Item item in target.Backpack.Items)
                {
                    if (item is Gold goldItem)
                    {
                        gold += goldItem.Amount;
                    }
                }

                if (gold >= 1000)
                    return "Very Wealthy";
                else if (gold >= 500)
                    return "Wealthy";
                else if (gold >= 100)
                    return "Moderate";
                else
                    return "Poor";
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
