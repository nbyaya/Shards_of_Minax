using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
    public class ForageToolsSpell : ForagersGuidebookSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forage Tools", "Gathering",
            212,
            9041,
            CReagent.Kindling
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 3.0; } } // 3 seconds casting delay
        public override double RequiredSkill { get { return 50.0; } } // Minimum required skill
        public override int RequiredMana { get { return 10; } } // Mana cost

        private static Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

        public ForageToolsSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head);
                Caster.PlaySound(0x244);
                Caster.SendMessage("You begin to forage for tools...");
                Caster.Frozen = true;
                Caster.Animate(32, 5, 1, true, false, 0); // Start the animation

                Timer animationTimer = new AnimationTimer(Caster);
                animationTimer.Start();

                Timer timer = new InternalTimer(Caster, this, animationTimer);
                timer.Start();
                m_Timers[Caster] = timer;
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;
            private ForageToolsSpell m_Spell;
            private Timer m_AnimationTimer;

            public InternalTimer(Mobile caster, ForageToolsSpell spell, Timer animationTimer)
                : base(TimeSpan.FromSeconds(5.0)) // 30 seconds foraging time
            {
                m_Caster = caster;
                m_Spell = spell;
                m_AnimationTimer = animationTimer;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Spell == null || m_Caster.Deleted)
                    return;

                m_Caster.Frozen = false;
                m_Caster.SendMessage("You have finished foraging.");
                m_AnimationTimer.Stop(); // Stop the animation timer

                double successChance = m_Caster.Skills[SkillName.Camping].Value / 100.0;
                if (successChance > Utility.RandomDouble())
                {
                    List<Item> tools = GetRandomTools(m_Caster.Skills[SkillName.Camping].Value);
                    foreach (var tool in tools)
                    {
                        m_Caster.AddToBackpack(tool);
                        m_Caster.SendMessage("You successfully foraged a " + tool.Name + "!");
                    }
                }
                else
                {
                    m_Caster.SendMessage("You failed to forage any useful tools.");
                }

                m_Timers.Remove(m_Caster);
            }

            private List<Item> GetRandomTools(double skill)
            {
                List<Item> tools = new List<Item>();

                // Always get the lowest tier tool
                tools.Add(CreateRandomTool(Tier1Tools));

                // Add additional tools based on skill level
                if (skill >= 70.0)
                {
                    tools.Add(CreateRandomTool(Tier2Tools));
                }
                if (skill >= 80.0)
                {
                    tools.Add(CreateRandomTool(Tier3Tools));
                }
                if (skill >= 90.0)
                {
                    tools.Add(CreateRandomTool(Tier4Tools));
                }

                return tools;
            }

            private Item CreateRandomTool(Type[] toolTypes)
            {
                Type toolType = toolTypes[Utility.Random(toolTypes.Length)];
                return (Item)Activator.CreateInstance(toolType);
            }

            private static readonly Type[] Tier1Tools = new Type[]
            {
                typeof(TinkerTools),
				typeof(Dagger),
				typeof(SkullCap),
				typeof(FloppyHat),
				typeof(BlankScroll),
                typeof(Shovel)
            };

            private static readonly Type[] Tier2Tools = new Type[]
            {
                typeof(Hatchet),
				typeof(Bottle),
				typeof(Bandana),
				typeof(RecallRune),
				typeof(Bedroll),
                typeof(SewingKit)
            };

            private static readonly Type[] Tier3Tools = new Type[]
            {
                typeof(FletcherTools),
				typeof(Scissors),
				typeof(Drums),
				typeof(OilFlask),
				typeof(Bag),
                typeof(MortarPestle)
            };

            private static readonly Type[] Tier4Tools = new Type[]
            {
                typeof(WizardsHat),
				typeof(Candle),
				typeof(Pouch),
				typeof(BlankMap),
				typeof(HousePlacementTool),
				typeof(Dyes),
				typeof(DyeTub),
				typeof(SmithHammer)
            };
        }

        private class AnimationTimer : Timer
        {
            private Mobile m_Caster;

            public AnimationTimer(Mobile caster)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster != null && !m_Caster.Deleted)
                {
                    m_Caster.Animate(32, 5, 1, true, false, 0); // Loop the animation
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
