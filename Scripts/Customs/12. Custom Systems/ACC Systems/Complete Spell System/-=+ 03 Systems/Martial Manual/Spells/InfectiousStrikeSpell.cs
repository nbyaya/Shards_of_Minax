using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class InfectiousStrikeSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Infectious Strike", "Noxfero",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public InfectiousStrikeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedEffect(0x3728, 10, 15);
                Caster.PlaySound(0xDD);

                Mobile defender = Caster.Combatant as Mobile;

                if (defender == null || defender.Deleted || defender.Map != Caster.Map || !defender.Alive || !Caster.CanSee(defender) || !Caster.CanBeHarmful(defender))
                {
                    Caster.SendLocalizedMessage(1061634); // You cannot attack that which you cannot see.
                }
                else
                {
                    BaseWeapon weapon = Caster.Weapon as BaseWeapon;

                    if (weapon == null)
                        return;

                    Poison p = weapon.Poison;

                    if (p == null || weapon.PoisonCharges <= 0)
                    {
                        Caster.SendLocalizedMessage(1061141); // Your weapon must have a dose of poison to perform an infectious strike!
                        return;
                    }

                    int noChargeChance = Server.Spells.SkillMasteries.MasteryInfo.NonPoisonConsumeChance(Caster);

                    if (noChargeChance == 0 || noChargeChance < Utility.Random(100))
                        --weapon.PoisonCharges;
                    else
                        Caster.SendLocalizedMessage(1156095); // Your mastery of poisoning allows you to use your poison charge without consuming it.

                    int maxLevel = 0;
                    if (p == Poison.DarkGlow)
                    {
                        maxLevel = 10 + (Caster.Skills[SkillName.Poisoning].Fixed / 333);
                        if (maxLevel > 13)
                            maxLevel = 13;
                    }
                    else if (p == Poison.Parasitic)
                    {
                        maxLevel = 14 + (Caster.Skills[SkillName.Poisoning].Fixed / 250);
                        if (maxLevel > 18)
                            maxLevel = 18;
                    }
                    else            
                    {
                        maxLevel = Caster.Skills[SkillName.Poisoning].Fixed / 200;
                        if (maxLevel > 5)
                            maxLevel = 5;
                    }
                    
                    if (maxLevel < 0)
                        maxLevel = 0;
                    if (p.Level > maxLevel)
                        p = Poison.GetPoison(maxLevel);

                    if ((Caster.Skills[SkillName.Poisoning].Value / 100.0) > Utility.RandomDouble())
                    {
                        if (p != null && p.Level + 1 <= maxLevel)
                        {
                            int level = p.Level + 1;
                            Poison newPoison = Poison.GetPoison(level);
                   
                            if (newPoison != null)
                            {
                                p = newPoison;

                                Caster.SendLocalizedMessage(1060080); // Your precise strike has increased the level of the poison by 1
                                defender.SendLocalizedMessage(1060081); // The poison seems extra effective!
                            }
                        }
                    }

                    defender.FixedParticles(0x3728, 244, 25, 9941, 1266, 0, EffectLayer.Waist);

                    if (defender.ApplyPoison(Caster, p) != ApplyPoisonResult.Immune)
                    {
                        Caster.SendLocalizedMessage(1008096, true, defender.Name); // You have poisoned your target : 
                        defender.SendLocalizedMessage(1008097, false, Caster.Name); //  : poisoned you!
                    }

                    Caster.RevealingAction();
                }
            }

            FinishSequence();
        }
    }
}