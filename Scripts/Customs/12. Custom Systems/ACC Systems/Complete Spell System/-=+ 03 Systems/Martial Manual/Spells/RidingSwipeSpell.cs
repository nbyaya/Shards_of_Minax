using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class RidingSwipeSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Riding Swipe", "Equus Ictus",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public RidingSwipeSpell(Mobile caster, Item scroll)
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
                    Caster.SendLocalizedMessage(1060848); // This attack only works on mounted targets
                }
                else if (!defender.Mounted && !defender.Flying && (!Core.ML || !Server.Spells.Ninjitsu.AnimalForm.UnderTransformation(defender)))
                {
                    Caster.SendLocalizedMessage(1060848); // This attack only works on mounted targets
                }
                else
                {
                    Caster.FixedEffect(0x3728, 10, 15);
                    Caster.PlaySound(0x2A1);

                    int amount = 10 + (int)(10.0 * (Caster.Skills[SkillName.Bushido].Value - 50.0) / 70.0 + 5);

                    if (!Caster.Mounted)
                    {
                        BlockMountType type = BlockMountType.RidingSwipe;
                        IMount mount = defender.Mount;

                        if (Core.SA)
                        {
                            if (defender.Flying)
                            {
                                type = BlockMountType.RidingSwipeFlying;
                            }
                            else if (mount is EtherealMount)
                            {
                                type = BlockMountType.RidingSwipeEthereal;
                            }
                        }

                        Server.Items.Dismount.DoDismount(Caster, defender, mount, 10, type);

                        if (mount is Mobile)
                        {
                            AOS.Damage((Mobile)mount, Caster, amount, 100, 0, 0, 0, 0);
                        }

                        defender.PlaySound(0x140);
                        defender.FixedParticles(0x3728, 10, 15, 9955, EffectLayer.Waist);
                    }
                    else
                    {
                        AOS.Damage(defender, Caster, amount, 100, 0, 0, 0, 0);

                        if (Server.Items.ParalyzingBlow.IsImmune(defender))
                        {
                            Caster.SendLocalizedMessage(1070804); // Your target resists paralysis.
                            defender.SendLocalizedMessage(1070813); // You resist paralysis.
                        }
                        else
                        {
                            defender.Paralyze(TimeSpan.FromSeconds(3.0));
                            Server.Items.ParalyzingBlow.BeginImmunity(defender, Server.Items.ParalyzingBlow.FreezeDelayDuration);
                        }
                    }
                }
            }

            FinishSequence();
        }
    }
}