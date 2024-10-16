using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class IllusionaryDoubleSpell : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Illusionary Double", "Illusionem Gemina",
            // SpellCircle.Fourth,
            21011, // Animation ID
            9210,  // Sound ID
            false
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 0.2; // 2-second cast delay
        public override double RequiredSkill => 50.0; // Skill requirement
        public override int RequiredMana => 25; // Mana requirement

        public IllusionaryDoubleSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You create an illusionary double of yourself!");

                // Visual and Sound Effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1153, 0, 5029, 0);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FD);

                // Summon the illusion
                IllusionaryDouble illusion = new IllusionaryDouble(Caster);
                illusion.MoveToWorld(Caster.Location, Caster.Map);

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), illusion.Delete); // Illusion lasts for 10 seconds
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // 5-second cooldown after casting
        }
    }

    public class IllusionaryDouble : Mobile
    {
        private Mobile m_Caster;

        public IllusionaryDouble(Mobile caster)
        {
            m_Caster = caster;
            Body = caster.Body;
            Hue = caster.Hue;
            Name = caster.Name;
            Female = caster.Female;

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete); // Auto-delete after 10 seconds

            // Make the illusion look exactly like the caster
            for (int i = 0; i < caster.Items.Count; ++i)
            {
                Item item = caster.Items[i];

                if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
                {
                    Item clone = new Item(item.ItemID)
                    {
                        Layer = item.Layer,
                        Movable = false,
                        Hue = item.Hue
                    };

                    AddItem(clone);
                }
            }

            // Targeting mechanics: Attract nearby enemies
            AggroNearbyEnemies();
        }

        private void AggroNearbyEnemies()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m.Combatant == m_Caster)
                {
                    m.Combatant = this; // Redirect aggro to the illusion
                    m.SendMessage("You are distracted by an illusion!");
                }
            }
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();
            m_Caster.SendMessage("Your illusionary double vanishes.");
        }
    }
}
