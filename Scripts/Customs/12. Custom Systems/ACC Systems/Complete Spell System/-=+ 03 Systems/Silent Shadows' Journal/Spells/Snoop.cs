using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class Snoop : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Snoop", "Eavesdrop",
            21005,
            9204
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 20;

        public Snoop(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply effect to the caster
                Caster.FixedParticles(0x376A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x1E6);

                // Find nearby mobiles for eavesdropping
                List<Mobile> mobiles = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(10))
                {
                    if (m != Caster && m.Alive && m.CanBeHarmful(Caster) && Caster.CanSee(m))
                    {
                        mobiles.Add(m);
                    }
                }

                // Find hidden traps and objects within range
                List<Item> items = new List<Item>();
                foreach (Item item in Caster.GetItemsInRange(10))
                {
                    if (item is BaseTrap && item.Movable && Caster.CanSee(item))
                    {
                        items.Add(item);
                    }
                }

                // Show information for 10 seconds
                if (mobiles.Count > 0 || items.Count > 0)
                {
                    foreach (Mobile m in mobiles)
                    {
                        m.LocalOverheadMessage(MessageType.Regular, 0x3B2, true, "*You hear a faint conversation*");
                        m.PlaySound(0x1E6);
                    }

                    foreach (Item item in items)
                    {
                        item.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*You sense a hidden object*");
                        Effects.SendLocationEffect(item.Location, item.Map, 0x3709, 30, 10, 0, 0);
                        // Removed the PlaySound call for items
                    }
                }
                else
                {
                    Caster.SendMessage("You detect nothing unusual.");
                }

                Timer.DelayCall(TimeSpan.FromSeconds(10), EndEffect);
            }

            FinishSequence();
        }

        private void EndEffect()
        {
            Caster.SendMessage("The snoop effect has worn off.");
            Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            Caster.PlaySound(0x1E2);
        }

        // Removed CastCooldown override since it's not defined in StealthSpell
    }
}
