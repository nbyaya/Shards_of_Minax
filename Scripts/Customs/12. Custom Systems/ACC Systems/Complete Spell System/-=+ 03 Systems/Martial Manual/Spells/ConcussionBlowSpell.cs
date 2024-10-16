using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class ConcussionBlowSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Concussion Blow", "Kal Vas Grav",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public ConcussionBlowSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile defender = Caster.Combatant as Mobile;

                if (defender == null)
                {
                    Caster.SendLocalizedMessage(1060165); // You have delivered a concussion!
                    return;
                }

                if (!Caster.CanBeHarmful(defender) || !Caster.InRange(defender, 1))
                {
                    Caster.SendLocalizedMessage(1060164); // You are too far away to perform that attack!
                    return;
                }

                Caster.SendLocalizedMessage(1060165); // You have delivered a concussion!
                defender.SendLocalizedMessage(1060166); // You feel disoriented!

                defender.PlaySound(0x213);
                defender.FixedParticles(0x377A, 1, 32, 9949, 1153, 0, EffectLayer.Head);

                Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 10), defender.Map), new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 20), defender.Map), 0x36FE, 1, 0, false, false, 1133, 3, 9501, 1, 0, EffectLayer.Waist, 0x100);

                int damage = 10; // Base damage is 10.

                if (defender.HitsMax > 0) 
                {
                    double hitsPercent = ((double)defender.Hits / (double)defender.HitsMax) * 100.0;

                    double manaPercent = 0;

                    if (defender.ManaMax > 0)
                        manaPercent = ((double)defender.Mana / (double)defender.ManaMax) * 100.0;

                    damage += Math.Min((int)(Math.Abs(hitsPercent - manaPercent) / 4), 20);
                }

                // Total damage is 10 + (0~20) = 10~30, physical, non-resistable.

                Caster.DoHarmful(defender);
                SpellHelper.Damage(this, defender, damage, 100, 0, 0, 0, 0);
            }

            FinishSequence();
        }
    }
}