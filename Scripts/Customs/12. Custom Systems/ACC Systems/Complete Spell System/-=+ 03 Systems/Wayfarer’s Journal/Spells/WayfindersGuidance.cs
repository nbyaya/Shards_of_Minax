using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class WayfindersGuidance : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wayfinder's Guidance", "Revelo!",
            21005, 9301, false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Arbitrary choice; adjust as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 50; } }

        public WayfindersGuidance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(WayfindersGuidance)))
            {
                Caster.SendMessage("You must wait before using this ability again.");
                return;
            }

            if (CheckSequence())
            {
                Caster.SendMessage("You feel the path ahead become clearer.");
                Caster.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist);
                Caster.PlaySound(0x1FD);

                Map map = Caster.Map;

                if (map != null)
                {
                    ArrayList toReveal = new ArrayList();
                    foreach (Item item in Caster.GetItemsInRange(10))
                    {
                        if (item is BaseTrap && !item.Visible)
                        {
                            toReveal.Add(item);
                        }
                    }

                    foreach (Mobile mob in Caster.GetMobilesInRange(10))
                    {
                        if (mob.Hidden && mob != Caster && mob.AccessLevel == AccessLevel.Player)
                        {
                            toReveal.Add(mob);
                        }
                    }

                    foreach (object obj in toReveal)
                    {
                        if (obj is Item item)
                        {
                            item.Visible = true;
                            // FixedParticles replaced with SendLocationParticles
                            Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x375A, 9, 32, 5030);
                        }
                        else if (obj is Mobile mob)
                        {
                            mob.RevealingAction();
                            mob.FixedEffect(0x37C4, 1, 12);
                            mob.PlaySound(0x1F2);
                        }
                    }
                }

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerCallback(EndEffect));
                Caster.BeginAction(typeof(WayfindersGuidance));
                Timer.DelayCall(TimeSpan.FromMinutes(10.0), new TimerStateCallback(ReleaseLock), Caster);
            }

            FinishSequence();
        }

        private void EndEffect()
        {
            Caster.SendMessage("The guidance fades away.");
        }

        private static void ReleaseLock(object state)
        {
            Mobile m = state as Mobile;
            if (m != null)
            {
                m.EndAction(typeof(WayfindersGuidance));
                m.SendMessage("You may now use Wayfinder's Guidance again.");
            }
        }
    }
}
