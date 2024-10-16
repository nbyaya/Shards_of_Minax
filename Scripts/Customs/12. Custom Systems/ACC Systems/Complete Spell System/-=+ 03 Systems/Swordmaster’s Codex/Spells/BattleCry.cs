using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class BattleCry : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Battle Cry", "HE YA!",
            21012, // Animation ID
            9411   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public BattleCry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.SendMessage("You let out a fierce battle cry!");

            Effects.PlaySound(Caster.Location, Caster.Map, 0x2F4); // Sound for intimidation
            Caster.FixedParticles(0x3728, 10, 15, 5032, 0); // Reverted to int for the effect layer

            IEnumerable<Mobile> nearbyMobiles = Caster.GetMobilesInRange(5);

            foreach (Mobile mobile in nearbyMobiles)
            {
                if (mobile == Caster)
                    continue;

                if (Caster.CanBeBeneficial(mobile, false))
                {
                    if (mobile is PlayerMobile && Caster.InRange(mobile, 5))
                    {
                        // Bolster Allies: Increase Hits, Mana, and Stamina for a short duration
                        mobile.SendMessage("You feel invigorated by the battle cry!");
                        mobile.FixedEffect(0x376A, 10, 16); // Visual effect for buff
                        mobile.Hits += 10;
                        mobile.Mana += 10;
                        mobile.Stam += 10;

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            mobile.Hits -= 10;
                            mobile.Mana -= 10;
                            mobile.Stam -= 10;
                        });
                    }
                }
                else if (Caster.CanBeHarmful(mobile))
                {
                    if (mobile is BaseCreature && Caster.InRange(mobile, 5))
                    {
                        BaseCreature creature = (BaseCreature)mobile;

                        // Intimidate Foes: Lower attack speed and defense for a short duration
                        mobile.SendMessage("You feel intimidated by the battle cry!");
                        mobile.FixedEffect(0x3728, 10, 15, 1153, 0); // Reverted to int for the effect layer
                        mobile.Dex -= 10;

                        // Lower physical resistance using ResistanceMod
                        ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -10);
                        creature.AddResistanceMod(mod);

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            mobile.Dex += 10;
                            creature.RemoveResistanceMod(mod); // Restore physical resistance
                        });
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
