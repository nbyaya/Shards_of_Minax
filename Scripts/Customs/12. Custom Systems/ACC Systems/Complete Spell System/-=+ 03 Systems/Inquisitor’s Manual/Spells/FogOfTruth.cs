using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class FogOfTruth : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Fog of Truth", "Vas Wis Rel Por",
                                                        // SpellCircle.Fifth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public FogOfTruth(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Create the mist effect
                Caster.PlaySound(0x64F); // Sound effect for the mist
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 20, 1153, 4, 9962, 0); // Mist visual effect

                // Reveal hidden entities in range
                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(Caster.Location, 5); // Radius of 5 tiles

                foreach (Mobile m in eable)
                {
                    if (m.Hidden && m != Caster && Caster.CanSee(m))
                    {
                        m.RevealingAction();
                        m.FixedParticles(0x375A, 10, 15, 5036, EffectLayer.Head); // Particle effect on the revealed entity
                        m.SendMessage("You have been revealed by the Fog of Truth!");
                    }
                }

                eable.Free();

                // Reveal hidden traps
                RevealTraps(Caster.Location, Caster.Map, 5); // Call to reveal traps in the area

                Caster.SendMessage("A mist surrounds you, revealing all that was hidden.");
            }

            FinishSequence();
        }

        private void RevealTraps(Point3D loc, Map map, int range)
        {
            List<Item> items = new List<Item>();
            IPooledEnumerable eable = map.GetItemsInRange(loc, range);

            foreach (Item item in eable)
            {
                if (item is BaseTrap && item.Visible == false)
                {
                    item.Visible = true;
                    item.PublicOverheadMessage(MessageType.Regular, 0x22, false, "A hidden trap is revealed!");
                    Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x375A, 10, 15, 5036, 4, 9962, 0); // Particle effect on the trap
                }
            }

            eable.Free();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
