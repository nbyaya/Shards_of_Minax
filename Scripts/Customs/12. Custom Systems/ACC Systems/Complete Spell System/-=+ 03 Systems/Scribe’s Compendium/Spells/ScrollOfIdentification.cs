using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class ScrollOfIdentification : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Scroll of Identification", "Identify!",
            21004, 9300 // Sound and Effect IDs for visual feedback
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 1.5; // Short delay for quick identification
        public override double RequiredSkill => 30.0; // Relatively low skill requirement
        public override int RequiredMana => 15; // Mana cost for the spell

        public ScrollOfIdentification(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ScrollOfIdentification m_Owner;

            public InternalTarget(ScrollOfIdentification owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    m_Owner.IdentifyItem(item);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target is not an item
                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void IdentifyItem(Item item)
        {
            if (!Caster.CanSee(item))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen
                FinishSequence();
                return;
            }

            // Use 'Item' directly as BaseItem is not a valid type
            // Add any custom identification logic here if needed
            item.InvalidateProperties();
            Caster.Mana -= RequiredMana;
            Effects.PlaySound(Caster.Location, Caster.Map, 0x20E); // Sound effect for successful identification
            Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5052); // Visual effect for identification

            Caster.SendMessage("You have successfully identified the item!");

            FinishSequence();
        }
    }
}
