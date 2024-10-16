using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class DisrobeSpell : MartialManualSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disrobe", "Exporso",
            212,
            9041
        );

        public static readonly TimeSpan BlockEquipDuration = TimeSpan.FromSeconds(5.0);

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public DisrobeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedEffect(0x3728, 10, 15);
                Caster.PlaySound(0x2A1);

                Mobile target = Caster.Combatant as Mobile;

                if (target == null || !Caster.CanSee(target) || !Caster.CanBeHarmful(target))
                {
                    Caster.SendLocalizedMessage(1062002); // You cannot disrobe your opponent.
                    FinishSequence();
                    return;
                }

                if (Caster.Mana >= RequiredMana)
                {
                    Caster.Mana -= RequiredMana;

                    Item toDisrobe = target.FindItemOnLayer(Layer.InnerTorso);

                    if (toDisrobe == null || !toDisrobe.Movable)
                        toDisrobe = target.FindItemOnLayer(Layer.OuterTorso);

                    Container pack = target.Backpack;

                    if (pack == null || toDisrobe == null || !toDisrobe.Movable)
                    {
                        Caster.SendLocalizedMessage(1062002); // You cannot disrobe your opponent.
                    }
                    else
                    {
                        target.SendLocalizedMessage(1062002); // You can no longer wear your ~1_ARMOR~
                        target.PlaySound(0x3B9);

                        pack.DropItem(toDisrobe);

                        BaseWeapon.BlockEquip(target, BlockEquipDuration);
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(1060187); // You lack the mana required to cast this spell.
                }
            }

            FinishSequence();
        }
    }
}
