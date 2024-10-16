using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class DismountSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dismount", "Eicio Equitem",
            212,
            9041
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public static readonly TimeSpan DefenderRemountDelay = TimeSpan.FromSeconds(10.0);
        public static readonly TimeSpan AttackerRemountDelay = TimeSpan.FromSeconds(3.0);

        public DismountSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if ((Caster.Mounted || Caster.Flying) && !(Caster.Weapon is Lance) && !(Caster.Weapon is GargishLance))
            {
                Caster.SendLocalizedMessage(1061283); // You cannot perform that attack while mounted or flying!
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private DismountSpell m_Owner;

            public InternalTarget(DismountSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile defender = (Mobile)o;
                    if (defender is ChaosDragoon || defender is ChaosDragoonElite)
                        return;

                    IMount mount = defender.Mount;

                    if (mount == null && !defender.Flying && (!Core.ML || !Server.Spells.Ninjitsu.AnimalForm.UnderTransformation(defender)))
                    {
                        from.SendLocalizedMessage(1060848); // This attack only works on mounted or flying targets
                        return;
                    }

                    defender.PlaySound(0x140);
                    defender.FixedParticles(0x3728, 10, 15, 9955, EffectLayer.Waist);

                    int delay = 10;

                    DoDismount(from, defender, mount, delay);

                    if (!from.Mounted)
                    {
                        AOS.Damage(defender, from, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);
                    }
                }
            }
        }

        public static void DoDismount(Mobile attacker, Mobile defender, IMount mount, int delay)
        {
            attacker.SendLocalizedMessage(1060082); // The force of your attack has dislodged them from their mount!

            if (defender is PlayerMobile)
            {
                if (Core.ML && Server.Spells.Ninjitsu.AnimalForm.UnderTransformation(defender))
                {
                    defender.SendLocalizedMessage(1114066, attacker.Name); // ~1_NAME~ knocked you out of animal form!
                }
                else if (defender.Flying)
                {
                    defender.SendLocalizedMessage(1113590, attacker.Name); // You have been grounded by ~1_NAME~!
                }
                else if (defender.Mounted)
                {
                    defender.SendLocalizedMessage(1060083); // You fall off of your mount and take damage!
                }

                ((PlayerMobile)defender).SetMountBlock(BlockMountType.Dazed, TimeSpan.FromSeconds(delay), true);
            }
            else if (mount != null)
            {
                mount.Rider = null;
            }

            if (attacker is PlayerMobile)
            {
                ((PlayerMobile)attacker).SetMountBlock(BlockMountType.DismountRecovery, TimeSpan.FromSeconds(10), false);
            }
            else if (Core.ML && attacker is BaseCreature)
            {
                BaseCreature bc = attacker as BaseCreature;

                if (bc.ControlMaster is PlayerMobile)
                {
                    PlayerMobile pm = bc.ControlMaster as PlayerMobile;

                    pm.SetMountBlock(BlockMountType.DismountRecovery, TimeSpan.FromSeconds(delay), false);
                }
            }
        }
    }
}