using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class EscapeArtist : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Escape Artist", "Vas An Lor",
            //SpellCircle.First,
            21010,
            9209,
            false,
            Reagent.Bloodmoss,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle => SpellCircle.First;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 25.0;
        public override int RequiredMana => 15;

        public EscapeArtist(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x5C3); // Sound effect for casting the spell
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist); // Visual effect for escape attempt

                // Apply the effect of the Escape Artist ability
                Caster.SendMessage("You feel a surge of agility and cunning!");

                // Make the caster immune to paralyze or hold effects for a short duration
                Caster.BeginAction(typeof(EscapeArtist));
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => Caster.EndAction(typeof(EscapeArtist)));

                // Add automatic trap escape chance
                Caster.CheckSkill(SkillName.Lockpicking, 0.0, 100.0); // Bonus to lockpicking as part of escape

                // Notify nearby enemies
                List<Mobile> enemies = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m.Alive && Caster.CanBeHarmful(m, false))
                        enemies.Add(m);
                }

                foreach (Mobile enemy in enemies)
                {
                    enemy.SendMessage($"{Caster.Name} has vanished from your sight!");
                    enemy.FixedEffect(0x3735, 1, 15, 1109, 3); // Effect indicating confusion or losing sight
                }

                // Finish casting sequence
                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5); // Cooldown for the ability
        }
    }
}
