using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ForagersGuidebook
{
    public class ForagersGuidebookForageReagentsSpell : ForagersGuidebookSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forage Reagents", " ",
            212,
            9041,
            CReagent.Kindling
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0; } }
        public override double RequiredSkill { get { return 0; } }
        public override int RequiredMana { get { return 10; } }

        private static Dictionary<Mobile, Timer> m_Timers = new Dictionary<Mobile, Timer>();

        public ForagersGuidebookForageReagentsSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (Caster.CanBeginAction(typeof(ForagersGuidebookForageReagentsSpell)) && CheckSequence())
            {
                Caster.BeginAction(typeof(ForagersGuidebookForageReagentsSpell));
                Caster.SendMessage("You begin foraging for reagents...");
                Caster.Frozen = true;
                Caster.Animate(32, 5, 1, true, false, 0); // Start the animation

                Timer animationTimer = new AnimationTimer(Caster);
                animationTimer.Start();

                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5013);

                Timer timer = new InternalTimer(Caster, TimeSpan.FromSeconds(5), animationTimer);
                timer.Start();
                m_Timers[Caster] = timer;
            }
            else
            {
                Caster.SendMessage("You cannot forage for reagents in that state!");
            }
        }

        private void ResolveForage(Mobile caster)
        {
            double campingSkill = caster.Skills[SkillName.Camping].Value;
            int minReagents = (int)(campingSkill / 20);
            int maxReagents = (int)(campingSkill / 10);
            int amount = Utility.RandomMinMax(minReagents, maxReagents);

            List<Type> tier1 = new List<Type> { typeof(Garlic), typeof(Ginseng), typeof(SpidersSilk), typeof(SulfurousAsh) };
            List<Type> tier2 = new List<Type> { typeof(BlackPearl), typeof(Bloodmoss), typeof(MandrakeRoot), typeof(Nightshade) };
            List<Type> tier3 = new List<Type> { typeof(GraveDust), typeof(DaemonBlood), typeof(BatWing), typeof(NoxCrystal) };
            List<Type> tier4 = new List<Type> { typeof(Bone), typeof(PigIron), typeof(DaemonBone), typeof(DragonBlood) };

            List<Type> availableReagents = new List<Type>(tier1);

            if (campingSkill >= 70)
            {
                availableReagents.AddRange(tier2);
            }
            if (campingSkill >= 80)
            {
                availableReagents.AddRange(tier3);
            }
            if (campingSkill >= 90)
            {
                availableReagents.AddRange(tier4);
            }

            Type reagentType = availableReagents[Utility.Random(availableReagents.Count)];
            Item reagent = (Item)Activator.CreateInstance(reagentType, new object[] { amount });

            caster.AddToBackpack(reagent);
            caster.SendMessage(String.Format("You successfully forage {0} {1}(s).", amount, reagent.Name));
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;
            private DateTime m_End;
            private Timer m_AnimationTimer;

            public InternalTimer(Mobile caster, TimeSpan duration, Timer animationTimer)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            {
                m_Caster = caster;
                m_End = DateTime.Now + duration;
                m_AnimationTimer = animationTimer;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End)
                {
                    m_Caster.Frozen = false;
                    m_Caster.EndAction(typeof(ForagersGuidebookForageReagentsSpell));
                    m_AnimationTimer.Stop(); // Stop the animation timer

                    ResolveForage(m_Caster);

                    m_Timers.Remove(m_Caster);
                    Stop();
                }
            }

            private void ResolveForage(Mobile caster)
            {
                double campingSkill = caster.Skills[SkillName.Camping].Value;
                int minReagents = (int)(campingSkill / 20);
                int maxReagents = (int)(campingSkill / 10);
                int amount = Utility.RandomMinMax(minReagents, maxReagents);

                List<Type> tier1 = new List<Type> { typeof(Garlic), typeof(Ginseng), typeof(SpidersSilk), typeof(SulfurousAsh) };
                List<Type> tier2 = new List<Type> { typeof(BlackPearl), typeof(Bloodmoss), typeof(MandrakeRoot), typeof(Nightshade) };
                List<Type> tier3 = new List<Type> { typeof(GraveDust), typeof(DaemonBlood), typeof(BatWing), typeof(NoxCrystal) };
                List<Type> tier4 = new List<Type> { typeof(Bone), typeof(PigIron), typeof(DaemonBone), typeof(DragonBlood) };

                List<Type> availableReagents = new List<Type>(tier1);

                if (campingSkill >= 70)
                {
                    availableReagents.AddRange(tier2);
                }
                if (campingSkill >= 80)
                {
                    availableReagents.AddRange(tier3);
                }
                if (campingSkill >= 90)
                {
                    availableReagents.AddRange(tier4);
                }

                Type reagentType = availableReagents[Utility.Random(availableReagents.Count)];
                Item reagent = (Item)Activator.CreateInstance(reagentType, new object[] { amount });

                caster.AddToBackpack(reagent);
                caster.SendMessage(String.Format("You successfully forage {0} {1}(s).", amount, reagent.Name));
            }
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
